﻿using KegID.Model;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace KegID.Common
{
    public class KegIDClient
    {
        private readonly HttpClient _client;

        public KegIDClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<KegIDResponse> Get(string Url, string Json)
        {
            KegIDResponse kegIDResponse = new KegIDResponse();
            var uri = new Uri(string.Format(Url, Json));

            try
            {
                var response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    kegIDResponse.Response = await response.Content.ReadAsStringAsync();
                }

                kegIDResponse.StatusCode = response.StatusCode.ToString();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }

            return kegIDResponse;
        }

        public async Task<KegIDResponse> Send(string Url, string Json, string RequestType)
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
                response = await _client.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    kegIDResponse.Response = await response.Content.ReadAsStringAsync();
                }

                kegIDResponse.StatusCode = response.StatusCode.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
            }
            return kegIDResponse;
        }

        public async Task<KegIDResponse> Post(string Url, string Json)
        {
            KegIDResponse kegIDResponse = new KegIDResponse();
            var uri = new Uri(string.Format(Url, string.Empty));

            try
            {
                var content = new StringContent(Json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                response = await _client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    kegIDResponse.Response = await response.Content.ReadAsStringAsync();
                }

                kegIDResponse.StatusCode = response.StatusCode.ToString();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
            }
            return kegIDResponse;
        }

        #region ExecuteCall

        public async Task<KegIDResponse> ExecuteServiceCall<T>(string url, HttpMethodType httpMethodType, string content, string RequestType = "")
        {
            KegIDResponse kegIDResponse = new KegIDResponse();
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
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
                kegIDResponse.StatusCode = System.Net.HttpStatusCode.Forbidden.ToString();

            return kegIDResponse;
        }

        #endregion

        public T DeserializeObject<T>(string response)
        {
            T type = default(T);
            try
            {
                return JsonConvert.DeserializeObject<T>(response, GetJsonSetting());
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                return type;
            }
        }

        public JsonSerializerSettings GetJsonSetting()
        {
            return new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                Converters = new List<JsonConverter> { new CustomIntConverter() }
            };
        }
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


