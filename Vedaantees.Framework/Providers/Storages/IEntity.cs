using System.Collections.Generic;
using System.IO;
using Vedaantees.Framework.Providers.Storages.Files;

namespace Vedaantees.Framework.Providers.Storages
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }

    public interface IEntityWithAttachments<T> : IEntity<T>
    {
        Dictionary<FileMetadata, Stream> Attachments { get; set; }
    }
}