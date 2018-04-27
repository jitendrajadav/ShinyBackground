using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using KegID.SQLiteClient;
using KegID.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class MoveViewModel : BaseViewModel
    {
        #region Properties

        public IMoveService _moveService { get; set; }
        public string Barcode { get; set; }
        public IList<Barcode> Barcodes
        {
            get;

            set;
        }
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

        #region PartnerModel

        /// <summary>
        /// The <see cref="PartnerModel" /> property's name.
        /// </summary>
        public const string PartnerModelPropertyName = "PartnerModel";

        private PartnerModel _PartnerModel = new PartnerModel();

        /// <summary>
        /// Sets and gets the PartnerModel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public PartnerModel PartnerModel
        {
            get
            {
                return _PartnerModel;
            }

            set
            {
                if (_PartnerModel == value)
                {
                    return;
                }

                _PartnerModel = value;
                Destination = _PartnerModel.FullName;
                IsRequiredVisible = false;
                IsSaveDraftVisible = true;
                RaisePropertyChanged(PartnerModelPropertyName);
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

        public RelayCommand SelectLocationCommand { get; }
        public RelayCommand MoreInfoCommand { get; }
        public RelayCommand ScanKegsCommad { get;}
        public RelayCommand SaveDraftCommand { get; }
        public RelayCommand CancelCommand { get; }
        public RelayCommand SubmitCommand { get; }

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
            SubmitCommand = new RelayCommand(SubmitCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void SubmitCommandRecieverAsync()
        {
            ManifestModel manifestPostModel = null;
            SimpleIoc @default = SimpleIoc.Default;

            try
            {
                Loader.StartLoading();

                manifestPostModel = await ManifestManager.GetManifestDraft(eventTypeEnum: EventTypeEnum.MOVE_MANIFEST, manifestId: ManifestId,
                    barcodeCollection: Barcodes, tags: Tags, partnerModel: PartnerModel, newPallets: new List<NewPallet>(), 
                    batches: new List<NewBatch>(), closedBatches: new List<string>(), validationStatus: 2 ,contents: Contents);
                
                if (manifestPostModel != null)
                {
                    try
                    {
                        var result = await _moveService.PostManifestAsync(manifestPostModel, AppSettings.User.SessionId, Configuration.NewManifest);

                        var manifest = await _moveService.GetManifestAsync(AppSettings.User.SessionId, result.ManifestId);
                        if (manifest.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            @default.GetInstance<ManifestDetailViewModel>().AssignInitialValue(manifest);
                            Loader.StopLoading();
                            await Application.Current.MainPage.Navigation.PushModalAsync(new ManifestDetailView());
                        }
                        else
                        {
                            Loader.StopLoading();
                            @default.GetInstance<LoginViewModel>().InvalideServiceCallAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                    finally
                    {
                        manifestPostModel = null;
                        @default = null;
                        Cleanup();
                    }
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Alert","Something goes wrong please check again","Ok");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                Loader.StopLoading();
            }
        }

        private async void SaveDraftCommandRecieverAsync()
        {
            ManifestModel manifestPostModel = null;
            DraftManifestModel draftManifestModel = null;
            SimpleIoc @default = SimpleIoc.Default;

            try
            {
                Loader.StartLoading();
                
                manifestPostModel = await ManifestManager.GetManifestDraft(eventTypeEnum: EventTypeEnum.MOVE_MANIFEST, manifestId: ManifestId,
                    barcodeCollection: @default.GetInstance<ScanKegsViewModel>().BarcodeCollection, tags: @default.GetInstance<ScanKegsViewModel>().Tags,
                    partnerModel: PartnerModel, newPallets: new List<NewPallet>(), batches: new List<NewBatch>(), closedBatches: new List<string>(),
                    validationStatus: 2, contents: @default.GetInstance<ScanKegsViewModel>().SelectedBrand?.BrandName);

                draftManifestModel = new DraftManifestModel()
                {
                    ManifestId = ManifestId,
                    DraftManifestJson = JsonConvert.SerializeObject(manifestPostModel)
                };

                try
                {
                    var Result = await SQLiteServiceClient.Db.InsertAsync(draftManifestModel);
                    if (Result > 0)
                        @default.GetInstance<DashboardViewModel>().CheckDraftmaniFestsAsync();
                }
                catch (Exception ex)
                {
                    var Result = await SQLiteServiceClient.Db.UpdateAsync(draftManifestModel);
                    Debug.WriteLine(ex.Message);
                }

                Loader.StopLoading();
                await Application.Current.MainPage.Navigation.PushModalAsync(new ManifestsView());
                await @default.GetInstance<ManifestsViewModel>().LoadDraftManifestAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                Loader.StopLoading();
                manifestPostModel = null;
                draftManifestModel = null;
                @default = null;
                Cleanup();
            }
        }

        internal void AssingScanKegsValue(List<Barcode> _barcodes, List<Tag> _tags,string _contents)
        {
            try
            {
                Contents = _contents;
                Tags = _tags;
                Barcodes = _barcodes;
                if (Barcodes.Count > 1)
                    AddKegs = string.Format("{0} Items", Barcodes.Count);
                else if (Barcodes.Count == 1)
                    AddKegs = string.Format("{0} Item", Barcodes.Count);
                if (!IsSubmitVisible)
                    IsSubmitVisible = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        internal void AssignAddTagsValue(List<Tag> _tags, string _tagsStr)
        {
            Tags = _tags;
            TagsStr = _tagsStr;
        }

        private async void CancelCommandRecieverAsync()
        {
            try
            {
                var result = await Application.Current.MainPage.DisplayActionSheet("Cancel? \n You have like to save this manifest as a draft or delete?", null, null, "Delete manifest", "Save as draft");
                if (result == "Delete manifest")
                {
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async void SelectLocationCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PushModalAsync(new PartnersView());

        private async void MoreInfoCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PushModalAsync(new AddTagsView());

        private async void ScanKegsCommadRecieverAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(PartnerModel?.PartnerId))
                {
                    await Application.Current.MainPage.Navigation.PushModalAsync(new ScanKegsView());
                    SimpleIoc.Default.GetInstance<ScanKegsViewModel>().AssignInitialValue(Barcode);
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Error", "Please select a destination first.", "Ok");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        internal void AssignInitialValue(string _kegId, string _barcode, string _addKegs, string _destination,string _partnerId, bool isSaveDraftVisible)
        {
            try
            {
                Barcode = _barcode;
                ManifestId = !string.IsNullOrEmpty(_kegId) ? _kegId : Uuid.GetUuId();
                AddKegs = !string.IsNullOrEmpty(_addKegs) ? string.Format("{0} Item", _addKegs) : "Add Kegs";
                if (!string.IsNullOrEmpty(_destination))
                {
                    Destination = _destination;
                    PartnerModel.PartnerId = _partnerId;
                    IsRequiredVisible = false;
                }
                IsSaveDraftVisible = isSaveDraftVisible;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public override void Cleanup()
        {
            try
            {
                Barcode = string.Empty;
                AddKegs = "Add Kegs";
                TagsStr = "Add info";
                try
                {
                    PartnerModel = null;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
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
                Debug.WriteLine(ex.Message);
            }

            base.Cleanup();
        }

        #endregion
    }
}
