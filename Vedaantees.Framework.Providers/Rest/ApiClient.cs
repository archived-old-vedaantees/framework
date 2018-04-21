using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json;
using Vedaantees.Framework.Providers.Communications.Rest;

namespace Vedaantees.Framework.Providers.Rest
{
    public class ApiClient : IApiClient
    {
        private readonly ApiClientConfiguration _configuration;

        public ApiClient(ApiClientConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<TResponse> Get<TResponse>(Uri url)
        {
            var discovery = await DiscoveryClient.GetAsync(_configuration.SingleSignOnServiceUrl);
            var tokenClient = new TokenClient(discovery.TokenEndpoint, _configuration.ClientId, _configuration.ClientSecret);
            var tokenResponse = tokenClient.RequestClientCredentialsAsync(_configuration.Client).Result;
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception(response.ReasonPhrase);

            var jsonObject = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(jsonObject);
        }

        public async Task<TResponse> Post<TRequest, TResponse>(Uri url, string resourceName, TRequest request = default(TRequest))
        {
            var discovery = await DiscoveryClient.GetAsync(_configuration.SingleSignOnServiceUrl);
            var tokenClient = new TokenClient(discovery.TokenEndpoint, _configuration.ClientId, _configuration.ClientSecret);
            var tokenResponse = tokenClient.RequestClientCredentialsAsync(resourceName).Result;
            var client = new HttpClient();
            var serializeObject = JsonConvert.SerializeObject(request);

            client.SetBearerToken(tokenResponse.AccessToken);
            var response = await client.PostAsync(url, new StringContent(serializeObject, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                throw new Exception(response.ReasonPhrase);

            var jsonObject = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(jsonObject);
        }
    }
}
