using KegID.Common;
using KegID.LocalDb;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace KegID.ViewModel
{
    public class MoveViewModel : BaseViewModel
    {
        #region Properties

        private readonly IPageDialogService _dialogService;
        private readonly IMoveService _moveService;
        private readonly IManifestManager _manifestManager;
        private readonly IUuidManager _uuidManager;
        private readonly IGeolocationService _geolocationService;

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
        public string AddKegs { get; set; } = "Add Kegs";
        public void OnAddKegsChanged()
        {
            IsSubmitVisible = AddKegs.Contains("Item") ? true : false;
        }
        public bool OriginRequired { get; set; }
        public bool IsOriginRequired { get; set; } = true;
        public bool IsSaveDraftVisible { get; set; }
        public string Order { get; set; }
        public bool IsSubmitVisible { get; set; }
        public void OnIsSubmitVisibleChanged()
        {
            IsSaveDraftVisible = IsSubmitVisible;
        }

        public bool IsRequiredVisible { get; set; } = true;
        public bool OrderNumRequired { get; set; }
        public string Origin { get; set; } = "Select a location";
        #endregion

        #region Commands

        public DelegateCommand SelectLocationCommand { get; }
        public DelegateCommand SelectOriginLocationCommand { get; }
        public DelegateCommand MoreInfoCommand { get; }
        public DelegateCommand ScanKegsCommad { get;}
        public DelegateCommand SaveDraftCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand SubmitCommand { get; }
        public object RealmDb { get; }

        #endregion

        #region Constructor

        public MoveViewModel(IMoveService moveService, INavigationService navigationService, IPageDialogService dialogService, IManifestManager manifestManager, IUuidManager uuidManager, IGeolocationService geolocationService) : base(navigationService)
        {
            _dialogService = dialogService;
            _moveService = moveService;
            _manifestManager = manifestManager;
            _uuidManager = uuidManager;
            _geolocationService = geolocationService;

            SelectLocationCommand = new DelegateCommand(SelectLocationCommandRecieverAsync);
            SelectOriginLocationCommand = new DelegateCommand(SelectOriginLocationCommandRecieverAsync);
            MoreInfoCommand = new DelegateCommand(MoreInfoCommandRecieverAsync);
            ScanKegsCommad = new DelegateCommand(ScanKegsCommadRecieverAsync);
            SaveDraftCommand = new DelegateCommand(SaveDraftCommandRecieverAsync);
            CancelCommand = new DelegateCommand(CancelCommandRecieverAsync);
            SubmitCommand = new DelegateCommand(SubmitCommandRecieverAsync);

            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            var preference = RealmDb.All<Preference>().Where(x => x.PreferenceName == "OrderNumRequired").FirstOrDefault();
            OrderNumRequired = preference != null ? bool.Parse(preference.PreferenceValue) : false;

            var RealmDb1 = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            var preference1 = RealmDb1.All<Preference>().Where(x => x.PreferenceName == "OriginRequired").FirstOrDefault();
            OriginRequired = preference1 != null ? bool.Parse(preference1.PreferenceValue) : false;
        }

        #endregion

        #region Methods

        private async void SubmitCommandRecieverAsync()
        {
            try
            {
                Loader.StartLoading();
                var location = await _geolocationService.GetLastLocationAsync();

                ManifestModel manifestPostModel = null;
                try
                {
                    manifestPostModel = GenerateManifest(location??new Xamarin.Essentials.Location(0,0));
                    if (manifestPostModel != null)
                    {
                        try
                        {
                            var current = Connectivity.NetworkAccess;
                            if (current == NetworkAccess.Internet)
                            {
                                var result = await _moveService.PostManifestAsync(manifestPostModel, AppSettings.SessionId, Configuration.NewManifest);

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
                Loader.StopLoading();
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
                manifest.CreatorCompany = new CreatorCompany { Address = ConstantManager.Partner.Address, State = ConstantManager.Partner.State, PostalCode = ConstantManager.Partner.PostalCode, Lon = ConstantManager.Partner.Lon, Address1 = ConstantManager.Partner.Address1, City = ConstantManager.Partner.City, CompanyNo = ConstantManager.Partner.CompanyNo.HasValue ? ConstantManager.Partner.CompanyNo.Value.ToString() : string.Empty, Country = ConstantManager.Partner.Country, FullName = ConstantManager.Partner.FullName, IsActive = ConstantManager.Partner.IsActive, IsInternal = ConstantManager.Partner.IsInternal, IsShared = ConstantManager.Partner.IsShared, Lat = ConstantManager.Partner.Lat, LocationCode = ConstantManager.Partner.LocationCode, LocationStatus = ConstantManager.Partner.LocationStatus, MasterCompanyId = ConstantManager.Partner.MasterCompanyId, ParentPartnerId = ConstantManager.Partner.ParentPartnerId, ParentPartnerName = ConstantManager.Partner.ParentPartnerName, PartnerId = ConstantManager.Partner.PartnerId, PartnershipIsActive = ConstantManager.Partner.PartnershipIsActive, PartnerTypeCode = ConstantManager.Partner.PartnerTypeCode, PartnerTypeName = ConstantManager.Partner.PartnerTypeName, PhoneNumber = ConstantManager.Partner.PhoneNumber, SourceKey = ConstantManager.Partner.SourceKey };
                manifest.SenderPartner = new CreatorCompany { Address = ConstantManager.Partner.Address/*, State = ConstantManager.Partner.State, PostalCode = ConstantManager.Partner.PostalCode, Lon = ConstantManager.Partner.Lon, Address1 = ConstantManager.Partner.Address1, City = ConstantManager.Partner.City, CompanyNo = ConstantManager.Partner.CompanyNo.HasValue ? ConstantManager.Partner.CompanyNo.Value.ToString() : string.Empty, Country = ConstantManager.Partner.Country, FullName = ConstantManager.Partner.FullName, IsActive = ConstantManager.Partner.IsActive, IsInternal = ConstantManager.Partner.IsInternal, IsShared = ConstantManager.Partner.IsShared, Lat = ConstantManager.Partner.Lat, LocationCode = ConstantManager.Partner.LocationCode, LocationStatus = ConstantManager.Partner.LocationStatus, MasterCompanyId = ConstantManager.Partner.MasterCompanyId, ParentPartnerId = ConstantManager.Partner.ParentPartnerId, ParentPartnerName = ConstantManager.Partner.ParentPartnerName, PartnerId = ConstantManager.Partner.PartnerId, PartnershipIsActive = ConstantManager.Partner.PartnershipIsActive, PartnerTypeCode = ConstantManager.Partner.PartnerTypeCode, PartnerTypeName = ConstantManager.Partner.PartnerTypeName, PhoneNumber = ConstantManager.Partner.PhoneNumber, SourceKey = ConstantManager.Partner.SourceKey */};
                manifest.ReceiverPartner = new CreatorCompany { Address = ConstantManager.Partner.Address, State = ConstantManager.Partner.State, PostalCode = ConstantManager.Partner.PostalCode, Lon = ConstantManager.Partner.Lon, Address1 = ConstantManager.Partner.Address1, City = ConstantManager.Partner.City, CompanyNo = ConstantManager.Partner.CompanyNo.HasValue ? ConstantManager.Partner.CompanyNo.Value.ToString() : string.Empty, Country = ConstantManager.Partner.Country, FullName = ConstantManager.Partner.FullName, IsActive = ConstantManager.Partner.IsActive, IsInternal = ConstantManager.Partner.IsInternal, IsShared = ConstantManager.Partner.IsShared, Lat = ConstantManager.Partner.Lat, LocationCode = ConstantManager.Partner.LocationCode, LocationStatus = ConstantManager.Partner.LocationStatus, MasterCompanyId = ConstantManager.Partner.MasterCompanyId, ParentPartnerId = ConstantManager.Partner.ParentPartnerId, ParentPartnerName = ConstantManager.Partner.ParentPartnerName, PartnerId = ConstantManager.Partner.PartnerId, PartnershipIsActive = ConstantManager.Partner.PartnershipIsActive, PartnerTypeCode = ConstantManager.Partner.PartnerTypeCode, PartnerTypeName = ConstantManager.Partner.PartnerTypeName, PhoneNumber = ConstantManager.Partner.PhoneNumber, SourceKey = ConstantManager.Partner.SourceKey };
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
            var location = await _geolocationService.GetLastLocationAsync();

            ManifestModel manifestModel = null;

            manifestModel = GenerateManifest(location ?? new Xamarin.Essentials.Location(0, 0));
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

        public ManifestModel GenerateManifest(Xamarin.Essentials.Location location)
        {
            return _manifestManager.GetManifestDraft(eventTypeEnum: EventTypeEnum.MOVE_MANIFEST, manifestId: ManifestId,
                        barcodeCollection: ConstantManager.Barcodes ?? new List<BarcodeModel>(), (long)location.Latitude, (long)location.Longitude, tags: Tags ?? new List<Tag>(), tagsStr: TagsStr,
                        partnerModel: ConstantManager.Partner, newPallets: new List<NewPallet>(), batches: new List<NewBatch>(),
                        closedBatches: new List<string>(), null, validationStatus: 2, contents: Contents);
        }

        internal void AssingScanKegsValue(List<BarcodeModel> _barcodes, List<Tag> _tags,string _contents)
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
                    AddKegs = "Add Kegs";
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
                if (!string.IsNullOrEmpty(ConstantManager.Partner?.PartnerId))
                {

                    if (OrderNumRequired && string.IsNullOrEmpty(Order))
                    {
                        await _dialogService.DisplayAlertAsync("Error", "Please enter order first.", "Ok");
                    }
                    if (OriginRequired && Origin.Contains("Select a location"))
                    {
                        await _dialogService.DisplayAlertAsync("Error", "Please select a origin first.", "Ok");
                    }
                    else
                    {
                        if (Barcodes != null)
                        {
                            await _navigationService.NavigateAsync("ScanKegsView", new NavigationParameters
                            {
                                { "models", Barcodes }
                            }, animated: false);
                        }
                        else
                        {
                            await _navigationService.NavigateAsync("ScanKegsView", animated: false);
                        }
                    }
                }
                else
                    await _dialogService.DisplayAlertAsync("Error", "Please select a destination first.", "Ok");
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal void AssignInitialValue(string _kegId, IList<ManifestItem> _barcode, string _addKegs, string _destination,string _partnerId, bool isSaveDraftVisible, IList<Tag> tags,string tagsStr)
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
                AddKegs = !string.IsNullOrEmpty(_addKegs) ? Convert.ToUInt32(_addKegs) > 1 ? string.Format("{0} Items", _addKegs) : string.Format("{0} Item", _addKegs) : "Add Kegs";
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
                AddKegs = "Add Kegs";
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

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("CancelCommandRecieverAsync"))
            {
                CancelCommandRecieverAsync();
            }
        }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            if (ConstantManager.Barcodes != null)
                AssingScanKegsValue(ConstantManager.Barcodes.ToList(), ConstantManager.Tags, ConstantManager.Contents);

            switch (parameters.Keys.FirstOrDefault())
            {
                case "ManifestId":
                    ManifestId = parameters.GetValue<string>("ManifestId");
                    break;
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
                default:
                    break;
            }
            return base.InitializeAsync(parameters);
        }

        //public override void OnNavigatingTo(INavigationParameters parameters)
        //{
        //    if (ConstantManager.Barcodes != null)
        //        AssingScanKegsValue(ConstantManager.Barcodes.ToList(), ConstantManager.Tags, ConstantManager.Contents);

        //    switch (parameters.Keys.FirstOrDefault())
        //    {
        //        case "ManifestId":
        //            ManifestId = parameters.GetValue<string>("ManifestId");
        //            break;
        //        case "model":
        //            if (parameters.GetValue<string>("CommingFrom") == "MoveOrigin")
        //            {
        //                Origin = ConstantManager.Partner.FullName;
        //                IsOriginRequired = false;
        //            }
        //            else
        //            {
        //                Destination = ConstantManager.Partner.FullName;
        //                IsRequiredVisible = false;
        //            }
        //            IsSaveDraftVisible = true;
        //            break;
        //        case "AddTags":
        //            AssignAddTagsValue(ConstantManager.Tags, ConstantManager.TagsStr);
        //            break;
        //        case "AssignInitialValue":
        //            AssignInitialValue(parameters);
        //            break;
        //        case "AssignInitialValueFromKegStatus":
        //            AssignInitialValueFromKegStatus(parameters);
        //            break;
        //        case "PartnerModel":
        //            Destination = parameters.GetValue<PossessorLocation>("PartnerModel").FullName;
        //            break;
        //        default:
        //            break;
        //    }
        //}

        private void AssignInitialValueFromKegStatus(INavigationParameters parameters)
        {
            string Barcode = parameters.GetValue<string>("AssignInitialValueFromKegStatus");
            string KegId = parameters.GetValue<string>("KegId");
            List<ManifestItem> manifestItem = new List<ManifestItem>
                    {
                        new ManifestItem { Barcode = Barcode }
                    };
            AssignInitialValue(KegId, manifestItem, "1", string.Empty, string.Empty, true, null, string.Empty);
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
                AddKegs = !string.IsNullOrEmpty(model.ManifestItemsCount.ToString()) ? Convert.ToUInt32(model.ManifestItemsCount.ToString()) > 1 ? string.Format("{0} Items", model.ManifestItemsCount.ToString()) : string.Format("{0} Item", model.ManifestItemsCount.ToString()) : "Add Kegs";
                if (!string.IsNullOrEmpty(model.OwnerName))
                {
                    Destination = model.OwnerName;
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

        #endregion
    }
}
