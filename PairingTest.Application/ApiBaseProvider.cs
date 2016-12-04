using PairingTest.Application.Abstractions;
using System.Configuration;

namespace PairingTest.Application
{
    public class ApiBaseUrlProvider : IApiBaseUrlProvider
    {
        public string ApiBaseUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["ApiBaseUrl"];
            }
        }
    }
}