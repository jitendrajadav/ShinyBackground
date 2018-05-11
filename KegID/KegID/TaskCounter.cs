using KegID.Common;
using KegID.Model;
using KegID.Services;
using KegID.SQLiteClient;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KegID
{
    public class TaskCounter
    {
        public IMoveService MoveService { get; set; }

        //public TaskCounter(IMoveService _moveService)
        //{
        //    MoveService = _moveService;
        //}

        public async Task RunCounter(CancellationToken token)
        {
            await Task.Run(async () => {

                //for (long i = 0; i < long.MaxValue; i++)
                //{
                //    token.ThrowIfCancellationRequested();

                //    await Task.Delay(250);
                //    var message = new TickedMessage
                //    {
                //        Message = i.ToString()
                //    };

                //    Device.BeginInvokeOnMainThread(() => {
                //        MessagingCenter.Send<TickedMessage>(message, "TickedMessage");
                //    });
                //}


            }, token);
        }

        public async Task<Barcode> ValidateBarcodeInsertIntoLocalDB(string _barcodeId, List<Tag> _tags, string _tagsStr)
        {
            ValidateBarcodeModel validateBarcodeModel = await MoveService.GetValidateBarcodeAsync(AppSettings.User.SessionId, _barcodeId);
            Barcode barcode = null;

            if (validateBarcodeModel.Kegs != null)
            {
                barcode = new Barcode
                {
                    Id = _barcodeId,
                    Tags = _tags,
                    TagsStr = _tagsStr,
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

            return barcode;
        }

    }
}
