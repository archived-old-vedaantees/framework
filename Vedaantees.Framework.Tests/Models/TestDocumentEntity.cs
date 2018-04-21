using System.ComponentModel.DataAnnotations;
using Vedaantees.Framework.Providers.Storages;
using Vedaantees.Framework.Providers.Storages.Data;

namespace Vedaantees.Framework.Tests.Models
{
    public class TestDocumentEntity : IEntity<string>
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }
    }
}