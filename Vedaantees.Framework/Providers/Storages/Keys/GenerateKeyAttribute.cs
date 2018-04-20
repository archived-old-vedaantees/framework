using System;

namespace Vedaantees.Framework.Providers.Storages.Keys
{
    public class GenerateKeyAttribute : Attribute
    {
        public GenerateKeyAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }

        public string CollectionName { get; }
    }
}