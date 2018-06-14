using System;
using System.Linq;
using System.Net.Http;
using Raven.Client.ServerWide.Operations;
using Vedaantees.Framework.Providers.Logging;

namespace Vedaantees.Framework.Providers
{
    public class ProviderVerifier
    {
        public bool IsSqlServiceRunning(string sqlStoreConnectionString, ILogger logger = null)
        {
            if (string.IsNullOrEmpty(sqlStoreConnectionString))
                return false;

            try
            {
                var dbName = sqlStoreConnectionString.Split(';').ToList().FirstOrDefault(p => p.Contains("Database"))?.Split('=')[1];
                var connection = new Npgsql.NpgsqlConnection(sqlStoreConnectionString);
                connection.Open();
                var cmd = new Npgsql.NpgsqlCommand($"SELECT datname FROM pg_database where datname = '{dbName}';", connection);
                cmd.ExecuteReader();
                connection.Close();

                logger?.Information("SQL STORE WORKING.");
            }
            catch (Exception e)
            {
                logger?.Error("SQL STORE DOWN. SERVICE DISABLED AUTOMATICALLY.");
                logger?.Error(e, $"ERROR: {1}", e);
                return false;
            }

            return true;
        }


        public bool IsNoSqlServiceRunning(string url, string username, string password, ILogger logger = null)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            try
            {
                var store = new Raven.Client.Documents.DocumentStore { Urls = new[] { url } };
                store.Initialize();
                store.Maintenance.Server.Send(new GetBuildNumberOperation());
                logger?.Information("NO-SQL STORE WORKING.");
            }
            catch (Exception e)
            {
                logger?.Error("DOCUMENT STORE DOWN. SERVICE DISABLED AUTOMATICALLY.");
                logger?.Error(e, $"ERROR: {1}", e);
                return false;
            }

            return true;
        }

        public bool IsGraphServiceRunning(string url, string username, string password, ILogger logger = null)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            try
            {
                var client = new HttpClient();
                var result = client.GetStringAsync($"{url}/db/data/").Result;
                logger?.Information("GRAPH STORE WORKING.");
            }
            catch (Exception e)
            {
                logger?.Error("GRAPH STORE DOWN. SERVICE DISABLED AUTOMATICALLY.");
                logger?.Error(e, $"ERROR: {1}", e);
                return false;
            }

            return true;
        }

        public bool IsQueueRunning(string busEndpoint, ILogger logger = null)
        {
            var factory = new RabbitMQ.Client.ConnectionFactory();
            factory.Uri = new Uri(busEndpoint);

            try
            {
                var conn = factory.CreateConnection();
                conn.Close();
                conn.Dispose();
                logger?.Information("QUEUE WORKING.");
            }
            catch (Exception ex)
            {
                logger?.Error("QUEUE DOWN. SERVICE DISABLED AUTOMATICALLY.");
                logger?.Error(ex, $"ERROR: {1}", ex);
                return false;
            }

            return true;
        }
    }
}