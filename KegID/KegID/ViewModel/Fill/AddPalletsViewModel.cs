using System.Collections.ObjectModel;
using KegID.Model;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;
using KegID.Views;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using System;
using KegID.Services;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace KegID.ViewModel
{
    public class AddPalletsViewModel : BaseViewModel
    {
        #region Properties
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

        public RelayCommand SubmitCommand { get; }
        public RelayCommand FillScanCommand { get; }
        public RelayCommand FillKegsCommand { get; }
        public RelayCommand<PalletModel> ItemTappedCommand { get; }

        #endregion

        #region Constructor

        public AddPalletsViewModel(IPalletizeService palletizeService, IMoveService moveService)
        {
            _palletizeService = palletizeService;
            _moveService = moveService;

            SubmitCommand = new RelayCommand(SubmitCommandRecieverAsync);
            FillScanCommand = new RelayCommand(FillScanCommandRecieverAsync);
            FillKegsCommand = new RelayCommand(FillKegsCommandRecieverAsync);
            ItemTappedCommand = new RelayCommand<PalletModel>((model) => ItemTappedCommandRecieverAsync(model));
        }

        private async void ItemTappedCommandRecieverAsync(PalletModel model)
        {
            SimpleIoc.Default.GetInstance<FillScanViewModel>().GenerateManifestIdAsync(model);
            await Application.Current.MainPage.Navigation.PushModalAsync(new FillScanView());
        }

        #endregion

        #region Methods

        private async void FillKegsCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void SubmitCommandRecieverAsync()
        {
            
            var barCodeCollection = SimpleIoc.Default.GetInstance<FillScanViewModel>().BarcodeCollection;

            if (barCodeCollection.Count == 0)
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
                    palletItem = new TItem();
                    palletItem.Barcode = item.Id;
                    //palletItem.BatchId = SimpleIoc.Default.GetInstance<FillViewModel>().NewBatchModel.BatchId;
                    //palletItem.Contents = "";
                    //palletItem.HeldOnPalletId = "";
                    //palletItem.KegId = "";
                    //palletItem.PalletId = "";
                    palletItem.ScanDate = DateTime.Today;
                    //palletItem.SkuId = "";
                    palletItem.ValidationStatus = 4;
                    palletItem.Tags = SimpleIoc.Default.GetInstance<FillScanViewModel>().Tags;

                    palletItems.Add(palletItem);
                }

                newPallet = new NewPallet();
                newPallet.Barcode = pallet.ManifestId;
                //newPallet.BarcodeFormat = "";
                newPallet.BuildDate = DateTime.Today;
                //newPallet.ManifestTypeId = 4;
                newPallet.StockLocation = SimpleIoc.Default.GetInstance<FillViewModel>().PartnerModel.PartnerId;
                newPallet.StockLocationId = SimpleIoc.Default.GetInstance<FillViewModel>().PartnerModel.PartnerId;
                newPallet.StockLocationName = SimpleIoc.Default.GetInstance<FillViewModel>().PartnerModel.FullName;
                newPallet.OwnerId = AppSettings.User.CompanyId;
                newPallet.PalletId = Uuid.GetUuId();
                newPallet.PalletItems = palletItems;
                newPallet.ReferenceKey = "";
                newPallet.Tags = SimpleIoc.Default.GetInstance<FillScanViewModel>().Tags;
                //newPallet.TargetLocation = "";

                newPallets.Add(newPallet);
            }

            var alertResult = await Application.Current.MainPage.DisplayAlert("Close batch", "Mark this batch as completed?", "Yes", "No");

            if (alertResult)
                closedBatches = PalletCollection.Select(x => x.ManifestId).ToList();

            Loader.StartLoading();

            ManifestModel manifestModel = await ManifestManager.GetManifestDraft(EventTypeEnum.FILL_MANIFEST, Uuid.GetUuId(),
                    SimpleIoc.Default.GetInstance<FillScanViewModel>().BarcodeCollection, SimpleIoc.Default.GetInstance<FillScanViewModel>().Tags,
                    SimpleIoc.Default.GetInstance<FillViewModel>().PartnerModel, newPallets,new List<NewBatch>(), closedBatches, 4);

            if (manifestModel != null)
            {
                try
                {
                    var manifestResult = await _moveService.PostManifestAsync(manifestModel, AppSettings.User.SessionId, Configuration.NewManifest);

                    if (manifestResult != null)
                    {
                        var manifest = await _moveService.GetManifestAsync(AppSettings.User.SessionId, manifestResult.ManifestId);
                        if (manifest.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().TrackingNumber = manifest.TrackingNumber;

                            SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().ManifestTo = manifest.CreatorCompany.FullName + "\n" + manifest.CreatorCompany.PartnerTypeName;

                            SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().ShippingDate = Convert.ToDateTime(manifest.ShipDate);
                            SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().ItemCount = manifest.ManifestItems.Count;
                            SimpleIoc.Default.GetInstance<ContentTagsViewModel>().ContentCollection = manifest.ManifestItems.Select(x => x.Barcode).ToList();

                            SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().Contents = !string.IsNullOrEmpty(manifest.ManifestItems.FirstOrDefault().Contents) ? manifest.ManifestItems.FirstOrDefault().Contents : "No contens";

                            Loader.StopLoading();
                            await Application.Current.MainPage.Navigation.PushModalAsync(new ManifestDetailView());
                        }
                        else
                        {
                            SimpleIoc.Default.GetInstance<LoginViewModel>().InvalideServiceCallAsync();
                        } 
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                finally
                {
                    Loader.StopLoading();
                }
            }
            else
                await Application.Current.MainPage.DisplayAlert("Alert", "Something goes wrong please check again", "Ok");
        }

        private async void FillScanCommandRecieverAsync()
        {
            SimpleIoc.Default.GetInstance<FillScanViewModel>().GenerateManifestIdAsync(null);
            await Application.Current.MainPage.Navigation.PushModalAsync(new FillScanView());
        }

        #endregion
    }

}
