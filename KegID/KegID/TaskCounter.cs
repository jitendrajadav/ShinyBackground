using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.LocalDb;
using KegID.Messages;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Realms;
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
                ValidateBarcodeModel validateBarcodeModel = await service.GetValidateBarcodeAsync(AppSettings.User.SessionId, item);
                validateBarcodeModel.Barcode = item;
                //Barcode barcode = null;
                if (validateBarcodeModel.Kegs != null)
                {
                    //barcode = new Barcode
                    //{
                    //    Id = item,
                    //    Icon = validateBarcodeModel.Kegs.Partners.Count > 1 ? GetIconByPlatform.GetIcon("validationquestion.png") :
                    //    validateBarcodeModel.Kegs.Partners.Count == 0 ? GetIconByPlatform.GetIcon("validationerror.png") : GetIconByPlatform.GetIcon("validationok.png"),
                    //};
                    //foreach (var partner in validateBarcodeModel.Kegs.Partners)
                    //{
                    //    barcode.Partners.Add(partner);
                    //}
                    //foreach (var maintenance in validateBarcodeModel.Kegs.MaintenanceItems)
                    //{
                    //    barcode.MaintenanceItems.Add(maintenance);
                    //}
                    //BarcodeModel barcodeModel = new BarcodeModel()
                    //{
                    //    Barcode = item,
                    //    BarcodeJson = JsonConvert.SerializeObject(validateBarcodeModel)
                    //};
                    try
                    {
                        // The item does not exists in the database so lets insert it
                        //var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                        //RealmDb.Write(() =>
                        //{
                        //    //RealmDb.Add(barcodeModel,true);
                        //    RealmDb.Add(validateBarcodeModel,true);
                        //});
                        //await SQLiteServiceClient.Db.InsertAsync(barcodeModel);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }

                Device.BeginInvokeOnMainThread(() => {
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
