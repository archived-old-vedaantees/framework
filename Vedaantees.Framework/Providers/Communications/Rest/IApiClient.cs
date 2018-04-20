using System;
using System.Threading.Tasks;

namespace Vedaantees.Framework.Providers.Communications.Rest
{
    public interface IApiClient
    {
        Task<TResponse> Get<TResponse>(Uri url);
        Task<TResponse> Post<TRequest, TResponse>(Uri url, string resourceName, TRequest request = default(TRequest));
    }

    public class EnableApiAttribute : Attribute
    {
        public string Route { get; set; }
        public string PermissionIdentifier { get; set; }
    }
}