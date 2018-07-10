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

namespace KegID.ViewModel
{
    public class MoveViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private readonly IMoveService _moveService;
        private readonly IManifestManager _manifestManager;

        public string Contents { get; set; }

        #region ManifestId

        /// <summary>
        /// The <see cref="ManifestId" /> property's name.
        /// </summary>
        public const string ManifestIdPropertyName = "ManifestId";

        private string _ManifestId = default(string);

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

        public MoveViewModel(IMoveService moveService, INavigationService navigationService, IPageDialogService dialogService, IManifestManager manifestManager)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
            _dialogService = dialogService;
            _moveService = moveService;
            _manifestManager = manifestManager;

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

                manifestPostModel = await GenerateManifest();
                if (manifestPostModel != null)
                {
                    try
                    {
                        var result = await _moveService.PostManifestAsync(manifestPostModel, AppSettings.User.SessionId, Configuration.NewManifest);
                        var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());

                        try
                        {
                            RealmDb.Write(() =>
                                               {
                                                   RealmDb.Add(manifestPostModel);
                                               });
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }

                        var manifest = await _moveService.GetManifestAsync(AppSettings.User.SessionId, result.ManifestId);
                        if (manifest.Response.StatusCode == System.Net.HttpStatusCode.OK.ToString())
                        {
                            Loader.StopLoading();
                            var param = new NavigationParameters
                            {
                                { "manifest", manifest },{ "Contents", Contents }
                            };
                            await _navigationService.NavigateAsync(new Uri("ManifestDetailView", UriKind.Relative), param, useModalNavigation: true, animated: false);
                        }
                        else
                        {
                            Loader.StopLoading();
                            //@default.GetInstance<LoginViewModel>().InvalideServiceCallAsync();
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
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            try
            {
                Loader.StartLoading();

                manifestModel = await GenerateManifest();

                manifestModel.IsDraft = true;

                try
                {
                    RealmDb.Write(() =>
                    {
                        var Result = RealmDb.Add(manifestModel, true);
                    });
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }

                Loader.StopLoading();
                //await Application.Current.MainPage.Navigation.PushModalAsync(new ManifestsView(), animated: false);
                //SimpleIoc.Default.GetInstance<ManifestsViewModel>().LoadDraftManifestAsync();
                var param = new NavigationParameters
                    {
                        { "LoadDraftManifestAsync", "LoadDraftManifestAsync" }
                    };
                await _navigationService.NavigateAsync(new Uri("ManifestsView", UriKind.Relative), param, useModalNavigation: true, animated: false);

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

        public async Task<ManifestModel> GenerateManifest()
        {
            return await _manifestManager.GetManifestDraft(eventTypeEnum: EventTypeEnum.MOVE_MANIFEST, manifestId: ManifestId,
                                barcodeCollection: ConstantManager.Barcodes, tags: Tags??new List<Tag>(), partnerModel: ConstantManager.Partner, newPallets: new List<NewPallet>(), 
                                batches: new List<NewBatch>(), closedBatches: new List<string>(), validationStatus: 2, contents: Contents);
        }

        internal void AssingScanKegsValue(List<BarcodeModel> _barcodes, List<Tag> _tags,string _contents)
        {
            try
            {
                Contents = _contents;
                Tags = _tags;
                ConstantManager.Barcodes = _barcodes;
                if (ConstantManager.Barcodes.Count > 1)
                    AddKegs = string.Format("{0} Items", ConstantManager.Barcodes.Count);
                else if (ConstantManager.Barcodes.Count == 1)
                    AddKegs = string.Format("{0} Item", ConstantManager.Barcodes.Count);
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
                var result = await _dialogService.DisplayActionSheetAsync("Cancel? \n Would you like to save this manifest as a draft or delete?",null,null, "Delete manifest", "Save as draft");
                if (result == "Delete manifest")
                {
                    await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
                }
                else
                {
                    //Save Draft Logic here...
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
            var param = new NavigationParameters
                    {
                        {"viewTypeEnum",ViewTypeEnum.MoveView }
                    };
            await _navigationService.NavigateAsync(new Uri("AddTagsView", UriKind.Relative), param, useModalNavigation: true, animated: false);
        }

        private async void ScanKegsCommadRecieverAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(ConstantManager.Partner?.PartnerId))
                {
                    var param = new NavigationParameters
                    {
                        { "Barcode", ConstantManager.Barcode }
                    };
                    await _navigationService.NavigateAsync(new Uri("ScanKegsView", UriKind.Relative), param, useModalNavigation: true, animated: false);

                }
                else
                    await _dialogService.DisplayAlertAsync("Error", "Please select a destination first.", "Ok");
            }
            catch (Exception ex)
            {
                 Crashes.TrackError(ex);
            }
        }

        internal void AssignInitialValue(string _kegId, string _barcode, string _addKegs, string _destination,string _partnerId, bool isSaveDraftVisible)
        {
            try
            {
                ConstantManager.Barcode = _barcode;
                ManifestId = !string.IsNullOrEmpty(_kegId) ? _kegId : Uuid.GetUuId();
                AddKegs = !string.IsNullOrEmpty(_addKegs) ? string.Format("{0} Item", _addKegs) : "Add Kegs";
                if (!string.IsNullOrEmpty(_destination))
                {
                    Destination = _destination;
                    ConstantManager.Partner = new PartnerModel
                    {
                        PartnerId = _partnerId
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
                //Barcode = string.Empty;
                AddKegs = "Add Kegs";
                TagsStr = "Add info";
                try
                {
                    //PartnerModel = null;
                }
                catch (Exception ex)
                {
                     Crashes.TrackError(ex);
                }
                IsSaveDraftVisible = false;
                IsSubmitVisible = false;
                IsRequiredVisible = true;
                Destination = "Select a location";
                ManifestId = Uuid.GetUuId();
                Tags = null;
            }
            catch (Exception ex)
            {
                 Crashes.TrackError(ex);
            }

            //base.Cleanup();
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (ConstantManager.Barcodes != null)
                AssingScanKegsValue(ConstantManager.Barcodes.ToList(), ConstantManager.Tags, ConstantManager.Contents);

            if (parameters.ContainsKey("ManifestId"))
            {
                ManifestId = parameters.GetValue<string>("ManifestId");
            }
            if (parameters.ContainsKey("model"))
            {
                Destination = ConstantManager.Partner.FullName;
                IsRequiredVisible = false;
                IsSaveDraftVisible = true;
            }
            if (parameters.ContainsKey("AddTags"))
            {
                AssignAddTagsValue(ConstantManager.Tags, ConstantManager.TagsStr);
            }
            if (parameters.ContainsKey("AssignInitialValue"))
            {
                var model = parameters.GetValue<ManifestModel>("AssignInitialValue");
                AssignInitialValue(model.ManifestId, model.ManifestItems.Count > 0 ? model.ManifestItems.FirstOrDefault().Barcode : string.Empty, model.ManifestItemsCount > 0 ? model.ManifestItemsCount.ToString() : string.Empty, model.OwnerName, model.ReceiverId, true);
            }
            if (parameters.ContainsKey("AssignInitialValueFromKegStatus"))
            {
                var Barcode = parameters.GetValue<string>("AssignInitialValueFromKegStatus");
                var KegId = parameters.GetValue<string>("KegId");
                AssignInitialValue(KegId, Barcode, "1", string.Empty, string.Empty, true);
            }
        }

        #endregion
    }
}
