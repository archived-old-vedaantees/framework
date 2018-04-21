namespace Vedaantees.Framework.Tests.Models
{
    public class TestRelationEntity
    {
        public TestGraphEntity Source { get; set; }
        public TestGraphEntity Destination { get; set; }
        public string Id { get; set; }
        public string GetLabel() => "TestRelation";
    }
}