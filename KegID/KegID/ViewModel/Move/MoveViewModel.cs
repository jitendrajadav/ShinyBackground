using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KegID.Common;
using KegID.Model;
using KegID.Response;
using KegID.Services;
using KegID.SQLiteClient;
using KegID.View;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class MoveViewModel : ViewModelBase
    {
        int count;
        bool tracking;
        Position savedPosition;
        public ObservableCollection<Position> Positions { get; } = new ObservableCollection<Position>();

        #region Properties

        public string MenifestId { get; set; }
        public IMoveService _moveService { get; set; }

        #region MenifestRefId

        /// <summary>
        /// The <see cref="MenifestRefId" /> property's name.
        /// </summary>
        public const string MenifestRefIdPropertyName = "MenifestRefId";

        private string _MenifestRefId = default(string);

        /// <summary>
        /// Sets and gets the MenifestRefId property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string MenifestRefId
        {
            get
            {
                return _MenifestRefId;
            }

            set
            {
                if (_MenifestRefId == value)
                {
                    return;
                }

                _MenifestRefId = value;
                RaisePropertyChanged(MenifestRefIdPropertyName);
            }
        }

        #endregion

        #region Destination

        /// <summary>
        /// The <see cref="Destination" /> property's name.
        /// </summary>
        public const string DestinationPropertyName = "Destination";

        private PartnerModel _destination =  new PartnerModel();

        /// <summary>
        /// Sets and gets the Destination property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public PartnerModel Destination
        {
            get
            {
                return _destination;
            }

            set
            {
                if (_destination == value)
                {
                    return;
                }

                _destination = value;
                RaisePropertyChanged(DestinationPropertyName);
            }
        }

        #endregion

        #region TagsStr

        /// <summary>
        /// The <see cref="TagsStr" /> property's name.
        /// </summary>
        public const string TagsStrPropertyName = "TagsStr";

        private string _tagsStr = "Add info";

        /// <summary>
        /// Sets and gets the TagsStr property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string TagsStr
        {
            get
            {
                return _tagsStr;
            }

            set
            {
                if (_tagsStr == value)
                {
                    return;
                }

                _tagsStr = value;
                RaisePropertyChanged(TagsStrPropertyName);
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

        #endregion

        #region Commands

        public RelayCommand SelectLocationCommand { get; set; }
        public RelayCommand MoreInfoCommand { get; set; }
        public RelayCommand ScanKegsCommad { get; set; }
        public RelayCommand SaveDraftCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        #endregion

        #region Constructor
        public MoveViewModel(IMoveService moveService)
        {
            _moveService = moveService;

            SelectLocationCommand = new RelayCommand(SelectLocationCommandRecieverAsync);
            MoreInfoCommand = new RelayCommand(MoreInfoCommandRecieverAsync);
            ScanKegsCommad = new RelayCommand(ScanKegsCommadRecieverAsync);
            SaveDraftCommand = new RelayCommand(SaveDraftCommandRecieverAsync);
            CancelCommand = new RelayCommand(CancelCommandRecieverAsync);
            Destination.FullName = "Select a location";
            GetGPS();
        }


        #endregion

        #region Methods

        private async void SaveDraftCommandRecieverAsync()
        {
            DraftManifestModel manifestModel = new DraftManifestModel()
            {
                ClosedBatches = 0,
                DestinationId = Destination.FullName,
                EffectiveDate = DateTime.Today,
                EventTypeId = 0,
                GS1GSIN = "",
                IsSendManifest = true,
                KegOrderId = "",
                Latitude = savedPosition.Latitude,
                Longitude = savedPosition.Longitude,
                NewBatch = "",
                NewBatches = 0,
                NewPallets = 0,
                OriginId = Destination.Address,
                PostedDate = DateTime.Today,
                SourceKey = "",
                SubmittedDate = DateTime.Today,
                Tags = TagsStr,
                ManifestItems = Convert.ToInt64(AddKegs.Split(' ').FirstOrDefault()),
                Id = 0,
                ManifestId = MenifestRefId.Split(':').LastOrDefault().Trim(),
                ReceiverId = Destination.FullName,
                SenderId = Destination.Address,
                ShipDate = DateTime.Today,
            };
            await SQLiteServiceClient.Db.InsertAsync(manifestModel);

            ManifestModel manifestPostModel = new ManifestModel()
            {
                ManifestId = MenifestId,
                EventTypeId = (long)EventTypeEnum.MOVE_MANIFEST,
                Latitude = (long)savedPosition.Latitude,
                Longitude = (long)savedPosition.Longitude,
                SubmittedDate = DateTime.Today,
                ShipDate = DateTime.Today,

                SenderId = Destination.Address,
                ReceiverId = Destination.FullName,
                DestinationName = Destination.FullName,
                DestinationTypeCode = Destination.LocationCode,

                ManifestItems = new List<ManifestItem>()
                {
                   new ManifestItem()
                   {
                        Barcode = "",
                        ScanDate = DateTime.Today,
                        ValidationStatus =2,
                        KegId = "",
                        Tags =Tags,  
                       KegStatus = new List<KegStatus>()
                       {
                            new KegStatus()
                            {
                                KegId= "",
                                Barcode="",
                                AltBarcode="",
                                Contents ="",
                                Batch ="",
                                Size ="",
                                Alert = "",
                                Location = new Location()
                                {
                                    Name ="",
                                    TypeCode ="",
                                    EntityId = ""
                                },
                                OwnerName = "",
                            }
                        },
                   }
                }
            };

            var result = await _moveService.PostManifestAsync(manifestPostModel,Configuration.SessionId);

            await Application.Current.MainPage.Navigation.PushModalAsync(new ManifestsView());

            //var manifest = await _moveService.GetManifestListAsync(Configuration.SessionId);
        }

        public void GetUuId()
        {
            MenifestId = Guid.NewGuid().ToString();
            MenifestRefId = Regex.Match(MenifestId, @"(.{8})\s*$").Value.ToUpper();
        }
        private async void CancelCommandRecieverAsync()
        {
            var result = await Application.Current.MainPage.DisplayActionSheet("Cancel? \n You have like to save this manifest as a draft or delete?",null,null, "Delete manifest", "Save as draft");
            if (result== "Delete manifest")
            {
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }

        private async void SelectLocationCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PushModalAsync(new PartnersView());

        private async void MoreInfoCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PushModalAsync(new AddTagsView());

        private async void ScanKegsCommadRecieverAsync() => await Application.Current.MainPage.Navigation.PushModalAsync(new ScanKegsView());

        private async void LastCached(object sender, EventArgs e)
        {
            try
            {
                var hasPermission = await Utils.CheckPermissions(Permission.Location);
                if (!hasPermission)
                    return;


                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 500;
                //LabelCached.Text = "Getting gps...";

                var position = await locator.GetLastKnownLocationAsync();

                if (position == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "null cached location :(", "Ok");
                    return;
                }

                savedPosition = position;
                var value = string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
                    position.Timestamp, position.Latitude, position.Longitude,
                    position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await Application.Current.MainPage.DisplayAlert("Uh oh", "Something went wrong, but don't worry we captured for analysis! Thanks.", "OK");
            }
            finally
            {
            }
        }

        private async void GetGPS()
        {
            try
            {
                var hasPermission = await Utils.CheckPermissions(Permission.Location);
                if (!hasPermission)
                    return;

                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 500;
                //labelGPS.Text = "Getting gps...";

                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(15), null, true);

                if (position == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "null gps :(", "cancel");
                    return;
                }
                savedPosition = position;
                var valu = string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
                    position.Timestamp, position.Latitude, position.Longitude,
                    position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await Application.Current.MainPage.DisplayAlert("Uh oh", "Something went wrong, but don't worry we captured for analysis! Thanks.", "OK");
            }
            finally
            {
                ButtonAddressForPosition();
            }
        }

        private async void ButtonAddressForPosition()
        {
            try
            {
                if (savedPosition == null)
                    return;

                var hasPermission = await Utils.CheckPermissions(Permission.Location);
                if (!hasPermission)
                    return;

                var locator = CrossGeolocator.Current;

                var address = await locator.GetAddressesForPositionAsync(savedPosition, "RJHqIE53Onrqons5CNOx~FrDr3XhjDTyEXEjng-CRoA~Aj69MhNManYUKxo6QcwZ0wmXBtyva0zwuHB04rFYAPf7qqGJ5cHb03RCDw1jIW8l");
                if (address == null || address.Count() == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Unable to find address", "Ok");
                }

                var a = address.FirstOrDefault();
                var value = $"Address: Thoroughfare = {a.Thoroughfare}\nLocality = {a.Locality}\nCountryCode = {a.CountryCode}\nCountryName = {a.CountryName}\nPostalCode = {a.PostalCode}\nSubLocality = {a.SubLocality}\nSubThoroughfare = {a.SubThoroughfare}";

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await Application.Current.MainPage.DisplayAlert("Uh oh", "Something went wrong, but don't worry we captured for analysis! Thanks.", "OK");
            }
            finally
            {
            }
        }

        private async void ButtonTrack_Clicked(object sender, EventArgs e)
        {
            try
            {
                var hasPermission = await Utils.CheckPermissions(Permission.Location);
                if (!hasPermission)
                    return;

                if (tracking)
                {
                    CrossGeolocator.Current.PositionChanged -= CrossGeolocator_Current_PositionChanged;
                    CrossGeolocator.Current.PositionError -= CrossGeolocator_Current_PositionError;
                }
                else
                {
                    CrossGeolocator.Current.PositionChanged += CrossGeolocator_Current_PositionChanged;
                    CrossGeolocator.Current.PositionError += CrossGeolocator_Current_PositionError;
                }

                if (CrossGeolocator.Current.IsListening)
                {
                    await CrossGeolocator.Current.StopListeningAsync();
                    //labelGPSTrack.Text = "Stopped tracking";
                    //ButtonTrack.Text = "Start Tracking";
                    tracking = false;
                    count = 0;
                }
                else
                {
                    Positions.Clear();
                    if (await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(10), 10, true, null))
                    {
                        //labelGPSTrack.Text = "Started tracking";
                        //ButtonTrack.Text = "Stop Tracking";
                        tracking = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await Application.Current.MainPage.DisplayAlert("Uh oh", "Something went wrong, but don't worry we captured for analysis! Thanks.", "OK");
            }
        }

        private void CrossGeolocator_Current_PositionError(object sender, PositionErrorEventArgs e)
        {
            var str = "Location error: " + e.Error.ToString();
        }

        void CrossGeolocator_Current_PositionChanged(object sender, PositionEventArgs e)
        {

            Device.BeginInvokeOnMainThread(() =>
            {
                var position = e.Position;
                Positions.Add(position);
                count++;
                var upate = $"{count} updates";
                var latinfo = string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
                    position.Timestamp, position.Latitude, position.Longitude,
                    position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);

            });
        }

        #endregion
    }
}
