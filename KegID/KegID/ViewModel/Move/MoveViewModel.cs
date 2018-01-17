using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KegID.Response;
using KegID.Services;
using KegID.SQLiteClient;
using KegID.View;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.ObjectModel;
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

        #region Tags

        /// <summary>
        /// The <see cref="Tags" /> property's name.
        /// </summary>
        public const string TagsPropertyName = "Tags";

        private string _tags = "Add info";

        /// <summary>
        /// Sets and gets the Tags property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Tags
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

        public string BrewersAddress { get; set; }

        #endregion

        #region Commands

        public RelayCommand ScannedCommand { get; set; }

        public RelayCommand SelectLocationCommand { get; set; }

        public RelayCommand MoreInfoCommand { get; set; }

        public RelayCommand ScanKegsCommad { get; set; }

        public RelayCommand SaveDraftCommand { get; set; }

        #endregion

        #region Constructor
        public MoveViewModel(IMoveService moveService)
        {
            _moveService = moveService;

            ScannedCommand = new RelayCommand(ScannedCommandRecieverAsync);
            SelectLocationCommand = new RelayCommand(SelectLocationCommandRecieverAsync);
            MoreInfoCommand = new RelayCommand(MoreInfoCommandRecieverAsync);
            ScanKegsCommad = new RelayCommand(ScanKegsCommadRecieverAsync);
            SaveDraftCommand = new RelayCommand(SaveDraftCommandRecieverAsync);
            Destination.FullName = "Select a location";
        }

        #endregion

        #region Methods

        private async void SaveDraftCommandRecieverAsync()
        {
            ManifestModel manifestModel = new ManifestModel()
            {
                ClosedBatches = 0,
                DestinationId = Destination.FullName,
                EffectiveDate = DateTime.Today,
                EventTypeId = 0,
                GS1GSIN = "",
                IsSendManifest = true,
                KegOrderId = "",
                Latitude = 12.20,
                Longitude = 15.22,
                NewBatch = "",
                NewBatches = 0,
                NewPallets = 0,
                OriginId = Destination.Address,
                PostedDate = DateTime.Today,
                SourceKey = "",
                SubmittedDate = DateTime.Today,
                Tags = Tags,
                ManifestItems = Convert.ToInt64(AddKegs.Split(' ').FirstOrDefault()),
                Id = 0,
                ManifestId = MenifestRefId.Split(':').LastOrDefault().Trim(),
                ReceiverId = Destination.FullName,
                SenderId = Destination.Address,
                ShipDate = DateTime.Today,
            };
            await SQLiteServiceClient.Db.InsertAsync(manifestModel);

            //var manifest = await _moveService.GetManifestListAsync(Configuration.SessionId);
        }

        private async void ScannedCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new ScannerView());
        }

        public void GetUuId()
        {
            MenifestRefId = "Menifest #: " + Regex.Match(Guid.NewGuid().ToString(), @"(.{8})\s*$").Value.ToUpper();
        }

        private async void SelectLocationCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new PartnersView());
        }

        private async void MoreInfoCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new AddTagsView());
        }
        private async void ScanKegsCommadRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new ScanKegsView());
        }
        
        #endregion
    }
}
