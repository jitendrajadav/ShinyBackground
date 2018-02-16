using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using KegID.Model;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;
using KegID.View;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using System;
using KegID.Services;
using System.Collections.Generic;
using System.Linq;

namespace KegID.ViewModel
{
    public class AddPalletsViewModel : ViewModelBase
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

        public RelayCommand SubmitCommand { get; set; }
        public RelayCommand FillScanCommand { get; set; }
        public RelayCommand FillKegsCommand { get; set; }
        public RelayCommand<PalletModel> ItemTappedCommand { get; set; }

        #endregion

        #region Constructor

        public AddPalletsViewModel(IPalletizeService palletizeService, IMoveService moveService)
        {
            _palletizeService =  palletizeService;
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

            if (barCodeCollection.Count==0)
            {
                 await Application.Current.MainPage.DisplayAlert("Error", "Error: Please add some scans.", "Ok");
                return;
            }


            //var manifest = await ManifestManager.GetManifestDraft(EventTypeEnum.FILL_MANIFEST, SimpleIoc.Default.GetInstance<FillScanViewModel>().ManifestId, SimpleIoc.Default.GetInstance<FillScanViewModel>().BarcodeCollection, SimpleIoc.Default.GetInstance<FillScanViewModel>().Tags, SimpleIoc.Default.GetInstance<FillViewModel>().PartnerModel);
            List<PalletItem> palletItems = new List<PalletItem>();
            PalletItem pallet = null;


            foreach (var item in barCodeCollection)
            {
                pallet = new PalletItem();

                pallet.Barcode = item.Id;
                //pallet.Contents = item.Contents;
                //pallet.DateScanned = item.DateScanned;
                //pallet.IsActive = item.IsActive;
                //pallet.Keg = new PalletKeg();
                //pallet.PalletId = Uuid.GetUuId();
                //pallet.RemovedManifest = item.RemovedManifest;
                pallet.ScanDate = DateTime.Now;
                pallet.Tags = SimpleIoc.Default.GetInstance<FillScanViewModel>().Tags;
                pallet.ValidationStatus = 4;

                palletItems.Add(pallet);
            }

            PalletRequestModel palletRequestModel = new PalletRequestModel();
            palletRequestModel.Barcode = SimpleIoc.Default.GetInstance<FillScanViewModel>().ManifestId.Split('-').LastOrDefault();
            palletRequestModel.BuildDate = DateTime.Now;
            palletRequestModel.OwnerId = Configuration.CompanyId;
            palletRequestModel.PalletId = Uuid.GetUuId();
            palletRequestModel.PalletItems = palletItems;
            palletRequestModel.ReferenceKey = "";
            palletRequestModel.StockLocation = SimpleIoc.Default.GetInstance<FillViewModel>().PartnerModel.PartnerId;
            palletRequestModel.StockLocationId = SimpleIoc.Default.GetInstance<FillViewModel>().PartnerModel.PartnerId;
            palletRequestModel.StockLocationName = SimpleIoc.Default.GetInstance<FillViewModel>().PartnerModel.FullName;
            palletRequestModel.Tags = SimpleIoc.Default.GetInstance<FillScanViewModel>().Tags;
            var value = await _palletizeService.PostPalletAsync(palletRequestModel, Configuration.SessionId, Configuration.NewPallet);

            var result = await Application.Current.MainPage.DisplayAlert("Close batch", "Mark this batch as completed?", "Yes","No");
            if (result)
            {
                var manifest = await _moveService.GetManifestAsync(Configuration.SessionId, value.Barcode);
                if (manifest.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().TrackingNumber = manifest.TrackingNumber;

                    SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().ManifestTo = manifest.CreatorCompany.FullName + "\n" + manifest.CreatorCompany.PartnerTypeName;

                    SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().ShippingDate = manifest.ShipDate;
                    SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().ItemCount = manifest.ManifestItems.Count;
                    SimpleIoc.Default.GetInstance<ContentTagsViewModel>().ContentCollection = manifest.ManifestItems;

                    SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().Contents = !string.IsNullOrEmpty(manifest.ManifestItems.FirstOrDefault().Contents) ? manifest.ManifestItems.FirstOrDefault().Contents : "No contens";

                    Loader.StopLoading();
                    await Application.Current.MainPage.Navigation.PushModalAsync(new ManifestDetailView());
                }
                else
                {
                    Loader.StopLoading();
                    SimpleIoc.Default.GetInstance<LoginViewModel>().InvalideServiceCallAsync();
                }
            }
        }
        private async void FillScanCommandRecieverAsync()
        {
            SimpleIoc.Default.GetInstance<FillScanViewModel>().GenerateManifestIdAsync(null);
            await Application.Current.MainPage.Navigation.PushModalAsync(new FillScanView());
        }

        #endregion
    }

}
