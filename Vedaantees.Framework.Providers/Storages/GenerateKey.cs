using Vedaantees.Framework.Providers.Storages.Keys;
using Raven.Client.Documents;
using Raven.Client.Documents.Commands;
using Sparrow.Json;

namespace Vedaantees.Framework.Providers.Storages
{
    public class GenerateKey : IGenerateKey
    {
        private readonly IDocumentStore _documentStore;

        public GenerateKey(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public long GetNextNumericalKey(string collectionName)
        {
            using (var shortTermSingleUse = JsonOperationContext.ShortTermSingleUse())
            {
                var command = new NextIdentityForCommand(collectionName);
                _documentStore.GetRequestExecutor("Master").Execute(command, shortTermSingleUse);
                return command.Result;
            }
        }
        
        public string GetNextStringKey(string collectionName)
        {
            return $"{collectionName}-{GetNextNumericalKey(collectionName)}";
        }
    }
}