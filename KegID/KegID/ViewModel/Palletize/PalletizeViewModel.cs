using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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

        #endregion

        #region Commands
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand PartnerCommand { get; set; }
        public RelayCommand AddTagsCommand { get; set; }
        public RelayCommand TargetLocationPartnerCommand { get; set; }
        public RelayCommand AddKegsCommand { get; set; }

        #endregion

        #region Constructor
        public PalletizeViewModel()
        {
            CancelCommand = new RelayCommand(CancelCommandRecieverAsync);
            PartnerCommand = new RelayCommand(PartnerCommandRecieverAsync);
            AddTagsCommand = new RelayCommand(AddTagsCommandRecieverAsync);
            TargetLocationPartnerCommand = new RelayCommand(TargetLocationPartnerCommandRecieverAsync);
            AddKegsCommand = new RelayCommand(AddKegsCommandRecieverAsync);
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
