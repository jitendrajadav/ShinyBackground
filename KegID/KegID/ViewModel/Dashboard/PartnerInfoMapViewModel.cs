using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KegID.Common;
using KegID.Model;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class PartnerInfoMapViewModel : BaseViewModel
    {
        #region Properties

        #region CustomPins

        /// <summary>
        /// The <see cref="CustomPins" /> property's name.
        /// </summary>
        public const string CustomPinsPropertyName = "CustomPins";

        private ObservableCollection<CustomPin> _customPins = new ObservableCollection<CustomPin>();

        /// <summary>
        /// Sets and gets the CustomPins property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<CustomPin> CustomPins
        {
            get
            {
                return _customPins;
            }

            set
            {
                if (_customPins == value)
                {
                    return;
                }

                _customPins = value;
                RaisePropertyChanged(CustomPinsPropertyName);
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

            var Suggestions = new List<Suggestion>
            {
                new Suggestion { Name = "The Salty Chicken", Description = "Loren ipsum dolor sit amet, consectetur adipisicing elit.", Picture = Device.RuntimePlatform == Device.UWP ? "Assets/img_1.png" : "img_1", Rating = 4, Votes = 81, SuggestionType = SuggestionType.Restaurant, Latitude = 47.5743905f, Longitude = -122.4023376f },
                new Suggestion { Name = "The Autumn Club", Description = "Loren ipsum dolor sit amet, consectetur adipisicing elit.", Picture = Device.RuntimePlatform == Device.UWP ? "Assets/img_2.png" : "img_2", Rating = 4, Votes = 66, SuggestionType = SuggestionType.Event, Latitude = 47.5790791f, Longitude = -122.4136163f },
                new Suggestion { Name = "Bike Rider", Description = "Loren ipsum dolor sit amet, consectetur adipisicing elit.", Picture = Device.RuntimePlatform == Device.UWP ? "Assets/img_3.png" : "img_3", Rating = 5, Votes = 22, SuggestionType = SuggestionType.Event, Latitude = 47.5766275f, Longitude = -122.4217906f },
                new Suggestion { Name = "C# Conference", Description = "Loren ipsum dolor sit amet, consectetur adipisicing elit.", Picture = Device.RuntimePlatform == Device.UWP ? "Assets/img_1.png" : "img_1", Rating = 4, Votes = 17, SuggestionType = SuggestionType.Event, Latitude = 47.5743905f, Longitude = -122.4023376f },
                new Suggestion { Name = "The Autumn Club", Description = "Loren ipsum dolor sit amet, consectetur adipisicing elit.", Picture = Device.RuntimePlatform == Device.UWP ? "Assets/img_2.png" : "img_2", Rating = 5, Votes = 132, SuggestionType = SuggestionType.Restaurant, Latitude = 47.5743905f, Longitude = -122.4023376f }
            };

            //CustomPin.Add(new CustomPin { Label = "Powai Mumbai", Position = new Xamarin.Forms.Maps.Position((long)Geolocation.savedPosition.Latitude, (long)Geolocation.savedPosition.Longitude), Type = SuggestionType.Event });
            //CustomPin.Add(new CustomPin { Label = "Malad Mumbai", Position = new Xamarin.Forms.Maps.Position((long)Geolocation.savedPosition.Latitude - 2, (long)Geolocation.savedPosition.Longitude - 2), Type = SuggestionType.Restaurant });
            //CustomPin.Add(new CustomPin { Label = " Borivali Mumbai", Position = new Xamarin.Forms.Maps.Position((long)Geolocation.savedPosition.Latitude + 2, (long)Geolocation.savedPosition.Longitude + 2), Type = SuggestionType.Event });
            //CustomPin.Add(new CustomPin { Label = "Nariman Point", Position = new Xamarin.Forms.Maps.Position((long)Geolocation.savedPosition.Latitude - 3, (long)Geolocation.savedPosition.Longitude - 3), Type = SuggestionType.Restaurant });
            //CustomPin.Add(new CustomPin { Label = "Bandra Mumbai", Position = new Xamarin.Forms.Maps.Position((long)Geolocation.savedPosition.Latitude + 1, (long)Geolocation.savedPosition.Longitude + 1), Type = SuggestionType.Event });
            //CustomPin.Add(new CustomPin { Label = "Juhu Mumbai", Position = new Xamarin.Forms.Maps.Position((long)Geolocation.savedPosition.Latitude + 5, (long)Geolocation.savedPosition.Longitude + 5), Type = SuggestionType.Restaurant });
            List<CustomPin> templist = new List<CustomPin>();
            foreach (var suggestion in Suggestions)
            {
                templist.Add(new CustomPin
                {
                    Label = suggestion.Name,
                    Position = new Xamarin.Forms.Maps.Position(suggestion.Latitude, suggestion.Longitude),
                    Type = suggestion.SuggestionType
                });
            }

            CustomPins = new ObservableCollection<CustomPin>(templist);
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
