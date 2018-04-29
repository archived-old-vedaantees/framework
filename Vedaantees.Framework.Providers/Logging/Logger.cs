using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Vedaantees.Framework.Providers.FileSystem;
using Vedaantees.Framework.Providers.Mailing;

namespace Vedaantees.Framework.Providers.Logging
{
    /// <summary>
    /// Logger implementation for ILogger. Wraps implementation for Serilog.
    /// </summary>
    public class Logger : ILogger, IDisposable
    {
        private readonly string _appName;
        private readonly string _path;

        public Logger(LoggerConfiguration configuration, string appName, EmailSettings emailSettings = null, string path = null)
        {
            _appName = appName;
            _path = path;
            var loggerConfiguration = Configure(configuration, emailSettings);
            Log.Logger = loggerConfiguration.CreateLogger();
        }

        private Serilog.LoggerConfiguration Configure(LoggerConfiguration settings, EmailSettings emailSettings)
        {
            FolderManager.CheckAndCreateDirectory($@"{_path}\logs");

            var seriLogConfig = new Serilog.LoggerConfiguration()
                                           .Enrich
                                           .WithExceptionDetails();

            if (settings.EnableSerilogDebugger)
            {
                var file = File.CreateText($@"{_path}\logs\serilog.txt");
                Serilog.Debugging.SelfLog.Enable(TextWriter.Synchronized(file));
            }

            if (settings.EnableFile)
                seriLogConfig.WriteTo.RollingFile($@"{_path}\logs\log-{{Date}}.txt");
            
            if (settings.EnableEmail && emailSettings != null && !string.IsNullOrEmpty(settings.LoggingFromEmail) && !string.IsNullOrEmpty(settings.LoggingToEmail))
                seriLogConfig.WriteTo.Email(fromEmail:         settings.LoggingFromEmail,
                                            toEmail:           settings.LoggingToEmail,
                                            mailServer:        emailSettings.Server,
                                            networkCredential: new NetworkCredential
                                                               {
                                                                    Domain   = emailSettings.Domain,
                                                                    UserName = emailSettings.Username,
                                                                    Password = emailSettings.Password
                                                               },
                                            restrictedToMinimumLevel: LogEventLevel.Error);
            
            return seriLogConfig;
        }

        public void Verbose(string messageTemplate, params object[] propertyValues)
        {
            var customizedPropertyValues = new List<object>(propertyValues) { _appName }.ToArray();
            Log.Logger.Verbose("{_appName} - " + messageTemplate, _appName, customizedPropertyValues);
        }

        public void Verbose(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            var customizedPropertyValues = new List<object>(propertyValues) { _appName, exception.ToString() }.ToArray();
            Log.Logger.Error("{_appName} - {exception}" + messageTemplate, customizedPropertyValues);
        }

        public void Debug(string messageTemplate, params object[] propertyValues)
        {
            var customizedPropertyValues = new List<object>(propertyValues) { _appName }.ToArray();
            Log.Logger.Debug("{_appName} - " + messageTemplate, _appName, customizedPropertyValues);
        }

        public void Debug(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            var customizedPropertyValues = new List<object>(propertyValues) { _appName, exception.ToString() }.ToArray();
            Log.Logger.Error("{_appName} - {exception}" + messageTemplate, customizedPropertyValues);
        }

        public void Information(string messageTemplate, params object[] propertyValues)
        {
            var customizedPropertyValues = new List<object>(propertyValues) { _appName }.ToArray();
            Log.Logger.Information("{_appName} - " + messageTemplate, _appName, customizedPropertyValues);
        }

        public void Information(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            var customizedPropertyValues = new List<object>(propertyValues) { _appName, exception.ToString() }.ToArray();
            Log.Logger.Error("{_appName} - {exception}" + messageTemplate, customizedPropertyValues);
        }

        public void Warning(string messageTemplate, params object[] propertyValues)
        {
            var customizedPropertyValues = new List<object>(propertyValues) { _appName }.ToArray();
            Log.Logger.Warning("{_appName} - " + messageTemplate, _appName, customizedPropertyValues);
        }

        public void Warning(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            var customizedPropertyValues = new List<object>(propertyValues) { _appName, exception.ToString() }.ToArray();
            Log.Logger.Error("{_appName} - {exception}" + messageTemplate, customizedPropertyValues);
        }

        public void Error(string messageTemplate, params object[] propertyValues)
        {
            var customizedPropertyValues = new List<object>(propertyValues) { _appName }.ToArray();
            Log.Logger.Error("{_appName} - " + messageTemplate, _appName, customizedPropertyValues);
        }

        public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            var customizedPropertyValues = new List<object>(propertyValues) { _appName, exception.ToString() }.ToArray();
            Log.Logger.Error("{_appName} - {exception}" + messageTemplate, customizedPropertyValues);
        }

        public void Fatal(string messageTemplate, params object[] propertyValues)
        {
            var customizedPropertyValues = new List<object>(propertyValues) { _appName }.ToArray();
            Log.Logger.Fatal("{_appName} - " + messageTemplate, _appName, customizedPropertyValues);
        }

        public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            var customizedPropertyValues = new List<object>(propertyValues) { _appName, exception.ToString() }.ToArray();
            Log.Logger.Error("{_appName} - {exception}" + messageTemplate, customizedPropertyValues);
        }

        public void Dispose()
        {
            Log.CloseAndFlush();
        }
    }
}
