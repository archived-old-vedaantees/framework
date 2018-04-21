using System.Linq;
using Vedaantees.Framework.Providers.Storages.Data;
using Vedaantees.Framework.Types.Results;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Vedaantees.Framework.Providers.Storages;

namespace Vedaantees.Framework.Providers.SingleSignOn
{
    public class SingleSignOnRegistrar
    {
        private readonly IDocumentStore _documentStore;
        private readonly IConfigurationRoot _configuration;

        public SingleSignOnRegistrar(IDocumentStore documentStore, IConfigurationRoot configuration)
        {
            _documentStore = documentStore;
            _configuration = configuration;
            _documentStore.SetSession("Master");
        }

        public MethodResult RegisterClient()
        {
            var client = _configuration.GetSection("SingleSignOnClient").Get<SingleSignOnClient>();
            var existingSetting = _documentStore.Find<SingleSignOnClient>(p => p.Id == client.Id).FirstOrDefault();

            if (existingSetting != null)
                _documentStore.Remove<SingleSignOnClient>(existingSetting.Id);

            _documentStore.Store(client);
            return new MethodResult(MethodResultStates.Successful);
        }

        public MethodResult RegisterResource()
        {
            var apiResource = _configuration.GetSection("SingleSignOnResource").Get<SingleSignOnApiResource>();
            var existingSetting = _documentStore.Find<SingleSignOnApiResource>(p => p.Id == apiResource.Id).FirstOrDefault();

            if (existingSetting != null)
                _documentStore.Remove<SingleSignOnApiResource>(existingSetting.Id);

            _documentStore.Store(apiResource);
            return new MethodResult(MethodResultStates.Successful);
        }
    }
    
    public class SingleSignOnClient:IEntity<string>
    {
        public SingleSignOnClient()
        {
            RequiredScopes = new List<string>();
        }

        public string Id     { get; set; }
        public string Name   { get; set; }
        public string Secret { get; set; }
        public List<string> RequiredScopes { get; set; }
    }

    public class SingleSignOnApiResource: IEntity<string>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<ResourceScope> Scopes { get; set; }
    }

    public class ResourceScope
    {
        public string Name { get; set; }
    }
}
