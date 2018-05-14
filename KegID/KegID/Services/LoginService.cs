﻿//using Akavache;
//using Fusillade;
//using KegID.Dtos;
//using Plugin.Connectivity;
//using Polly;
//using System;
//using System.Reactive.Linq;
//using System.Threading.Tasks;

//namespace KegID.Services
//{
//    public class LoginService : ILoginService
//    {
//        private readonly IApiService _apiService;

//        public LoginService(IApiService apiService)
//        {
//            _apiService = apiService;
//        }

//        //public async Task<List<ConferenceDto>> GetConferences(Priority priority)
//        //{
//        //    var cache = BlobCache.LocalMachine;
//        //    var cachedConferences = cache.GetAndFetchLatest("conferences", () => GetRemoteConferencesAsync(priority),
//        //        offset =>
//        //        {
//        //            TimeSpan elapsed = DateTimeOffset.Now - offset;
//        //            return elapsed > new TimeSpan(hours: 24, minutes: 0, seconds: 0);
//        //        });

//        //    var conferences = await cachedConferences.FirstOrDefaultAsync();
//        //    return conferences;
//        //}

//        public async Task<LoginDto> GetLogin(Priority priority, string username,string password)
//        {
//            var cachedConference = BlobCache.LocalMachine.GetAndFetchLatest(username, () => GetRemoteLogin(priority, username,password), offset =>
//            {
//                TimeSpan elapsed = DateTimeOffset.Now - offset;
//                return elapsed > new TimeSpan(hours: 0, minutes: 30, seconds: 0);
//            });

//            var conference = await cachedConference.FirstOrDefaultAsync();

//            return conference;
//        }


//        //private async Task<List<ConferenceDto>> GetRemoteConferencesAsync(Priority priority)
//        //{
//        //    List<ConferenceDto> conferences = null;
//        //    Task<List<ConferenceDto>> getConferencesTask;
//        //    switch (priority)
//        //    {
//        //        case Priority.Background:
//        //            getConferencesTask = _apiService.Background.GetConferences();
//        //            break;
//        //        case Priority.UserInitiated:
//        //            getConferencesTask = _apiService.UserInitiated.GetConferences();
//        //            break;
//        //        case Priority.Speculative:
//        //            getConferencesTask = _apiService.Speculative.GetConferences();
//        //            break;
//        //        default:
//        //            getConferencesTask = _apiService.UserInitiated.GetConferences();
//        //            break;
//        //    }

//        //    if (CrossConnectivity.Current.IsConnected)
//        //    {
//        //        conferences = await Policy
//        //              .Handle<WebException>()
//        //              .WaitAndRetry
//        //              (
//        //                retryCount: 5,
//        //                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
//        //              )
//        //              .ExecuteAsync(async () => await getConferencesTask);
//        //    }
//        //    return conferences;
//        //}

//        public async Task<LoginDto> GetRemoteLogin(Priority priority, string username, string password)
//        {
//            LoginDto login   = null;

//            Task<LoginDto> getConferenceTask;
//            switch (priority)
//            {
//                case Priority.Background:
//                    getConferenceTask = _apiService.Background.GetLogin(username,password);
//                    break;
//                case Priority.UserInitiated:
//                    getConferenceTask = _apiService.UserInitiated.GetLogin(username, password);
//                    break;
//                case Priority.Speculative:
//                    getConferenceTask = _apiService.Speculative.GetLogin(username, password);
//                    break;
//                default:
//                    getConferenceTask = _apiService.UserInitiated.GetLogin(username, password);
//                    break;
//            }

//            if (CrossConnectivity.Current.IsConnected)
//            {
//                login = await Policy
//                    .Handle<Exception>()
//                    .RetryAsync(retryCount: 5)
//                    .ExecuteAsync(async () => await getConferenceTask);
//            }

//            return login;
//        }

//    }

//}
