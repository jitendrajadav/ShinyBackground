using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Messages;
using KegID.Model;
using KegID.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KegID
{
    public class TaskCounter
    {
        public readonly IMoveService MoveService;

        public async Task RunCounter(CancellationToken token, IList<string> _barcode,string _page)
        {
            await Task.Run(async () => {

                await ValidateBarcodeInsertIntoLocalDB(_barcode, _page);

            }, token);
        }

        public async Task ValidateBarcodeInsertIntoLocalDB(IList<string> _barcodeId, string _page)
        {
            var service = SimpleIoc.Default.GetInstance<IMoveService>();

            foreach (var item in _barcodeId)
            {
                BarcodeModel validateBarcodeModel = await service.GetValidateBarcodeAsync(AppSettings.User.SessionId, item);
                validateBarcodeModel.Barcode = item;
                if (validateBarcodeModel.Kegs != null)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        switch (_page)
                        {
                            case "ScanKegsView":
                                ScanKegsMessage scanKegsMessage = new ScanKegsMessage
                                {
                                    Barcodes = validateBarcodeModel
                                };
                                MessagingCenter.Send(scanKegsMessage, "ScanKegsMessage");
                                break;
                            case "FillScanView":
                                MessagingCenter.Send(new FillScanMessage
                                {
                                    Barcodes = validateBarcodeModel
                                }, "FillScanMessage");
                                break;
                            case "MaintainScanView":
                                MaintainScanMessage maintainScanMessage = new MaintainScanMessage
                                {
                                    Barcodes = validateBarcodeModel
                                };
                                MessagingCenter.Send(maintainScanMessage, "MaintainScanMessage");
                                break;
                            case "BulkUpdateScanView":
                                BulkUpdateScanMessage bulkUpdateScanMessage = new BulkUpdateScanMessage
                                {
                                    Barcodes = validateBarcodeModel
                                };
                                MessagingCenter.Send(bulkUpdateScanMessage, "BulkUpdateScanMessage");
                                break;
                            default:
                                break;
                        }
                    });
                }
            }
        }
    }
}
