using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.View;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class PalletizeViewModel : ViewModelBase
    {
        #region Properties

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

        #endregion

        #region Commands
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand PartnerCommand { get; set; }
        public RelayCommand AddTagsCommand { get; set; }
        public RelayCommand TargetLocationPartnerCommand { get; set; }
        public RelayCommand AddKegsCommand { get; set; }
        public RelayCommand IsPalletVisibleCommand { get; set; }
        public RelayCommand BarcodeScanCommand { get; set; }
        #endregion

        #region Constructor
        public PalletizeViewModel()
        {
            CancelCommand = new RelayCommand(CancelCommandRecieverAsync);
            PartnerCommand = new RelayCommand(PartnerCommandRecieverAsync);
            AddTagsCommand = new RelayCommand(AddTagsCommandRecieverAsync);
            TargetLocationPartnerCommand = new RelayCommand(TargetLocationPartnerCommandRecieverAsync);
            AddKegsCommand = new RelayCommand(AddKegsCommandRecieverAsync);
            IsPalletVisibleCommand = new RelayCommand(IsPalletVisibleCommandReciever);
            BarcodeScanCommand = new RelayCommand(BarcodeScanCommandReciever);
            Pallet = "Pallet #:-10000008500359874";
        }

        private void BarcodeScanCommandReciever()
        {
            SimpleIoc.Default.GetInstance<ScanKegsViewModel>().BarcodeScanCommandReciever();
        }

        private void IsPalletVisibleCommandReciever()
        {
            IsCameraVisible = true;
        }

        #endregion

        #region Methods

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
