using Acr.UserDialogs;
using KegID.Common;
using KegID.Delegates;
using KegID.LocalDb;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Realms;
using Shiny;
using Shiny.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class MoveViewModel : BaseViewModel, IDestructible
    {
        #region Properties

        private readonly IPageDialogService _dialogService;
        private readonly IManifestManager _manifestManager;
        private readonly IUuidManager _uuidManager;
        //private readonly IGeolocationService _geolocationService;
        private readonly IGpsListener _gpsListener;
        private readonly IGpsManager _gpsManager;

        public string ContainerTypes { get; set; }
        public Position LocationMessage { get; set; }
        public string Contents { get; set; }
        public IList<BarcodeModel> Barcodes { get; set; }
        public string ManifestId { get; set; }
        public void OnManifestIdChanged()
        {
            ConstantManager.ManifestId = ManifestId;
        }

        public string Destination { get; set; } = "Select a location";
        public string TagsStr { get; set; } = "Add info";
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public string AddKegs { get; set; }

        public void OnAddKegsChanged()
        {
            IsSubmitVisible = AddKegs.Contains("Item");
        }

        public bool OriginRequired { get; set; }
        public void OnOriginRequiredChanged()
        {
            IsOriginRequired = OriginRequired;
        }
        public bool IsOriginRequired { get; set; }
        public bool IsSaveDraftVisible { get; set; }
        public string Order { get; set; }
        public bool IsSubmitVisible { get; set; }
        public void OnIsSubmitVisibleChanged()
        {
            IsSaveDraftVisible = IsSubmitVisible;
        }
        public bool IsEffectiveDateAllowed { get; set; }
        public DateTimeOffset EffectiveDateAllowed { get; set; } = DateTimeOffset.UtcNow.Date;
        public bool IsRequiredVisible { get; set; } = true;
        public bool OrderNumRequired { get; set; }
        public string Origin { get; set; } = "Select a location";
        public void OnOriginChanged()
        {
            IsOriginRequired = false;
        }
        public bool Operator { get; set; }

        #endregion

        #region Commands

        public DelegateCommand SelectLocationCommand { get; }
        public DelegateCommand SelectOriginLocationCommand { get; }
        public DelegateCommand MoreInfoCommand { get; }
        public DelegateCommand ScanKegsCommad { get; }
        public DelegateCommand SaveDraftCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand SubmitCommand { get; }
        public object RealmDb { get; }

        #endregion

        #region Constructor

        public MoveViewModel(INavigationService navigationService, IPageDialogService dialogService, IManifestManager manifestManager, IUuidManager uuidManager, IGpsManager gpsManager, IGpsListener gpsListener) : base(navigationService)
        {
            _dialogService = dialogService;
            _manifestManager = manifestManager;
            _uuidManager = uuidManager;
            //_geolocationService = geolocationService;
            _gpsManager = gpsManager;
            _gpsListener = gpsListener;
            _gpsListener.OnReadingReceived += OnReadingReceived;

            SelectLocationCommand = new DelegateCommand(SelectLocationCommandRecieverAsync);
            SelectOriginLocationCommand = new DelegateCommand(SelectOriginLocationCommandRecieverAsync);
            MoreInfoCommand = new DelegateCommand(MoreInfoCommandRecieverAsync);
            ScanKegsCommad = new DelegateCommand(ScanKegsCommadRecieverAsync);
            SaveDraftCommand = new DelegateCommand(SaveDraftCommandRecieverAsync);
            CancelCommand = new DelegateCommand(CancelCommandRecieverAsync);
            SubmitCommand = new DelegateCommand(async () => await RunSafe(SubmitCommandRecieverAsync()));

            PreferenceSetting();
            AddKegs = string.Format("Add {0}", ContainerTypes);
        }

        #endregion

        #region Methods

        public void OnReadingReceived(object sender, GpsReadingEventArgs e)
        {
            LocationMessage = e.Reading.Position;
            //= $"{e.Reading.Position.Latitude}, {e.Reading.Position.Longitude}";
        }

        private void PreferenceSetting()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            var preferences = RealmDb.All<Preference>().ToList();

            var preferenceOrderNumRequired = preferences.Find(x => x.PreferenceName == "OrderNumRequired");
            OrderNumRequired = preferenceOrderNumRequired != null && bool.Parse(preferenceOrderNumRequired.PreferenceValue);

            var preferenceOriginRequired = preferences.Find(x => x.PreferenceName == "OriginRequired");
            OriginRequired = preferenceOriginRequired != null && bool.Parse(preferenceOriginRequired.PreferenceValue);

            var effectiveDate = preferences.Find(x => x.PreferenceName == "EffectiveDateAllowed");
            IsEffectiveDateAllowed = effectiveDate != null && bool.Parse(effectiveDate.PreferenceValue);

            var preferenceOperator = preferences.Find(x => x.PreferenceName == "Operator");
            Operator = preferenceOperator != null && bool.Parse(preferenceOperator.PreferenceValue);

            ContainerTypes = preferences.Find(x => x.PreferenceName == "CONTAINER_TYPES")?.PreferenceValue;
        }

        private async Task SubmitCommandRecieverAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Loading");
                //var location = await _geolocationService.GetLastLocationAsync();

                ManifestModel manifestPostModel = null;
                try
                {
                    manifestPostModel = GenerateManifest(LocationMessage ?? new Position(0, 0));
                    if (manifestPostModel != null)
                    {
                        try
                        {
                            var current = Connectivity.NetworkAccess;
                            if (current == NetworkAccess.Internet)
                            {
                                var response = await ApiManager.PostManifest(manifestPostModel, Settings.SessionId);

                                try
                                {
                                    AddorUpdateManifestOffline(manifestPostModel, false);
                                }
                                catch (Exception ex)
                                {
                                    Crashes.TrackError(ex);
                                }
                                await GetPostedManifestDetail();
                            }
                            else
                            {
                                try
                                {
                                    AddorUpdateManifestOffline(manifestPostModel, true);
                                }
                                catch (Exception ex)
                                {
                                    Crashes.TrackError(ex);
                                }
                                await GetPostedManifestDetail();
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }
                        finally
                        {
                            manifestPostModel = null;
                            Cleanup();
                        }
                    }
                    else
                        await _dialogService.DisplayAlertAsync("Alert", "Something goes wrong please check again", "Ok");
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private async Task GetPostedManifestDetail()
        {
            string sizeName = string.Empty;
            ManifestResponseModel manifest = new ManifestResponseModel();
            try
            {
                sizeName = ConstantManager.Tags.LastOrDefault().Value;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            try
            {
                manifest.ManifestItems = new List<CreatedManifestItem>();
                foreach (var item in Barcodes)
                {
                    manifest.ManifestItems.Add(new CreatedManifestItem
                    {
                        Barcode = item.Barcode,
                        Contents = Contents,
                        Keg = new CreatedManifestKeg
                        {
                            Barcode = item.Barcode,
                            Contents = Contents,
                            VolumeName = "Needs to add here!",
                            OwnerName = ConstantManager.Partner.FullName,
                            SizeName = sizeName,
                        }
                    });
                }

                manifest.TrackingNumber = ManifestId;
                manifest.ShipDate = DateTimeOffset.UtcNow.Date.ToShortDateString();
                manifest.CreatorCompany = new CreatorCompany { Address = ConstantManager.Partner.Address, State = ConstantManager.Partner.State, PostalCode = ConstantManager.Partner.PostalCode, Lon = ConstantManager.Partner.Lon, Address1 = ConstantManager.Partner.Address1, City = ConstantManager.Partner.City, CompanyNo = ConstantManager.Partner.CompanyNo.ToString(), Country = ConstantManager.Partner.Country, FullName = ConstantManager.Partner.FullName, IsActive = ConstantManager.Partner.IsActive, IsInternal = ConstantManager.Partner.IsInternal, IsShared = ConstantManager.Partner.IsShared, Lat = ConstantManager.Partner.Lat, LocationCode = ConstantManager.Partner.LocationCode, LocationStatus = ConstantManager.Partner.LocationStatus, MasterCompanyId = ConstantManager.Partner.MasterCompanyId, ParentPartnerId = ConstantManager.Partner.ParentPartnerId, ParentPartnerName = ConstantManager.Partner.ParentPartnerName, PartnerId = ConstantManager.Partner.PartnerId, PartnershipIsActive = ConstantManager.Partner.PartnershipIsActive, PartnerTypeCode = ConstantManager.Partner.PartnerTypeCode, PartnerTypeName = ConstantManager.Partner.PartnerTypeName, PhoneNumber = ConstantManager.Partner.PhoneNumber, SourceKey = ConstantManager.Partner.SourceKey };
                manifest.SenderPartner = new CreatorCompany { Address = ConstantManager.Partner.Address/*, State = ConstantManager.Partner.State, PostalCode = ConstantManager.Partner.PostalCode, Lon = ConstantManager.Partner.Lon, Address1 = ConstantManager.Partner.Address1, City = ConstantManager.Partner.City, CompanyNo = ConstantManager.Partner.CompanyNo.HasValue ? ConstantManager.Partner.CompanyNo.Value.ToString() : string.Empty, Country = ConstantManager.Partner.Country, FullName = ConstantManager.Partner.FullName, IsActive = ConstantManager.Partner.IsActive, IsInternal = ConstantManager.Partner.IsInternal, IsShared = ConstantManager.Partner.IsShared, Lat = ConstantManager.Partner.Lat, LocationCode = ConstantManager.Partner.LocationCode, LocationStatus = ConstantManager.Partner.LocationStatus, MasterCompanyId = ConstantManager.Partner.MasterCompanyId, ParentPartnerId = ConstantManager.Partner.ParentPartnerId, ParentPartnerName = ConstantManager.Partner.ParentPartnerName, PartnerId = ConstantManager.Partner.PartnerId, PartnershipIsActive = ConstantManager.Partner.PartnershipIsActive, PartnerTypeCode = ConstantManager.Partner.PartnerTypeCode, PartnerTypeName = ConstantManager.Partner.PartnerTypeName, PhoneNumber = ConstantManager.Partner.PhoneNumber, SourceKey = ConstantManager.Partner.SourceKey */};
                manifest.ReceiverPartner = new CreatorCompany { Address = ConstantManager.Partner.Address, State = ConstantManager.Partner.State, PostalCode = ConstantManager.Partner.PostalCode, Lon = ConstantManager.Partner.Lon, Address1 = ConstantManager.Partner.Address1, City = ConstantManager.Partner.City, CompanyNo = ConstantManager.Partner.CompanyNo.ToString(), Country = ConstantManager.Partner.Country, FullName = ConstantManager.Partner.FullName, IsActive = ConstantManager.Partner.IsActive, IsInternal = ConstantManager.Partner.IsInternal, IsShared = ConstantManager.Partner.IsShared, Lat = ConstantManager.Partner.Lat, LocationCode = ConstantManager.Partner.LocationCode, LocationStatus = ConstantManager.Partner.LocationStatus, MasterCompanyId = ConstantManager.Partner.MasterCompanyId, ParentPartnerId = ConstantManager.Partner.ParentPartnerId, ParentPartnerName = ConstantManager.Partner.ParentPartnerName, PartnerId = ConstantManager.Partner.PartnerId, PartnershipIsActive = ConstantManager.Partner.PartnershipIsActive, PartnerTypeCode = ConstantManager.Partner.PartnerTypeCode, PartnerTypeName = ConstantManager.Partner.PartnerTypeName, PhoneNumber = ConstantManager.Partner.PhoneNumber, SourceKey = ConstantManager.Partner.SourceKey };
                manifest.SenderShipAddress = new Address { City = ConstantManager.Partner.City, Country = ConstantManager.Partner.Country, Geocoded = false, Latitude = (long)ConstantManager.Partner.Lat, Line1 = ConstantManager.Partner.Address, Line2 = ConstantManager.Partner.Address1, Longitude = (long)ConstantManager.Partner.Lon, PostalCode = ConstantManager.Partner.PostalCode, State = ConstantManager.Partner.State };
                manifest.ReceiverShipAddress = new Address { City = ConstantManager.Partner.City, Country = ConstantManager.Partner.Country, Geocoded = false, Latitude = (long)ConstantManager.Partner.Lat, Line1 = ConstantManager.Partner.Address, Line2 = ConstantManager.Partner.Address1, Longitude = (long)ConstantManager.Partner.Lon, PostalCode = ConstantManager.Partner.PostalCode, State = ConstantManager.Partner.State };
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            await _navigationService.NavigateAsync("ManifestDetailView", new NavigationParameters
                                {
                                    { "manifest", manifest },{ "Contents", Contents }
                                }, animated: false);
        }

        private void AddorUpdateManifestOffline(ManifestModel manifestPostModel, bool queue)
        {
            string manifestId = manifestPostModel.ManifestId;
            var isNew = Realm.GetInstance(RealmDbManager.GetRealmDbConfig()).Find<ManifestModel>(manifestId);
            if (isNew != null)
            {
                try
                {
                    manifestPostModel.IsDraft = false;
                    var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                    RealmDb.Write(() =>
                    {
                        RealmDb.Add(manifestPostModel, update: true);
                    });
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
            else
            {
                try
                {
                    if (queue)
                    {
                        manifestPostModel.IsQueue = true;
                    }
                    var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                    RealmDb.Write(() =>
                    {
                        RealmDb.Add(manifestPostModel);
                    });
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        private async void SaveDraftCommandRecieverAsync()
        {
            //var location = await _geolocationService.GetLastLocationAsync();

            ManifestModel manifestModel = null;

            manifestModel = GenerateManifest(LocationMessage ?? new Position(0, 0));
            if (manifestModel != null)
            {

                manifestModel.IsDraft = true;
                var isNew = Realm.GetInstance(RealmDbManager.GetRealmDbConfig()).Find<ManifestModel>(manifestModel.ManifestId);
                if (isNew != null)
                {
                    var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                    RealmDb.Write(() =>
                    {
                        RealmDb.Add(manifestModel, update: true);
                    });
                }
                else
                {
                    var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                    RealmDb.Write(() =>
                   {
                       RealmDb.Add(manifestModel);
                   });
                }

                await _navigationService.NavigateAsync("ManifestsView",
                    new NavigationParameters
                    {
                                { "LoadDraftManifestAsync", "LoadDraftManifestAsync" }
                    }, animated: false);
            }
            else
            {
                await _dialogService.DisplayAlertAsync("Error", "Could not save manifest.", "Ok");
            }
            Cleanup();
        }

        public ManifestModel GenerateManifest(Position location)
        {
            return _manifestManager.GetManifestDraft(eventTypeEnum: EventTypeEnum.MOVE_MANIFEST, manifestId: ManifestId,
                        barcodeCollection: ConstantManager.Barcodes ?? new List<BarcodeModel>(), (long)location.Latitude, (long)location.Longitude, Origin, Order, tags: Tags ?? new List<Tag>(), tagsStr: TagsStr,
                        partnerModel: ConstantManager.Partner, newPallets: new List<NewPallet>(), batches: new List<NewBatch>(),
                        closedBatches: new List<string>(), null, validationStatus: 2, EffectiveDateAllowed, contents: Contents);
        }

        internal void AssingScanKegsValue(List<BarcodeModel> _barcodes, List<Tag> _tags, string _contents)
        {
            try
            {
                Contents = _contents;
                Tags = _tags;
                Barcodes = _barcodes;
                if (_barcodes.Count > 1)
                    AddKegs = string.Format("{0} Items", _barcodes.Count);
                else if (_barcodes.Count == 1)
                    AddKegs = string.Format("{0} Item", _barcodes.Count);
                else
                    AddKegs = string.Format("Add {0}", ContainerTypes);
                //if (!IsSubmitVisible)
                //    IsSubmitVisible = true;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal void AssignAddTagsValue(List<Tag> _tags, string _tagsStr)
        {
            try
            {
                Tags = _tags;
                TagsStr = _tagsStr;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void CancelCommandRecieverAsync()
        {
            try
            {
                var result = await _dialogService.DisplayActionSheetAsync("Cancel? \n Would you like to save this manifest as a draft or delete?", null, null, "Delete manifest", "Save as draft");
                if (result == "Delete manifest")
                {
                    // Delete an object with a transaction
                    DeleteManifest(ManifestId);
                    await _navigationService.GoBackAsync(animated: false);
                }
                else if (result == "Save as draft")
                {
                    //Save Draft Logic here...
                    SaveDraftCommandRecieverAsync();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                Cleanup();
            }
        }

        private void DeleteManifest(string manifestId)
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                var manifest = RealmDb.All<ManifestModel>().First(b => b.ManifestId == manifestId);
                using (var trans = RealmDb.BeginWrite())
                {
                    RealmDb.Remove(manifest);
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void SelectLocationCommandRecieverAsync()
        {
            await _navigationService.NavigateAsync("PartnersView", new NavigationParameters
                    {
                        { "GoingFrom",  "Destination" }
                    }, animated: false);
        }

        private async void SelectOriginLocationCommandRecieverAsync()
        {
            await _navigationService.NavigateAsync("PartnersView", new NavigationParameters
                    {
                        { "GoingFrom",  "MoveOrigin" }
                    }, animated: false);
        }

        private async void MoreInfoCommandRecieverAsync()
        {
            await _navigationService.NavigateAsync("AddTagsView", new NavigationParameters
                    {
                        {"viewTypeEnum",ViewTypeEnum.MoveView }
                    }, animated: false);

        }

        private async void ScanKegsCommadRecieverAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(ConstantManager.Partner?.PartnerId))
                {
                    await _dialogService.DisplayAlertAsync("Error", "Please select a destination first.", "Ok");
                    return;
                }
                if (OrderNumRequired && string.IsNullOrEmpty(Order))
                {
                    await _dialogService.DisplayAlertAsync("Error", "Please enter order first.", "Ok");
                    return;
                }
                if (OriginRequired && Origin.Contains("Select a location"))
                {
                    await _dialogService.DisplayAlertAsync("Error", "Please select a origin first.", "Ok");
                    return;
                }
                else if (Barcodes != null)
                {
                    if (TargetIdiom.Tablet == Device.Idiom)
                    {
                        await _navigationService.NavigateAsync("ScanKegsTabView", new NavigationParameters
                            {
                                { "models", Barcodes }
                            }, animated: false);
                    }
                    else
                    {
                        await _navigationService.NavigateAsync("ScanKegsView", new NavigationParameters
                            {
                                { "models", Barcodes }
                            }, animated: false);
                    }
                }
                else
                {
                    if (TargetIdiom.Tablet == Device.Idiom)
                    {
                        await _navigationService.NavigateAsync("ScanKegsTabView", animated: false);
                    }
                    else
                    {
                        await _navigationService.NavigateAsync("ScanKegsView", animated: false);
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal void AssignInitialValue(string _kegId, IList<ManifestItem> _barcode, string _addKegs, string _destination, string _partnerId, bool isSaveDraftVisible, IList<Tag> tags, string tagsStr)
        {
            try
            {
                if (_barcode != null)
                {
                    foreach (var item in _barcode)
                    {
                        var model = new BarcodeModel
                        {
                            Barcode = item?.Barcode,
                            Icon = item?.Icon,
                            TagsStr = item?.TagsStr
                        };
                        foreach (Tag tag in item?.Tags)
                        {
                            model.Tags.Add(tag);
                        }
                        ConstantManager.Barcodes.Add(model);
                    }
                }
                Barcodes = ConstantManager.Barcodes;
                Tags = tags?.ToList();
                TagsStr = !string.IsNullOrEmpty(tagsStr) ? tagsStr : "Add info";
                ManifestId = !string.IsNullOrEmpty(_kegId) ? _kegId : _uuidManager.GetUuId();
                AddKegs = !string.IsNullOrEmpty(_addKegs) ? Convert.ToUInt32(_addKegs) > 1 ? string.Format("{0} Items", _addKegs) : string.Format("{0} Item", _addKegs) : string.Format("Add {0}", ContainerTypes);
                if (!string.IsNullOrEmpty(_destination))
                {
                    Destination = _destination;
                    ConstantManager.Partner = new PartnerModel
                    {
                        PartnerId = _partnerId,
                        FullName = _destination
                    };
                    IsRequiredVisible = false;
                }
                IsSaveDraftVisible = isSaveDraftVisible;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public void Cleanup()
        {
            try
            {
                TagsStr = "Add info";
                AddKegs = string.Format("Add {0}", ContainerTypes);
                IsSaveDraftVisible = false;
                IsSubmitVisible = false;
                IsRequiredVisible = true;
                Destination = "Select a location";
                ManifestId = _uuidManager.GetUuId();
                Tags = null;
                Barcodes = null;
                ConstantManager.Barcodes.Clear();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("CancelCommandRecieverAsync"))
            {
                CancelCommandRecieverAsync();
            }
            if (ConstantManager.Barcodes?.Count > 0)
                AssingScanKegsValue(ConstantManager.Barcodes.ToList(), ConstantManager.Tags, ConstantManager.Contents);

            switch (parameters.Keys.FirstOrDefault())
            {
                case "model":
                    if (parameters.GetValue<string>("CommingFrom") == "MoveOrigin")
                    {
                        Origin = ConstantManager.Partner.FullName;
                        IsOriginRequired = false;
                    }
                    else
                    {
                        Destination = ConstantManager.Partner.FullName;
                        IsRequiredVisible = false;
                    }
                    IsSaveDraftVisible = true;
                    break;
                case "AddTags":
                    AssignAddTagsValue(ConstantManager.Tags, ConstantManager.TagsStr);
                    break;
                case "AssignInitialValue":
                    AssignInitialValue(parameters);
                    break;
                case "AssignInitialValueFromKegStatus":
                    AssignInitialValueFromKegStatus(parameters);
                    break;
                case "PartnerModel":
                    Destination = parameters.GetValue<PossessorLocation>("PartnerModel").FullName;
                    break;
            }

            if (_gpsManager.IsListening)
            {
                await _gpsManager.StopListener();
            }

            await _gpsManager.StartListener(new GpsRequest
            {
                UseBackground = true,
                Priority = GpsPriority.Highest,
                Interval = TimeSpan.FromSeconds(5),
                ThrottledInterval = TimeSpan.FromSeconds(3) //Should be lower than Interval
            });
        }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            if (ConstantManager.Barcodes?.Count > 0)
                AssingScanKegsValue(ConstantManager.Barcodes.ToList(), ConstantManager.Tags, ConstantManager.Contents);

            if (parameters.ContainsKey("ManifestId"))
            {
                ManifestId = parameters.GetValue<string>("ManifestId");
            }

            return base.InitializeAsync(parameters);
        }

        private void AssignInitialValueFromKegStatus(INavigationParameters parameters)
        {
            List<string> Barcode = null;
            List<ManifestItem> manifestItems = new List<ManifestItem>();

            var getType = parameters.FirstOrDefault().Value.GetType();
            if (getType.FullName.Contains("System.Collections.Generic"))
            {
                Barcode = parameters.GetValue<List<string>>("AssignInitialValueFromKegStatus");
                ManifestItem manifestItem;
                foreach (var item in Barcode)
                {
                    manifestItem = new ManifestItem
                    {
                        Barcode = item
                    };
                    manifestItems.Add(manifestItem);
                }
            }
            else
            {
                string barcode = parameters.GetValue<string>("AssignInitialValueFromKegStatus");
                ManifestItem manifestItem = new ManifestItem
                {
                    Barcode = barcode
                };
                manifestItems.Add(manifestItem);
            }
            string KegId = parameters.GetValue<string>("KegId");

            AssignInitialValue(KegId, manifestItems, "1", string.Empty, string.Empty, true, null, string.Empty);
        }

        private void AssignInitialValue(INavigationParameters parameters)
        {
            ManifestModel model = parameters.GetValue<ManifestModel>("AssignInitialValue");
            try
            {
                if (model != null)
                {
                    foreach (var item in model.BarcodeModels)
                    {
                        ConstantManager.Barcodes.Add(item);
                    }
                }
                Barcodes = ConstantManager.Barcodes;
                Tags = model.Tags?.ToList();
                TagsStr = !string.IsNullOrEmpty(model.TagsStr) ? model.TagsStr : "Add info";
                ManifestId = !string.IsNullOrEmpty(model.ManifestId) ? model.ManifestId : _uuidManager.GetUuId();
                AddKegs = !string.IsNullOrEmpty(model.ManifestItemsCount.ToString()) ? Convert.ToUInt32(model.ManifestItemsCount.ToString()) > 1 ? string.Format("{0} Items", model.ManifestItemsCount.ToString()) : string.Format("{0} Item", model.ManifestItemsCount.ToString()) : string.Format("Add {0}", ContainerTypes);
                if (!string.IsNullOrEmpty(model.OwnerName))
                {
                    Destination = model.OwnerName;
                    Origin = model.OriginId;
                    Order = model.KegOrderId;
                    ConstantManager.Partner = new PartnerModel
                    {
                        PartnerId = model.ReceiverId,
                        FullName = model.OwnerName
                    };
                    IsRequiredVisible = false;
                }
                IsSaveDraftVisible = true;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public void Destroy()
        {
            _gpsListener.OnReadingReceived -= OnReadingReceived;
        }

        #endregion
    }
}
