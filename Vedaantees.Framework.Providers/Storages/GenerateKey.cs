using Vedaantees.Framework.Providers.Storages.Data;
using Vedaantees.Framework.Providers.Storages.Keys;

namespace Vedaantees.Framework.Providers.Storages
{
    //TODO: Make this obsolete. Use IDocumentStore instead.
    public class GenerateKey : IGenerateKey
    {
        private readonly IDocumentStore _documentStore;

        public GenerateKey(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public long GetNextNumericalKey(string collectionName)
        {
            return _documentStore.GetNextNumericalKey(collectionName);
        }
        
        public string GetNextStringKey(string collectionName)
        {
            return _documentStore.GetNextStringKey(collectionName);
        }
    }
}