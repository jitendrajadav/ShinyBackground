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

namespace KegID.ViewModel
{
    public class MoveViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private readonly IMoveService _moveService;
        private readonly IManifestManager _manifestManager;
        private readonly IUuidManager _uuidManager;

        public string Contents { get; set; }
        public IList<BarcodeModel> Barcodes { get; set; }

        #region ManifestId

        /// <summary>
        /// The <see cref="ManifestId" /> property's name.
        /// </summary>
        public const string ManifestIdPropertyName = "ManifestId";

        private string _ManifestId = default;

        /// <summary>
        /// Sets and gets the ManifestId property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ManifestId
        {
            get
            {
                return _ManifestId;
            }

            set
            {
                if (_ManifestId == value)
                {
                    return;
                }

                _ManifestId = value;
                ConstantManager.ManifestId = value;
                RaisePropertyChanged(ManifestIdPropertyName);
            }
        }

        #endregion

        #region Destination

        /// <summary>
        /// The <see cref="Destination" /> property's name.
        /// </summary>
        public const string DestinationPropertyName = "Destination";

        private string _Destination = "Select a location";

        /// <summary>
        /// Sets and gets the Destination property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Destination
        {
            get
            {
                return _Destination;
            }

            set
            {
                if (_Destination == value)
                {
                    return;
                }

                _Destination = value;
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
                IsSubmitVisible = _AddKegs.Contains("Item") ? true : false;
                RaisePropertyChanged(AddKegsPropertyName);
            }
        }

        #endregion

        #region IsSaveDraftVisible

        /// <summary>
        /// The <see cref="IsSaveDraftVisible" /> property's name.
        /// </summary>
        public const string IsSaveDraftVisiblePropertyName = "IsSaveDraftVisible";

        private bool _IsSaveDraftVisible = false;

        /// <summary>
        /// Sets and gets the IsSaveDraftVisible property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsSaveDraftVisible
        {
            get
            {
                return _IsSaveDraftVisible;
            }

            set
            {
                if (_IsSaveDraftVisible == value)
                {
                    return;
                }

                _IsSaveDraftVisible = value;
                RaisePropertyChanged(IsSaveDraftVisiblePropertyName);
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
                IsSaveDraftVisible = _IsSubmitVisible;
                RaisePropertyChanged(IsSubmitVisiblePropertyName);
            }
        }

        #endregion

        #region IsRequiredVisible

        /// <summary>
        /// The <see cref="IsRequiredVisible" /> property's name.
        /// </summary>
        public const string IsRequiredVisiblePropertyName = "IsRequiredVisible";

        private bool _IsRequiredVisible = true;

        /// <summary>
        /// Sets and gets the IsRequiredVisible property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsRequiredVisible
        {
            get
            {
                return _IsRequiredVisible;
            }

            set
            {
                if (_IsRequiredVisible == value)
                {
                    return;
                }

                _IsRequiredVisible = value;
                RaisePropertyChanged(IsRequiredVisiblePropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand SelectLocationCommand { get; }
        public DelegateCommand MoreInfoCommand { get; }
        public DelegateCommand ScanKegsCommad { get;}
        public DelegateCommand SaveDraftCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand SubmitCommand { get; }

        #endregion

        #region Constructor

        public MoveViewModel(IMoveService moveService, INavigationService navigationService, IPageDialogService dialogService, IManifestManager manifestManager, IUuidManager uuidManager)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
            _dialogService = dialogService;
            _moveService = moveService;
            _manifestManager = manifestManager;
            _uuidManager = uuidManager;

            SelectLocationCommand = new DelegateCommand(SelectLocationCommandRecieverAsync);
            MoreInfoCommand = new DelegateCommand(MoreInfoCommandRecieverAsync);
            ScanKegsCommad = new DelegateCommand(ScanKegsCommadRecieverAsync);
            SaveDraftCommand = new DelegateCommand(SaveDraftCommandRecieverAsync);
            CancelCommand = new DelegateCommand(CancelCommandRecieverAsync);
            SubmitCommand = new DelegateCommand(SubmitCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void SubmitCommandRecieverAsync()
        {
            ManifestModel manifestPostModel = null;
            try
            {
                Loader.StartLoading();

                manifestPostModel = GenerateManifest();
                if (manifestPostModel != null)
                {
                    try
                    {
                        var result = await _moveService.PostManifestAsync(manifestPostModel, AppSettings.SessionId, Configuration.NewManifest);
                        try
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
                                    var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                                    await RealmDb.WriteAsync((realmDb) =>
                                    {
                                        realmDb.Add(manifestPostModel);
                                    });
                                }
                                catch (Exception ex)
                                {
                                    Crashes.TrackError(ex);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }

                        var manifest = await _moveService.GetManifestAsync(AppSettings.SessionId, result.ManifestId);
                        if (manifest.Response.StatusCode == System.Net.HttpStatusCode.OK.ToString())
                        {
                            Loader.StopLoading();
                            await _navigationService.NavigateAsync(new Uri("ManifestDetailView", UriKind.Relative), new NavigationParameters
                            {
                                { "manifest", manifest },{ "Contents", Contents }
                            }, useModalNavigation: true, animated: false);
                        }
                        else
                        {
                            Loader.StopLoading();
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
            finally
            {
                Loader.StopLoading();
            }
        }

        private async void SaveDraftCommandRecieverAsync()
        {
            ManifestModel manifestModel = null;
            
            try
            {
                manifestModel = GenerateManifest();
                if (manifestModel != null)
                {
                    Loader.StartLoading();
                    manifestModel.IsDraft = true;
                    var isNew = Realm.GetInstance(RealmDbManager.GetRealmDbConfig()).Find<ManifestModel>(manifestModel.ManifestId);
                    if (isNew != null)
                    {
                        try
                        {
                            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                            RealmDb.Write(() =>
                            {
                                RealmDb.Add(manifestModel, update: true);
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
                            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                            await RealmDb.WriteAsync((realmDb) =>
                            {
                                realmDb.Add(manifestModel);
                            });
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }
                    }

                    Loader.StopLoading();
                    await _navigationService.NavigateAsync(new Uri("ManifestsView", UriKind.Relative), new NavigationParameters
                    {
                        { "LoadDraftManifestAsync", "LoadDraftManifestAsync" }
                    }, useModalNavigation: true, animated: false);
                }
                else
                {
                    await _dialogService.DisplayAlertAsync("Error", "Could not save manifest.", "Ok");
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                Loader.StopLoading();
                manifestModel = null;
                Cleanup();
            }
        }

        public ManifestModel GenerateManifest()
        {
            return _manifestManager.GetManifestDraft(eventTypeEnum: EventTypeEnum.MOVE_MANIFEST, manifestId: ManifestId,
                        barcodeCollection: ConstantManager.Barcodes ?? new List<BarcodeModel>(), tags: Tags ?? new List<Tag>(), tagsStr: TagsStr, 
                        partnerModel: ConstantManager.Partner, newPallets: new List<NewPallet>(), batches: new List<NewBatch>(), 
                        closedBatches: new List<string>(), validationStatus: 2, contents: Contents);
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
                if (!IsSubmitVisible)
                    IsSubmitVisible = true;
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
                    await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
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
            await _navigationService.NavigateAsync(new Uri("PartnersView", UriKind.Relative), useModalNavigation: true, animated: false);
        }

        private async void MoreInfoCommandRecieverAsync()
        {
            await _navigationService.NavigateAsync(new Uri("AddTagsView", UriKind.Relative), new NavigationParameters
                    {
                        {"viewTypeEnum",ViewTypeEnum.MoveView }
                    }, useModalNavigation: true, animated: false);
        }

        private async void ScanKegsCommadRecieverAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(ConstantManager.Partner?.PartnerId))
                {
                    if (Barcodes != null)
                    {
                        await _navigationService.NavigateAsync(new Uri("ScanKegsView", UriKind.Relative), new NavigationParameters
                        {
                            { "models", Barcodes }
                        }, useModalNavigation: true, animated: false);
                    }
                    else
                        await _navigationService.NavigateAsync(new Uri("ScanKegsView", UriKind.Relative), useModalNavigation: true, animated: false);
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

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (ConstantManager.Barcodes != null && ConstantManager.Barcodes.Count > 0)
                AssingScanKegsValue(ConstantManager.Barcodes.ToList(), ConstantManager.Tags, ConstantManager.Contents);

            switch (parameters.Keys.FirstOrDefault())
            {
                case "ManifestId":
                    ManifestId = parameters.GetValue<string>("ManifestId");
                    break;
                case "model":
                    Destination = ConstantManager.Partner.FullName;
                    IsRequiredVisible = false;
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
        }

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
