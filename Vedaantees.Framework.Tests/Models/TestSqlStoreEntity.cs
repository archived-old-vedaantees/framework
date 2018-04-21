using System.ComponentModel.DataAnnotations.Schema;
using Vedaantees.Framework.Providers.Storages;

namespace Vedaantees.Framework.Tests.Models
{
    [Table("tests")]
    public partial class TestSqlStoreEntity : IEntity<long>
    {
        [Column("id")]
        public long Id { get; set; }

        [Column("name")]
        public string Name { get; set; }
    }
} 