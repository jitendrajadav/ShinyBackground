using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Model;
using KegID.Services;
using KegID.View;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class PalletizeViewModel : ViewModelBase
    {
        #region Properties
        public IPalletizeService _palletizeService { get; set; }

        public bool TargetLocationPartner { get; set; }

        #region SelectLocationTitle

        /// <summary>
        /// The <see cref="SelectLocationTitle" /> property's name.
        /// </summary>
        public const string SelectLocationTitlePropertyName = "SelectLocationTitle";

        private string _SelectLocationTitle = "select location";

        /// <summary>
        /// Sets and gets the SelectLocationTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string SelectLocationTitle
        {
            get
            {
                return _SelectLocationTitle;
            }

            set
            {
                if (_SelectLocationTitle == value)
                {
                    return;
                }

                _SelectLocationTitle = value;
                RaisePropertyChanged(SelectLocationTitlePropertyName);
            }
        }

        #endregion

        #region TargetLocationTitle

        /// <summary>
        /// The <see cref="TargetLocationTitle" /> property's name.
        /// </summary>
        public const string TargetLocationTitlePropertyName = "TargetLocationTitle";

        private string _TargetLocationTitle = "none";

        /// <summary>
        /// Sets and gets the TargetLocationTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string TargetLocationTitle
        {
            get
            {
                return _TargetLocationTitle;
            }

            set
            {
                if (_TargetLocationTitle == value)
                {
                    return;
                }

                _TargetLocationTitle = value;
                RaisePropertyChanged(TargetLocationTitlePropertyName);
            }
        }

        #endregion

        #region AddInfoTitle

        /// <summary>
        /// The <see cref="AddInfoTitle" /> property's name.
        /// </summary>
        public const string AddInfoTitlePropertyName = "AddInfoTitle";

        private string _AddInfoTitle = "Add info";

        /// <summary>
        /// Sets and gets the AddInfoTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string AddInfoTitle
        {
            get
            {
                return _AddInfoTitle;
            }

            set
            {
                if (_AddInfoTitle == value)
                {
                    return;
                }

                _AddInfoTitle = value;
                RaisePropertyChanged(AddInfoTitlePropertyName);
            }
        }

        #endregion

        #region IsCameraVisible

        /// <summary>
        /// The <see cref="IsCameraVisible" /> property's name.
        /// </summary>
        public const string IsCameraVisiblePropertyName = "IsCameraVisible";

        private bool _IsCameraVisible = false;

        /// <summary>
        /// Sets and gets the IsCameraVisible property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsCameraVisible
        {
            get
            {
                return _IsCameraVisible;
            }

            set
            {
                if (_IsCameraVisible == value)
                {
                    return;
                }

                _IsCameraVisible = value;
                RaisePropertyChanged(IsCameraVisiblePropertyName);
            }
        }

        #endregion

        #region Pallet

        /// <summary>
        /// The <see cref="Pallet" /> property's name.
        /// </summary>
        public const string PalletPropertyName = "Pallet";

        private string _Pallet = default(string);

        /// <summary>
        /// Sets and gets the Pallet property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Pallet
        {
            get
            {
                return _Pallet;
            }

            set
            {
                if (_Pallet == value)
                {
                    return;
                }

                _Pallet = value;
                RaisePropertyChanged(PalletPropertyName);
            }
        }

        #endregion

        #region AddKegs

        /// <summary>
        /// The <see cref="AddKegs" /> property's name.
        /// </summary>
        public const string AddKegsPropertyName = "AddKegs";

        private string _AddKegs = "Add Kegs";

        /// <summary>
        /// Sets and gets the AddKegs property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string AddKegs
        {
            get
            {
                return _AddKegs;
            }

            set
            {
                if (_AddKegs == value)
                {
                    return;
                }

                _AddKegs = value;
                RaisePropertyChanged(AddKegsPropertyName);
            }
        }

        #endregion

        #region IsSubmitVisible

        /// <summary>
        /// The <see cref="IsSubmitVisible" /> property's name.
        /// </summary>
        public const string IsSubmitVisiblePropertyName = "IsSubmitVisible";

        private bool _IsSubmitVisible = false;

        /// <summary>
        /// Sets and gets the IsSubmitVisible property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsSubmitVisible
        {
            get
            {
                return _IsSubmitVisible;
            }

            set
            {
                if (_IsSubmitVisible == value)
                {
                    return;
                }

                _IsSubmitVisible = value;
                RaisePropertyChanged(IsSubmitVisiblePropertyName);
            }
        }

        #endregion

        #region Tags
        /// <summary>
        /// The <see cref="Tags" /> property's name.
        /// </summary>
        public const string TagsPropertyName = "Tags";

        private List<Tag> _tags = new List<Tag>();

        /// <summary>
        /// Sets and gets the Tags property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public List<Tag> Tags
        {
            get
            {
                return _tags;
            }

            set
            {
                if (_tags == value)
                {
                    return;
                }

                _tags = value;
                RaisePropertyChanged(TagsPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand PartnerCommand { get; set; }
        public RelayCommand AddTagsCommand { get; set; }
        public RelayCommand TargetLocationPartnerCommand { get; set; }
        public RelayCommand AddKegsCommand { get; set; }
        public RelayCommand IsPalletVisibleCommand { get; set; }
        public RelayCommand BarcodeScanCommand { get; set; }
        public RelayCommand SubmitCommand { get; set; }
        #endregion

        #region Constructor
        public PalletizeViewModel(IPalletizeService palletizeService)
        {
            _palletizeService = palletizeService;
            CancelCommand = new RelayCommand(CancelCommandRecieverAsync);
            PartnerCommand = new RelayCommand(PartnerCommandRecieverAsync);
            AddTagsCommand = new RelayCommand(AddTagsCommandRecieverAsync);
            TargetLocationPartnerCommand = new RelayCommand(TargetLocationPartnerCommandRecieverAsync);
            AddKegsCommand = new RelayCommand(AddKegsCommandRecieverAsync);
            IsPalletVisibleCommand = new RelayCommand(IsPalletVisibleCommandReciever);
            BarcodeScanCommand = new RelayCommand(BarcodeScanCommandReciever);
            SubmitCommand = new RelayCommand(SubmitCommandRecieverAsync);
            Pallet = "Pallet #:-10000008500359874";
        }

        #endregion

        #region Methods
        private async void SubmitCommandRecieverAsync()
        {
            PalletItem palletItem = new PalletItem();
            List<PalletItem> palletItemList = new List<PalletItem>();
            PalletRequestModel palletRequestModel = new PalletRequestModel();
            palletRequestModel.Barcode = "";
            palletRequestModel.BarcodeFormat = "";
            palletRequestModel.BuildDate = DateTime.Today;
            palletRequestModel.ManifestTypeId = 000;
            palletRequestModel.OwnerId = "";
            palletRequestModel.PalletId = "";
            palletRequestModel.PalletItems = palletItemList;
            palletRequestModel.ReferenceKey = "";
            palletRequestModel.StockLocation = "";
            palletRequestModel.Tags = Tags;
            palletRequestModel.TargetLocation = "";

           await _palletizeService.PostPalletAsync(palletRequestModel,Configuration.SessionId,Configuration.NewPallet);
        }

        private void BarcodeScanCommandReciever()
        {
            SimpleIoc.Default.GetInstance<ScanKegsViewModel>().BarcodeScanCommandReciever();
        }

        private void IsPalletVisibleCommandReciever()
        {
            IsCameraVisible = true;
        }

        private async void AddKegsCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new ScanKegsView());
        }

        private async void AddTagsCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new AddTagsView());
        }

        private async void PartnerCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new PartnersView());
        }
        private async void TargetLocationPartnerCommandRecieverAsync()
        {
            TargetLocationPartner = true;
            await Application.Current.MainPage.Navigation.PushModalAsync(new PartnersView());
        }

        private async void CancelCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PopModalAsync();

        #endregion
    }
}
