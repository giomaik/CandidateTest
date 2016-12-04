using PairingTest.Application.Abstractions;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PairingTest.Application
{
    public class HttpClientHelper : IHttpClientHelper
    {
        private readonly HttpClient _client = new HttpClient();

        public HttpClientHelper(IApiBaseUrlProvider baseUrlProvider)
        {
            _client.BaseAddress = new Uri(baseUrlProvider.ApiBaseUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<HttpResponseMessage> GetAsync(string path)
        {
            return await _client.GetAsync(path);
        }
    }
}