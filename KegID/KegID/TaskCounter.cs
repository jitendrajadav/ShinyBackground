using KegID.Common;
using KegID.Messages;
using KegID.Model;
using KegID.Services;
using KegID.ViewModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KegID
{
    public class TaskCounter : BaseViewModel
    {
        public TaskCounter() : base(null)
        {

        }

        public async Task RunCounter(CancellationToken token, IList<string> _barcode, string _page)
        {
            await Task.Run(async () =>
            {
                await ValidateBarcodeInsertIntoLocalDB(_barcode, _page);
            }, token);
        }

        public async Task ValidateBarcodeInsertIntoLocalDB(IList<string> _barcodeId, string _page)
        {
            //IMoveService _moveService = new MoveService();

            foreach (var item in _barcodeId)
            {
                var response = await ApiManager.GetValidateBarcode(item, AppSettings.SessionId);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = await Task.Run(() => JsonConvert.DeserializeObject<BarcodeModel>(json, GetJsonSetting()));

                    data.Barcode = item;
                    if (data.Kegs != null)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            switch (_page)
                            {
                                case "ScanKegsView":
                                    ScanKegsMessage scanKegsMessage = new ScanKegsMessage
                                    {
                                        Barcodes = data
                                    };
                                    MessagingCenter.Send(scanKegsMessage, "ScanKegsMessage");
                                    break;
                                case "FillScanView":
                                    MessagingCenter.Send(new FillScanMessage
                                    {
                                        Barcodes = data
                                    }, "FillScanMessage");
                                    break;
                                case "MaintainScanView":
                                    MaintainScanMessage maintainScanMessage = new MaintainScanMessage
                                    {
                                        Barcodes = data
                                    };
                                    MessagingCenter.Send(maintainScanMessage, "MaintainScanMessage");
                                    break;
                                case "BulkUpdateScanView":
                                    BulkUpdateScanMessage bulkUpdateScanMessage = new BulkUpdateScanMessage
                                    {
                                        Barcodes = data
                                    };
                                    MessagingCenter.Send(bulkUpdateScanMessage, "BulkUpdateScanMessage");
                                    break;
                            }
                        });
                    }
                }
            }
        }
    }
}
