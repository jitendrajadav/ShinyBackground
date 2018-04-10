﻿using KegID.Model;
using KegID.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace KegID.Common
{
    public enum HttpMethodType
    {
        Get,
        Post,
        Send,
        Put,
        Delete
    }

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

        public static async Task<KegIDResponse> Get(string Url, string Json)
        {
            KegIDResponse kegIDResponse = new KegIDResponse();
            var uri = new Uri(string.Format(Url, Json));

            try
            {
                    var response = await client.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                        kegIDResponse.Response = await response.Content.ReadAsStringAsync();
                    kegIDResponse.StatusCode = response.StatusCode;
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }

            return kegIDResponse;
        }

        public static async Task<KegIDResponse> Send(string Url, string Json, string RequestType)
        {
            KegIDResponse kegIDResponse = new KegIDResponse();
            var uri = new Uri(string.Format(Url, string.Empty));

            try
            {
                var content = new StringContent(Json, Encoding.UTF8, "application/json");

                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
                requestMessage.Headers.Add("Request-type", RequestType);
                requestMessage.Content = content;

                HttpResponseMessage response = null;
                response = await client.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                    kegIDResponse.Response = await response.Content.ReadAsStringAsync();
                kegIDResponse.StatusCode = response.StatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
            }
            return kegIDResponse;
        }

        public static async Task<KegIDResponse> Post(string Url, string Json)
        {
            KegIDResponse kegIDResponse = new KegIDResponse();
            var uri = new Uri(string.Format(Url, string.Empty));

            try
            {
                var content = new StringContent(Json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                    kegIDResponse.Response = await response.Content.ReadAsStringAsync();
                kegIDResponse.StatusCode = response.StatusCode;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
            }
            return kegIDResponse;
        }

        #region ExecuteCall

        public static async Task<KegIDResponse> ExecuteServiceCall<T>(string url, HttpMethodType httpMethodType, string content, string RequestType = "")
        {
            KegIDResponse kegIDResponse = new KegIDResponse();

            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    switch (httpMethodType)
                    {
                        case HttpMethodType.Get:
                            kegIDResponse = await Get(url, content);
                            break;
                        case HttpMethodType.Send:
                            kegIDResponse = await Send(url, content, RequestType);
                            break;
                        case HttpMethodType.Post:
                            kegIDResponse = await Post(url, content);
                            break;
                        case HttpMethodType.Put:
                            break;
                        case HttpMethodType.Delete:
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
            else
                kegIDResponse.StatusCode = System.Net.HttpStatusCode.Forbidden;

            return kegIDResponse;
        }

        #endregion

        public static T DeserializeObject<T>(string response, JsonSerializerSettings setting)
        {
            T type = default(T);
            try
            {
                return JsonConvert.DeserializeObject<T>(response,setting);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return type;
            }
        }
        public static JsonSerializerSettings GetJsonSetting()
        {
            return new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                Converters = new List<JsonConverter> { new CustomIntConverter() }
            };
        }

        public class CustomIntConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType) => (objectType == typeof(int));

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                JValue jsonValue = serializer.Deserialize<JValue>(reader);

                if (jsonValue.Type == JTokenType.Float)
                {
                    return (int)Math.Round(jsonValue.Value<double>());
                }
                else if (jsonValue.Type == JTokenType.Integer)
                {
                    return jsonValue.Value<int>();
                }

                throw new FormatException();
            }


            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                
            }
        }

    }
}
