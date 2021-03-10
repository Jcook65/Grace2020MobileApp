using Grace2020.DependencyInjection;
using Grace2020.Models;
using Grace2020.Models.Tables;
using Grace2020.Resources;
using Grace2020.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Grace2020.Services
{
    public class RestService : IRestService
    {
        private static SemaphoreSlim _configSemaphore = new SemaphoreSlim(1, 1);

        //public event EventHandler Errored;
        
        public RestService()
        {
        }

        public async Task<HttpContent> GetRequest(string url)
        {
            using(var client = new HttpClient(new RetryHandler(new HttpClientHandler())))
            {
                var response = await client.GetAsync(url);
                return response.Content;
            }
        }

        public async Task RefreshDataAsync<TModel>(string url, string jsonPackage ) where TModel : IModel, new()
        {
            var uri = new Uri(string.Format(url, string.Empty));
            try
            {
                HttpResponseMessage response = null;
                using(var client = new HttpClient(new RetryHandler(new HttpClientHandler())))
                {
                    response = await client.PostAsync(uri, new StringContent(jsonPackage, Encoding.UTF8, "application/json"));
                }

                if (response != null && response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var models = JsonConvert.DeserializeObject<List<TModel>>(content);
                    using(var db = new DbUtil())
                    {
                        try
                        {
                            await _configSemaphore.WaitAsync();
                            await db.AsyncConnection.DeleteAllAsync<TModel>();
                            await db.AsyncConnection.InsertAllAsync(models);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex);
                        }
                        finally
                        {
                            _configSemaphore.Release();
                            response.Dispose();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"\tERROR {ex.Message}");
            }
        }
    }

    public class RetryHandler : DelegatingHandler
    {
        private const int MaxRetries = 3;

        public RetryHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {

        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            for(int i =0; i< MaxRetries; i++)
            {
                try
                {
                    response = await base.SendAsync(request, cancellationToken);
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex);
                    response = null;
                }
                if (response != null && response.IsSuccessStatusCode)
                {
                    return response;
                }
            }
            return response;
        }
    }
}
