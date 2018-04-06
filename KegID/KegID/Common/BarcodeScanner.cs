﻿using KegID.Model;
using KegID.Services;
using KegID.SQLiteClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace KegID.Common
{
    public static class BarcodeScanner
    {
        static ZXingScannerPage scanPage;

        public static async Task<List<Barcode>> BarcodeScanAsync(IMoveService _moveService)
        {
            List<Barcode> barcodes = null;
            // Create our custom overlay
            var customOverlay = new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowSpacing = 0
            };

            customOverlay.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
            customOverlay.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            customOverlay.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });

            var torch = new Button
            {
                Text = "Toggle Torch"
            };
            torch.Clicked += delegate
            {
                scanPage.ToggleTorch();
            };
            var title = new Label
            {
                TextColor = Color.White,
                Margin = new Thickness(10, 0, 0, 0),
                VerticalTextAlignment = TextAlignment.End

            };
            var done = new Button
            {
                VerticalOptions = LayoutOptions.End,
                Text = "Done",
                TextColor = Color.Blue
            };
            done.Clicked += async delegate
            {
                //switch ((ViewTypeEnum)Enum.Parse(typeof(ViewTypeEnum), Application.Current.MainPage.Navigation.ModalStack[Application.Current.MainPage.Navigation.ModalStack.Count - 2].GetType().Name))
                //{
                //    case ViewTypeEnum.FillScanView:
                //        SimpleIoc.Default.GetInstance<FillScanViewModel>().BarcodeCollection = BarcodeCollection;
                //        break;
                //    case ViewTypeEnum.MaintainScanView:
                //        SimpleIoc.Default.GetInstance<MaintainScanViewModel>().BarcodeCollection = BarcodeCollection;
                //        break;
                //}
                await Application.Current.MainPage.Navigation.PopModalAsync();
            };

            customOverlay.Children.Add(torch, 0, 0);
            customOverlay.Children.Add(title, 0, 1);
            customOverlay.Children.Add(done, 0, 2);


            scanPage = new ZXingScannerPage(customOverlay: customOverlay);
            scanPage.OnScanResult += (result) =>
            Device.BeginInvokeOnMainThread(async () =>
            {
                var check = barcodes.Any(x => x.Id == result.Text);

                if (!check)
                {
                    title.Text = "Last scan: " + result.Text;
                    barcodes = await ValidateBarcodeInsertIntoLocalDB(result.Text, _moveService);
                }
            });

            await Application.Current.MainPage.Navigation.PushModalAsync(scanPage);
            return barcodes;
        }
        public static async Task<List<Barcode>> ValidateBarcodeInsertIntoLocalDB(string barcodeId, IMoveService _moveService)
        {
            List<Barcode> barcodes = new List<Barcode>();
            ValidateBarcodeModel validateBarcodeModel = await _moveService.GetValidateBarcodeAsync(AppSettings.User.SessionId, barcodeId);

            Barcode barcode = new Barcode
            {
                Id = barcodeId,
                PartnerCount = validateBarcodeModel.Kegs.Partners.Count,
                Icon = validateBarcodeModel.Kegs.Partners.Count > 1 ? GetIconByPlatform.GetIcon("validationerror.png") : GetIconByPlatform.GetIcon("validationquestion.png"),
            };

            BarcodeModel barcodeModel = new BarcodeModel()
            {
                Barcode = barcodeId,
                BarcodeJson = JsonConvert.SerializeObject(validateBarcodeModel)
            };
            try
            {
                // The item does not exists in the database so lets insert it
                await SQLiteServiceClient.Db.InsertAsync(barcodeModel);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            var isNew = barcodes.ToList().Any(x => x.Id == barcode.Id);
            if (!isNew)
                barcodes.Add(barcode);

            return barcodes;
        }

    }
}