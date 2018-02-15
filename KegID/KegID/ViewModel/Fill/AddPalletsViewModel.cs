using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using KegID.Model;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;
using KegID.View;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;

namespace KegID.ViewModel
{
    public class AddPalletsViewModel : ViewModelBase
    {
        #region Properties

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

        public AddPalletsViewModel()
        {
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
            var manifest = await ManifestManager.GetManifestDraft(EventTypeEnum.FILL_MANIFEST, SimpleIoc.Default.GetInstance<FillScanViewModel>().ManifestId, SimpleIoc.Default.GetInstance<FillScanViewModel>().BarcodeCollection, SimpleIoc.Default.GetInstance<FillScanViewModel>().Tags, SimpleIoc.Default.GetInstance<FillViewModel>().PartnerModel);

            var result = await Application.Current.MainPage.DisplayAlert("Close batch", "Mark this batch as completed?", "Yes","No");
            if (result)
            {

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
