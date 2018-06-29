using System.Collections.ObjectModel;
using KegID.Model;
using Xamarin.Forms;
using KegID.Common;
using System;
using KegID.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using KegID.Messages;

namespace KegID.ViewModel
{
    public class AddPalletsViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;
        public IPalletizeService _palletizeService { get; set; }
        public IMoveService _moveService { get; set; }

        #region AddPalletsTitle

        /// <summary>
        /// The <see cref="AddPalletsTitle" /> property's name.
        /// </summary>
        public const string AddPalletsTitlePropertyName = "AddPalletsTitle";

        private string _AddPalletsTitle = default(string);

        /// <summary>
        /// Sets and gets the AddPalletsTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string AddPalletsTitle
        {
            get
            {
                return _AddPalletsTitle;
            }

            set
            {
                if (_AddPalletsTitle == value)
                {
                    return;
                }

                _AddPalletsTitle = value;
                RaisePropertyChanged(AddPalletsTitlePropertyName);
            }
        }

        #endregion

        #region Pallets

        /// <summary>
        /// The <see cref="Pallets" /> property's name.
        /// </summary>
        public const string PalletsPropertyName = "Pallets";

        private string _Pallets = default(string);

        /// <summary>
        /// Sets and gets the Pallets property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Pallets
        {
            get
            {
                return _Pallets;
            }

            set
            {
                if (_Pallets == value)
                {
                    return;
                }

                _Pallets = value;
                RaisePropertyChanged(PalletsPropertyName);
            }
        }

        #endregion

        #region PalletCollection

        /// <summary>
        /// The <see cref="PalletCollection" /> property's name.
        /// </summary>
        public const string PalletCollectionPropertyName = "PalletCollection";

        private ObservableCollection<PalletModel> _PalletCollection = new ObservableCollection<PalletModel>();

        /// <summary>
        /// Sets and gets the PalletCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<PalletModel> PalletCollection
        {
            get
            {
                return _PalletCollection;
            }

            set
            {
                if (_PalletCollection == value)
                {
                    return;
                }

                _PalletCollection = value;
                RaisePropertyChanged(PalletCollectionPropertyName);
            }
        }

        #endregion

        #region Kegs

        /// <summary>
        /// The <see cref="Kegs" /> property's name.
        /// </summary>
        public const string KegsPropertyName = "Kegs";

        private string _Kegs = default(string);

        /// <summary>
        /// Sets and gets the Kegs property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Kegs
        {
            get
            {
                return _Kegs;
            }

            set
            {
                if (_Kegs == value)
                {
                    return;
                }

                _Kegs = value;
                RaisePropertyChanged(KegsPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand SubmitCommand { get; }
        public DelegateCommand FillScanCommand { get; }
        public DelegateCommand FillKegsCommand { get; }
        public DelegateCommand<PalletModel> ItemTappedCommand { get; }

        #endregion

        #region Constructor

        public AddPalletsViewModel(IPalletizeService palletizeService, IMoveService moveService, INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            _palletizeService = palletizeService;
            _moveService = moveService;

            SubmitCommand = new DelegateCommand(SubmitCommandRecieverAsync);
            FillScanCommand = new DelegateCommand(FillScanCommandRecieverAsync);
            FillKegsCommand = new DelegateCommand(FillKegsCommandRecieverAsync);
            ItemTappedCommand = new DelegateCommand<PalletModel>((model) => ItemTappedCommandRecieverAsync(model));

            //HandleReceivedMessages();
        }

        #endregion

        #region Methods

        //private void HandleReceivedMessages()
        //{
        //    MessagingCenter.Subscribe<FillScanToAddPalletPagesMsg>(this, "FillScanToAddPalletPagesMsg", message =>
        //    {
        //        Device.BeginInvokeOnMainThread(async () =>
        //        {
        //            var value = message;
        //            if (value != null)
        //            {
        //              await AssignValueToAddPalletAsync(value.BatchId, value.BarcodeModels);
        //            }
        //        });
        //    });
        //}

        private async void ItemTappedCommandRecieverAsync(PalletModel model)
        {
            try
            {
                //SimpleIoc.Default.GetInstance<FillScanViewModel>().GenerateManifestIdAsync(model);
                //await Application.Current.MainPage.Navigation.PushModalAsync(new FillScanView(), animated: false);
                var param = new NavigationParameters
                    {
                        { "model", model }
                    };
                await _navigationService.NavigateAsync(new Uri("FillScanView", UriKind.Relative), param, useModalNavigation: true, animated: false);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void FillKegsCommandRecieverAsync()
        {
            try
            {
                //await Application.Current.MainPage.Navigation.PopModalAsync();
                await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public async void SubmitCommandRecieverAsync()
        {
            var barcodes = ConstantManager.Barcodes;
            var tags = ConstantManager.Tags;
            var partnerModel = ConstantManager.Partner;
            //var barcodes = SimpleIoc.Default.GetInstance<FillScanViewModel>().BarcodeCollection;
            //var tags = SimpleIoc.Default.GetInstance<FillScanViewModel>().Tags;
            //var partnerModel = SimpleIoc.Default.GetInstance<FillViewModel>().PartnerModel;

            if (barcodes.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Error: Please add some scans.", "Ok");
                return;
            }

            List<string> closedBatches = new List<string>();
            List<NewPallet> newPallets = new List<NewPallet>();
            NewPallet newPallet = null;
            List<TItem> palletItems = new List<TItem>();
            TItem palletItem = null;

            foreach (var pallet in PalletCollection)
            {
                foreach (var item in pallet.Barcode)
                {
                    palletItem = new TItem
                    {
                        Barcode = item.Barcode,
                        //palletItem. = SimpleIoc.Default.GetInstance<FillViewModel>().NewBatchModel.BatchId,
                        //palletItem.Contents = "";
                        //palletItem.HeldOnPalletId = "";
                        //palletItem.KegId = "";
                        //palletItem.PalletId = "";
                        ScanDate = DateTimeOffset.UtcNow.Date,
                        //palletItem.SkuId = "";
                        //ValidationStatus = 4,
                        //Tags = tags
                    };

                    palletItems.Add(palletItem);
                }

                newPallet = new NewPallet
                {
                    Barcode = pallet.ManifestId,
                    //newPallet.BarcodeFormat = "";
                    BuildDate = DateTimeOffset.UtcNow.Date,
                    //newPallet.ManifestTypeId = 4;
                    StockLocation = partnerModel.PartnerId,
                    StockLocationId = partnerModel.PartnerId,
                    StockLocationName = partnerModel.FullName,
                    OwnerId = AppSettings.User.CompanyId,
                    PalletId = Uuid.GetUuId(),
                    //PalletItems = palletItems,
                    ReferenceKey = "",
                    //Tags = tags
                };
                //newPallet.TargetLocation = "";
                foreach (var item in tags)
                    newPallet.Tags.Add(item);
                foreach (var item in palletItems)
                    newPallet.PalletItems.Add(item);
                newPallets.Add(newPallet);
            }

            var alertResult = await Application.Current.MainPage.DisplayAlert("Close batch", "Mark this batch as completed?", "Yes", "No");

            if (alertResult)
                closedBatches = PalletCollection.Select(x => x.ManifestId).ToList();

            Loader.StartLoading();

            var model = await ManifestManager.GetManifestDraft(EventTypeEnum.FILL_MANIFEST, Uuid.GetUuId(),
                    barcodes, tags, partnerModel, newPallets, new List<NewBatch>(), closedBatches, 4);

            if (model != null)
            {
                try
                {
                    var manifestResult = await _moveService.PostManifestAsync(model, AppSettings.User.SessionId, Configuration.NewManifest);

                    if (manifestResult.ManifestId != null)
                    {
                        var manifest = await _moveService.GetManifestAsync(AppSettings.User.SessionId, manifestResult.ManifestId);
                        if (manifest.Response.StatusCode == System.Net.HttpStatusCode.OK.ToString())
                        {
                            //SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().AssignInitialValue(manifest,string.Empty);
                            Loader.StopLoading();
                            //await Application.Current.MainPage.Navigation.PushModalAsync(new ManifestDetailView(), animated: false);
                            var param = new NavigationParameters
                            {
                                { "manifest", manifest }
                            };
                            await _navigationService.NavigateAsync(new Uri("ManifestDetailView", UriKind.Relative), param, useModalNavigation: true, animated: false);
                        }
                        else
                        {
                            //SimpleIoc.Default.GetInstance<LoginViewModel>().InvalideServiceCallAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                     Crashes.TrackError(ex);
                }
                finally
                {
                    Loader.StopLoading();
                    model = null;
                    barcodes = null;
                    tags = null;
                    partnerModel = null;
                    closedBatches = null;
                    newPallets = null;
                    newPallet = null;
                    palletItems = null;
                    palletItem = null;
                    Cleanup();
                }
            }
            else
                await Application.Current.MainPage.DisplayAlert("Alert", "Something goes wrong please check again", "Ok");
        }

        private async void FillScanCommandRecieverAsync()
        {
            try
            {
                //SimpleIoc.Default.GetInstance<FillScanViewModel>().GenerateManifestIdAsync(null);
                //await Application.Current.MainPage.Navigation.PushModalAsync(new FillScanView(), animated: false);
                var param = new NavigationParameters
                    {
                        { "GenerateManifestIdAsync", "GenerateManifestIdAsync" }
                    };
                await _navigationService.NavigateAsync(new Uri("FillScanView", UriKind.Relative), param, useModalNavigation: true, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal async Task AssignValueToAddPalletAsync(string manifestId, IList<BarcodeModel> barcodes)
        {
            try
            {
                if (!PalletCollection.Any(x => x.ManifestId == manifestId))
                {
                    PalletCollection.Add(new PalletModel() { Barcode = barcodes, Count = barcodes.Count(), ManifestId = manifestId });

                    if (PalletCollection.Sum(x => x.Count) > 1)
                        Kegs = string.Format("({0} Kegs)", PalletCollection.Sum(x => x.Count));
                    else
                        Kegs = string.Format("({0} Keg)", PalletCollection.Sum(x => x.Count));
                }
                else
                {
                    PalletCollection.Where(x => x.ManifestId == manifestId).FirstOrDefault().Barcode = barcodes;
                    PalletCollection.Where(x => x.ManifestId == manifestId).FirstOrDefault().Count = barcodes.Count;

                    if (PalletCollection.Sum(x => x.Count) > 1)
                        Kegs = string.Format("({0} Kegs)", PalletCollection.Sum(x => x.Count));
                    else
                        Kegs = string.Format("({0} Keg)", PalletCollection.Sum(x => x.Count));
                }
                await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal void AssignFillScanValue(IList<BarcodeModel> _barcodes, string _manifest)
        {
            try
            {
                PalletCollection.Add(new PalletModel()
                {
                    Barcode = _barcodes,
                    Count = _barcodes.Count(),
                    ManifestId = _manifest
                });

                if (PalletCollection.Sum(x => x.Count) > 1)
                    Kegs = string.Format("({0} Kegs)", PalletCollection.Sum(x => x.Count));
                else
                    Kegs = string.Format("({0} Keg)", PalletCollection.Sum(x => x.Count));
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public void Cleanup()
        {
            try
            {
                //base.Cleanup();
                PalletCollection.Clear();
                Kegs = default(string);
                AddPalletToFillScanMsg msg = new AddPalletToFillScanMsg
                {
                    CleanUp = true
                };
                MessagingCenter.Send(msg, "AddPalletToFillScanMsg");
                //SimpleIoc.Default.GetInstance<FillScanViewModel>().Cleanup();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            //MessagingCenter.Unsubscribe<FillScanToAddPalletPagesMsg>(this, "FillScanToAddPalletPagesMsg");
        }


        public async override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("Barcodes"))
            {
                AssignFillScanValue(parameters.GetValue<IList<BarcodeModel>>("Barcodes"), parameters.GetValue<string>("ManifestId"));
            }
            if (parameters.ContainsKey("AddPalletsTitle"))
            {
                AddPalletsTitle = parameters.GetValue<string>("AddPalletsTitle");
            }
            if (parameters.ContainsKey("SubmitCommandRecieverAsync"))
            {
                SubmitCommandRecieverAsync();
            }
            if (parameters.ContainsKey("AssignValueToAddPalletAsync"))
            {
               await AssignValueToAddPalletAsync(parameters.GetValue<string>("AssignValueToAddPalletAsync"), parameters.GetValue<IList<BarcodeModel>>("Barcodes"));
            }
        }

        #endregion
    }

}
