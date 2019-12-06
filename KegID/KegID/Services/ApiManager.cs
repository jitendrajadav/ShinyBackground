using Acr.UserDialogs;
using Fusillade;
using KegID.Messages;
using KegID.Model;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Polly;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KegID.Services
{
    public class ApiManager : IApiManager
    {
        private readonly IUserDialogs _userDialogs = UserDialogs.Instance;
        private readonly IConnectivity _connectivity = CrossConnectivity.Current;
        private readonly IApiService<IAccountApi> _accountApi;
        private readonly IApiService<IDashboardApi> _dashboardApi;
        private readonly IApiService<IFillApi> _fillApi;
        private readonly IApiService<IMaintainApi> _maintainApi;
        private readonly IApiService<IMoveApi> _moveApi;
        private readonly IApiService<IPalletApi> _palletApi;


        public bool IsConnected { get; set; }
        public bool IsReachable { get; set; }
        private readonly Dictionary<int, CancellationTokenSource> runningTasks = new Dictionary<int, CancellationTokenSource>();
        //private readonly Dictionary<string, Task<HttpResponseMessage>> taskContainer = new Dictionary<string, Task<HttpResponseMessage>>();

        public ApiManager(IApiService<IAccountApi> accountApi, IApiService<IDashboardApi> dashboardApi, IApiService<IFillApi> fillApi, IApiService<IMaintainApi> maintainApi, IApiService<IMoveApi> moveApi, IApiService<IPalletApi> palletApi)
        {
            _accountApi = accountApi;
            _dashboardApi = dashboardApi;
            _fillApi = fillApi;
            _maintainApi = maintainApi;
            _moveApi = moveApi;
            _palletApi = palletApi;

            IsConnected = _connectivity.IsConnected;
            _connectivity.ConnectivityChanged += OnConnectivityChanged;
        }

        private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsConnected = e.IsConnected;

            if (!e.IsConnected)
            {
                // Cancel All Running Task
                var items = runningTasks.ToList();
                foreach (var item in items)
                {
                    item.Value.Cancel();
                    runningTasks.Remove(item.Key);
                }
            }
        }

        protected async Task<TData> RemoteRequestAsync<TData>(Task<TData> task)
            where TData : HttpResponseMessage,
            new()
        {
            TData data = new TData();

            if (!IsConnected)
            {
                var strngResponse = "There's not a network connection";
                data.StatusCode = HttpStatusCode.BadRequest;
                data.Content = new StringContent(strngResponse);

                _userDialogs.Toast(strngResponse, TimeSpan.FromSeconds(1));
                return data;
            }

            data = await Policy
            .Handle<WebException>()
            .Or<ApiException>()
            .Or<TaskCanceledException>()
            .Or<ValidationApiException>()
            .WaitAndRetryAsync
            (
                retryCount: 1,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
            )
            .ExecuteAsync(async () =>
            {
                var result = await task;
                if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    //Logout the user
                    MessagingCenter.Send(new InvalidServiceCall { IsInvalidCall = true }, "InvalidServiceCall");

                    // Cancel All Running Task
                    var items = runningTasks.ToList();
                    foreach (var item in items)
                    {
                        item.Value.Cancel();
                        runningTasks.Remove(item.Key);
                    }
                }
                runningTasks.Remove(task.Id);

                return result;
            });

            return data;
        }

        #region Account Api

        public async Task<HttpResponseMessage> PostDeviceCheckin(DeviceCheckinRequestModel model, string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_accountApi.GetApi(Priority.UserInitiated).PostDeviceCheckin(model, sessionId, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> GetAuthenticate(string username, string password)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_accountApi.GetApi(Priority.UserInitiated).GetAuthenticate(username, password, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        #endregion

        #region Dashboard Api

        public async Task<HttpResponseMessage> GetDashboardPartnersList(string ownerId, string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_dashboardApi.GetApi(Priority.UserInitiated).GetDashboardPartnersList(ownerId, sessionId, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> GetDeshboardDetail(string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_dashboardApi.GetApi(Priority.UserInitiated).GetDeshboardDetail(sessionId, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> GetInventory(string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_dashboardApi.GetApi(Priority.UserInitiated).GetInventory(sessionId, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> GetKegPossession(string sessionId, string partnerId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_dashboardApi.GetApi(Priority.UserInitiated).GetKegPossession(sessionId, partnerId, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> GetPartnerInfo(string sessionId, string partnerId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_dashboardApi.GetApi(Priority.UserInitiated).GetPartnerInfo(sessionId, partnerId, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> GetKegStatus(string kegId, string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_dashboardApi.GetApi(Priority.UserInitiated).GetKegStatus(kegId, sessionId, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> GetKegMaintenanceHistory(string kegId, string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_dashboardApi.GetApi(Priority.UserInitiated).GetKegMaintenanceHistory(kegId, sessionId, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> GetKegMaintenanceAlert(string kegId, string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_dashboardApi.GetApi(Priority.UserInitiated).GetKegMaintenanceAlert(kegId, sessionId, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> GetDeletedMaintenanceAlert(string kegId, string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_dashboardApi.GetApi(Priority.UserInitiated).GetDeletedMaintenanceAlert(kegId, sessionId, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> GetPalletSearch(string sessionId, string locationId, string fromDate, string toDate, string kegs, string kegOwnerId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_dashboardApi.GetApi(Priority.UserInitiated).GetPalletSearch(sessionId, locationId, fromDate, toDate, kegs, kegOwnerId, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> GetKegSearch(string sessionId, string barcode, bool includePartials)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_dashboardApi.GetApi(Priority.UserInitiated).GetKegSearch(sessionId, barcode, includePartials, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> GetAssetVolume(string sessionId, bool assignableOnly)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_dashboardApi.GetApi(Priority.UserInitiated).GetAssetVolume(sessionId, assignableOnly, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> GetOperators(string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_dashboardApi.GetApi(Priority.UserInitiated).GetOperators(sessionId, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> PostKegStatus(KegRequestModel model, string kegId, string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_dashboardApi.GetApi(Priority.UserInitiated).PostKegStatus(model, kegId, sessionId, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> PostKeg(KegRequestModel model, string kegId, string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_dashboardApi.GetApi(Priority.UserInitiated).PostKeg(model, string.Empty, sessionId, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> PostMaintenanceAlert(AddMaintenanceAlertRequestModel model, string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_dashboardApi.GetApi(Priority.UserInitiated).PostMaintenanceAlert(model, sessionId, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> PostMaintenanceDeleteAlertUrl(DeleteMaintenanceAlertRequestModel model, string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_dashboardApi.GetApi(Priority.UserInitiated).PostMaintenanceDeleteAlertUrl(model, sessionId, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> PostKegUpload(KegBulkUpdateItemRequestModel model, string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_dashboardApi.GetApi(Priority.UserInitiated).PostKegUpload(model, sessionId, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        #endregion

        #region Fill Api

        public async Task<HttpResponseMessage> GetBatchList(string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_fillApi.GetApi(Priority.UserInitiated).GetBatchList(sessionId, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> GetSkuList(string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_fillApi.GetApi(Priority.UserInitiated).GetSkuList(sessionId, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        #endregion

        #region Maintain Api

        public async Task<HttpResponseMessage> GetMaintainType(string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_maintainApi.GetApi(Priority.UserInitiated).GetMaintainType(sessionId, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> PostMaintenanceDone(MaintenanceDoneRequestModel model, string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_maintainApi.GetApi(Priority.UserInitiated).PostMaintenanceDone(model, sessionId, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }


        #endregion

        #region Move Api

        public async Task<HttpResponseMessage> GetOwner(string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_moveApi.GetApi(Priority.UserInitiated).GetOwner(sessionId, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> GetPartnersList(string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_moveApi.GetApi(Priority.UserInitiated).GetPartnersList(sessionId, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> GetValidateBarcode(string barcode, string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_moveApi.GetApi(Priority.UserInitiated).GetValidateBarcode(barcode, sessionId, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> GetBrandList(string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_moveApi.GetApi(Priority.UserInitiated).GetBrandList(sessionId, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> GetManifest(string manifestId, string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_moveApi.GetApi(Priority.UserInitiated).GetManifest(manifestId, sessionId, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> GetPartnerType(string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_moveApi.GetApi(Priority.UserInitiated).GetPartnerType(sessionId, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> GetPartnerSearch(string sessionId, string search, bool internalonly, bool includepublic)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_moveApi.GetApi(Priority.UserInitiated).GetPartnerSearch(sessionId, search, internalonly, includepublic, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> GetManifestSearch(string sessionId, string trackingNumber, string barcode, string senderId, string destinationId, string referenceKey, string fromDate, string toDate)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_moveApi.GetApi(Priority.UserInitiated).GetManifestSearch(sessionId, trackingNumber, barcode, senderId, destinationId, referenceKey, fromDate, toDate, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> GetAssetSize(string sessionId, bool assignableOnly)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_moveApi.GetApi(Priority.UserInitiated).GetAssetSize(sessionId, assignableOnly, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> GetAssetType(string sessionId, bool assignableOnly)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_moveApi.GetApi(Priority.UserInitiated).GetAssetType(sessionId, assignableOnly, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> PostManifest(ManifestModel model, string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_moveApi.GetApi(Priority.UserInitiated).PostManifest(model, sessionId, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> PostNewPartner(NewPartnerRequestModel model, string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_moveApi.GetApi(Priority.UserInitiated).PostNewPartner(model, sessionId, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }


        #endregion

        #region Pallet Api

        public async Task<HttpResponseMessage> PostPallet(PalletRequestModel model, string sessionId)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_palletApi.GetApi(Priority.UserInitiated).PostPallet(model, sessionId, cts.Token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        #endregion
    }
}
