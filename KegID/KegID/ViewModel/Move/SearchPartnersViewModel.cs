using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class SearchPartnersViewModel : ViewModelBase
    {
        #region Properties
        
        #region BackPartners

        /// <summary>
        /// The <see cref="BackPartners" /> property's name.
        /// </summary>
        public const string BackPartnersPropertyName = "BackPartners";

        private string _BackPartners = default(string);

        /// <summary>
        /// Sets and gets the BackPartners property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string BackPartners
        {
            get
            {
                return _BackPartners;
            }

            set
            {
                if (_BackPartners == value)
                {
                    return;
                }

                _BackPartners = value;
                RaisePropertyChanged(BackPartnersPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public RelayCommand BackPartnersCommand { get; set; }

        #endregion

        #region Contructor

        public SearchPartnersViewModel()
        {
            BackPartners = "< Partners";
            BackPartnersCommand = new RelayCommand(BackPartnersCommandRecieverAsync);
        }

        #endregion

        #region Methods
        private async void BackPartnersCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PopModalAsync();

        #endregion
    }
}
