using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Messages;
using KegID.Model;
using KegID.Services;
using KegID.SQLiteClient;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KegID
{
    public class TaskCounter
    {
        public readonly IMoveService MoveService;

        public async Task RunCounter(CancellationToken token, IList<string> _barcode,ViewTypeEnum _page)
        {
            await Task.Run(async () => {

                await ValidateBarcodeInsertIntoLocalDB(_barcode, _page);

            }, token);
        }

        public async Task ValidateBarcodeInsertIntoLocalDB(IList<string> _barcodeId, ViewTypeEnum _page)
        {
            var service = SimpleIoc.Default.GetInstance<IMoveService>();
            //IList<Barcode> barcodes = new List<Barcode>();

            foreach (var item in _barcodeId)
            {
                ValidateBarcodeModel validateBarcodeModel = await service.GetValidateBarcodeAsync(AppSettings.User.SessionId, item);
                Barcode barcode = null;
                if (validateBarcodeModel.Kegs != null)
                {
                    barcode = new Barcode
                    {
                        Id = item,
                        Partners = validateBarcodeModel.Kegs.Partners,
                        Icon = validateBarcodeModel.Kegs.Partners.Count > 1 ? GetIconByPlatform.GetIcon("validationquestion.png") :
                        validateBarcodeModel.Kegs.Partners.Count == 0 ? GetIconByPlatform.GetIcon("validationerror.png") : GetIconByPlatform.GetIcon("validationok.png"),
                        MaintenanceItems = validateBarcodeModel.Kegs.MaintenanceItems
                    };

                    BarcodeModel barcodeModel = new BarcodeModel()
                    {
                        Barcode = item,
                        BarcodeJson = JsonConvert.SerializeObject(validateBarcodeModel)
                    };
                    try
                    {
                        // The item does not exists in the database so lets insert it
                        await SQLiteServiceClient.Db.InsertAsync(barcodeModel);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }
                //barcodes.Add(barcode);
                Device.BeginInvokeOnMainThread(() => {
                    switch (_page)
                    {
                        case ViewTypeEnum.ScanKegsView:
                            ScanKegsMessage scanKegsMessage = new ScanKegsMessage
                            {
                                Barcodes = barcode
                            };
                            MessagingCenter.Send(scanKegsMessage, "ScanKegsMessage");
                            break;
                        case ViewTypeEnum.FillScanView:
                            MessagingCenter.Send(new FillScanMessage
                            {
                                Barcodes = barcode
                            }, "FillScanMessage");
                            break;
                        case ViewTypeEnum.MaintainScanView:
                            MaintainScanMessage maintainScanMessage = new MaintainScanMessage
                            {
                                Barcodes = barcode
                            };
                            MessagingCenter.Send(maintainScanMessage, "MaintainScanMessage");
                            break;
                        case ViewTypeEnum.BulkUpdateScanView:
                            BulkUpdateScanMessage bulkUpdateScanMessage = new BulkUpdateScanMessage
                            {
                                Barcodes = barcode
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
