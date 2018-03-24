using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace httpClient.core.Clients
{
    public interface IProxiedHttpClient
    {
        HttpClient Instance { get; }
    }
    public class ProxiedHttpClient : IProxiedHttpClient
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private readonly Uri _proxyUri;
        private bool _useProxy;

        public HttpClient Instance { get { return this._httpClient; } }
        public Uri ProxyUrl { get { return this._proxyUri; } }

        public bool UseProxy { get { return this._useProxy; } }
        public ProxiedHttpClient(IConfiguration config)
        {
            this._config = config;

            HttpClientHandler httpClientHandler = null;

            var useProxyString = this._config.GetSection("proxySettings").GetSection("useProxy").Value;
            if (bool.TryParse(useProxyString, out this._useProxy) && this._useProxy)
            {
                var proxyUriString = this._config.GetSection("proxySettings").GetSection("proxyAddress").Value;


                if (System.Uri.TryCreate(proxyUriString, UriKind.Absolute, out this._proxyUri))
                {
                    httpClientHandler = new HttpClientHandler
                    {
                        Proxy = new WebProxy(this._proxyUri, true)
                        {
                            UseDefaultCredentials = true
                        }
                    };

                }
            }


            if (httpClientHandler != null)
            {
                this._httpClient = new HttpClient(httpClientHandler);
            }
            else
            {
                this._httpClient = new HttpClient();
            }



        }


    }
}