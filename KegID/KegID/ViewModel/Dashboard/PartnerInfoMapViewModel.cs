using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KegID.Common;
using KegID.Model;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class PartnerInfoMapViewModel : ViewModelBase
    {
        #region Properties

        #region CustomPins

        /// <summary>
        /// The <see cref="CustomPin" /> property's name.
        /// </summary>
        public const string CustomPinPropertyName = "CustomPin";

        private ObservableCollection<CustomPin> _CustomPin = null;

        /// <summary>
        /// Sets and gets the CustomPin property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<CustomPin> CustomPin
        {
            get
            {
                return _CustomPin;
            }

            set
            {
                if (_CustomPin == value)
                {
                    return;
                }

                _CustomPin = value;
                RaisePropertyChanged(CustomPinPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands
        public RelayCommand PartnerInfoCommand { get; }
        #endregion

        #region Constructor
        public PartnerInfoMapViewModel()
        {
            PartnerInfoCommand = new RelayCommand(PartnerInfoCommandRecieverAsync);
            CustomPin = new ObservableCollection<CustomPin>();
            CustomPin.Add(new CustomPin { Address = "Powai Mumbai streat road",  Label = "Powai Mumbai",  Position = new Xamarin.Forms.Maps.Position((long)Geolocation.savedPosition.Latitude, (long)Geolocation.savedPosition.Longitude), Type = SuggestionType.Event });
            CustomPin.Add(new CustomPin { Address = "Malad S V road", Label = "Malad Mumbai", Position = new Xamarin.Forms.Maps.Position((long)Geolocation.savedPosition.Latitude-2, (long)Geolocation.savedPosition.Longitude-2), Type = SuggestionType.Event });
            CustomPin.Add(new CustomPin { Address = "Borivali Link road", Label = " Borivali Mumbai", Position = new Xamarin.Forms.Maps.Position((long)Geolocation.savedPosition.Latitude+2, (long)Geolocation.savedPosition.Longitude+2), Type = SuggestionType.Event });
            CustomPin.Add(new CustomPin { Address = "Nariman Point", Label = "Nariman Point", Position = new Xamarin.Forms.Maps.Position((long)Geolocation.savedPosition.Latitude-3, (long)Geolocation.savedPosition.Longitude-3), Type = SuggestionType.Event });
            CustomPin.Add(new CustomPin { Address = "Powai Mumbai streat road", Label = "Powai Mumbai", Position = new Xamarin.Forms.Maps.Position((long)Geolocation.savedPosition.Latitude+1, (long)Geolocation.savedPosition.Longitude+1), Type = SuggestionType.Event });
            CustomPin.Add(new CustomPin { Address = "Powai Mumbai streat road", Label = "Powai Mumbai", Position = new Xamarin.Forms.Maps.Position((long)Geolocation.savedPosition.Latitude+5, (long)Geolocation.savedPosition.Longitude+5), Type = SuggestionType.Event });
        }

        #endregion

        #region Methods
        private async void PartnerInfoCommandRecieverAsync()
        {
          await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        #endregion
    }
}
