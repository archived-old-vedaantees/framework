namespace Vedaantees.Framework.Providers.Mailing.Models
{
    public class EmbeddedAttachment
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Path { get; set; }

        //Use MediaTypeNames static class for this.
        public string MediaType { get; set; }
    }
}