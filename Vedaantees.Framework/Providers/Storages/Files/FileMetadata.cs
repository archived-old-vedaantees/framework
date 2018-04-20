using System;

namespace Vedaantees.Framework.Providers.Storages.Files
{
    public class FileMetadata
    {
        public string Filename { get; set; }
        public string Extension { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}