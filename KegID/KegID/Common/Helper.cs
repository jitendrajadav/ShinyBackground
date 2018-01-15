using KegID.Model;
using KegID.Services;
using KegID.SQLiteClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace KegID.Common
{
    public class Helper
    {
        static HttpClient client;
        static Helper()
        {
            client = new HttpClient
            {
                MaxResponseContentBufferSize = 999999999
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic");
        }

        public static async Task<string> Get(string Url, string Json)
        {
            string data = string.Empty;
            var uri = new Uri(string.Format(Url, Json));

            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                    data = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }

            return data;
        }
        public static async Task<string> Post(string Url, string Json)
        {
            string data = string.Empty;
            var uri = new Uri(string.Format(Url, string.Empty));

            try
            {
                var content = new StringContent(Json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                    data = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
            }
            return data;
        }

        #region ExecutePostCall

        public static async Task<T> ExecutePostCall<T>(string url, HttpMethodType httpMethodType,string content)
        {
            T response = default(T);
            try
            {
                switch (httpMethodType)
                {
                    case HttpMethodType.Get:
                        content = await Get(url, content);
                        break;
                    case HttpMethodType.Post:
                        content = await Post(url, content);
                        break;
                    case HttpMethodType.Put:
                        break;
                    case HttpMethodType.Delete:
                        break;
                    default:
                        break;
                }

                if (!string.IsNullOrEmpty(content))
                    response = JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return response;
        }

        #endregion

    }

}
