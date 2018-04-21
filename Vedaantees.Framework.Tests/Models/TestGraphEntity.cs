using Vedaantees.Framework.Providers.Storages;
using Vedaantees.Framework.Providers.Storages.Graphs;
using Neo4jClient;

namespace Vedaantees.Framework.Tests.Models
{
    public class TestGraphEntity : IEntity<string>
    {

        public string Id { get; set; }
        public string Date { get; set; }
        public string Name { get; set; }
    }
}