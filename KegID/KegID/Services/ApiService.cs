using System;
using System.Net.Http;
using Fusillade;
using ModernHttpClient;
using Refit;

namespace KegID.Services
{
    public class ApiService : IApiService
    {
        public const string ApiBaseAddress = Configuration.TestAPIUrl;//"http://api.tekconf.com/v1";

        public ApiService(string apiBaseAddress = null)
        {
            Func<HttpMessageHandler, ITekAccApi> createClient = messageHandler =>
            {
                var client = new HttpClient(messageHandler)
                {
                    BaseAddress = new Uri(apiBaseAddress ?? ApiBaseAddress)
                };

                return RestService.For<ITekAccApi>(client);
            };

            _background = new Lazy<ITekAccApi>(() => createClient(
                new RateLimitedHttpMessageHandler(new NativeMessageHandler(), Priority.Background)));

            _userInitiated = new Lazy<ITekAccApi>(() => createClient(
                new RateLimitedHttpMessageHandler(new NativeMessageHandler(), Priority.UserInitiated)));

            _speculative = new Lazy<ITekAccApi>(() => createClient(
                new RateLimitedHttpMessageHandler(new NativeMessageHandler(), Priority.Speculative)));
        }

        private readonly Lazy<ITekAccApi> _background;
        private readonly Lazy<ITekAccApi> _userInitiated;
        private readonly Lazy<ITekAccApi> _speculative;

        public ITekAccApi Background
        {
            get { return _background.Value; }
        }

        public ITekAccApi UserInitiated
        {
            get { return _userInitiated.Value; }
        }

        public ITekAccApi Speculative
        {
            get { return _speculative.Value; }
        }
    }

}
