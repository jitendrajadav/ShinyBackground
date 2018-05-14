using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using KegID.SQLiteClient;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KegID
{
    public class TaskCounter
    {
        public readonly IMoveService MoveService;

        public async Task RunCounter(CancellationToken token, string _barcode)
        {
            await Task.Run(async () => {

                await ValidateBarcodeInsertIntoLocalDB(_barcode);

            }, token);
        }

        public async Task ValidateBarcodeInsertIntoLocalDB(string _barcodeId)
        {
            var service = SimpleIoc.Default.GetInstance<IMoveService>();
            ValidateBarcodeModel validateBarcodeModel = await service.GetValidateBarcodeAsync(AppSettings.User.SessionId, _barcodeId);
            Barcode barcode = null;

            if (validateBarcodeModel.Kegs != null)
            {
                barcode = new Barcode
                {
                    Id = _barcodeId,
                    Partners = validateBarcodeModel.Kegs.Partners,
                    Icon = validateBarcodeModel.Kegs.Partners.Count > 1 ? GetIconByPlatform.GetIcon("validationquestion.png") :
                    validateBarcodeModel.Kegs.Partners.Count == 0 ? GetIconByPlatform.GetIcon("validationerror.png") : GetIconByPlatform.GetIcon("validationok.png"),
                    MaintenanceItems = validateBarcodeModel.Kegs.MaintenanceItems
                };

                BarcodeModel barcodeModel = new BarcodeModel()
                {
                    Barcode = _barcodeId,
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

            Device.BeginInvokeOnMainThread(() => {
                MessagingCenter.Send<Barcode>(barcode, "BarcodeMessage");
            }); ;
        }

    }
}
