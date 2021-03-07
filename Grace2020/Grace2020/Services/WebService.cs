using Grace2020.DependencyInjection;
using Grace2020.Models;
using Grace2020.Models.Tables;
using Grace2020.Resources;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Grace2020.Services
{
    public class WebService : IWebService
    {
        private IRestService _restService;
        public WebService()
        {
            _restService = new RestService();
        }

        public static async Task<bool> OpenUri(string link)
        {
            var uri = new Uri(link);
            return await OpenLink(uri);
        }

        public static async Task<bool> OpenEmail(string address)
        {
            var uri = new Uri("mailto:" + address);
            return await OpenLink(uri);
        }

        private static async Task<bool> OpenLink(Uri link)
        {
            try
            {
                var success = await Launcher.CanOpenAsync(link);
                if (success)
                {
                    await Launcher.OpenAsync(link);
                }
                return success;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        public async Task GetPrayersAsync()
        {
            await GetDataAsync<Prayer>(new string[] { "Prayers", GetTwoCharacterCultureCode()});
        }

        public async Task GetTopicsAsync()
        {
            await GetDataAsync<Topic>(new string[] { "Topics", GetTwoCharacterCultureCode() });
        }

        public async Task GetNewsAsync()
        {
            await GetDataAsync<News>(new string[] { "News", GetTwoCharacterCultureCode() });
        }

        public async Task GetNewsAssetLookupAsync()
        {
            await GetDataAsync<NewsAssetLookup>(new string[] { "ImagesLookup", ""});
        }

        public async Task GetModulesLookupAsync()
        {
            await GetDataAsync<ModulesLookup>(new string[] { "ModulesLookup", ""});
        }

        public async Task GetModulesAsync()
        {
            await GetDataAsync<Models.Tables.Module>(new string[] { "Modules", GetTwoCharacterCultureCode() });
        }

        public async Task GetEventsAsync()
        {
            await GetDataAsync<Event>(new string[] { "Events", GetTwoCharacterCultureCode() });
        }

        public async Task GetEventsImagesLookupAsync()
        {
            await GetDataAsync<EventAssetLookup>(new string[] { "EventsImagesLookup", "" });
        }

        private async Task GetDataAsync<TModel>(string[] binds) 
            where TModel : IModel, new()
        {
            var url = string.Format(Constants.GRACE2020ServiceUrl, new object[] { "GetData.php" });
            var json = await GetJsonFromFile("GetData");
            json = BindJson(json, binds);
            await _restService.RefreshDataAsync<TModel>(url, json);
        }

        private async Task<string> GetJsonFromFile(string filename)
        {
            try
            {
                var assembly = typeof(WebService).GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".JSON." + filename + ".json");
                string json = string.Empty;
                using (var reader = new StreamReader(stream))
                {
                    json = await reader.ReadToEndAsync();
                }

                return json;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
        }

        private string BindJson(string json, string[] binds)
        {
            JObject jobject = JObject.Parse(json);
            var i = 0;
            foreach(var jtoken in jobject.Children().ToList())
            {
                if (jtoken.ToString().Contains("{0}"))
                {
                    var newToken = JToken.FromObject(binds[i]);
                    jobject.SelectToken(jtoken.Path).Replace(newToken);
                    i++;
                }
            }
            return jobject.ToString();
        }

        private string GetTwoCharacterCultureCode()
        {
            var code = App.GetCultureInfo().TwoLetterISOLanguageName;
            if (code == "en")
            {
                return "en";
            }
            else if(code == "ja")
            {
                return "ja";
            }
            else
            {
                return "en";
            }
        }
    }
}
