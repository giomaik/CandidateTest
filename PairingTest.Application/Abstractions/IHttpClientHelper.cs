using System.Net.Http;
using System.Threading.Tasks;

namespace PairingTest.Application.Abstractions
{
    public interface IHttpClientHelper
    {
        Task<HttpResponseMessage> GetAsync(string path);
    }
}