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
            PartnerModel.FullName = "Select a location";
        }

        #endregion

        #region Methods

        private async void SubmitCommandRecieverAsync()
        {
            try
            {
                Loader.StartLoading();

                ManifestModel manifestPostModel = await ManifestManager.GetManifestDraft(EventTypeEnum.MOVE_MANIFEST, ManifestId, SimpleIoc.Default.GetInstance<ScanKegsViewModel>().BarcodeCollection, SimpleIoc.Default.GetInstance<ScanKegsViewModel>().Tags, PartnerModel, new List<NewPallet>(), new List<NewBatch>(), new List<string>(), 2 ,SimpleIoc.Default.GetInstance<ScanKegsViewModel>().SelectedBrand.BrandName);
                
                if (manifestPostModel != null)
                {
                    try
                    {
                        var result = await _moveService.PostManifestAsync(manifestPostModel, AppSettings.User.SessionId, Configuration.NewManifest);

                        var manifest = await _moveService.GetManifestAsync(AppSettings.User.SessionId, result.ManifestId);
                        if (manifest.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().AssignInitialValue(manifest);
                            Loader.StopLoading();
                            await Application.Current.MainPage.Navigation.PushModalAsync(new ManifestDetailView());
                        }
                        else
                        {
                            Loader.StopLoading();
                            SimpleIoc.Default.GetInstance<LoginViewModel>().InvalideServiceCallAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
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
            try
            {
                Loader.StartLoading();

                ManifestModel manifestPostModel =
                    await ManifestManager.GetManifestDraft(EventTypeEnum.MOVE_MANIFEST, ManifestId, SimpleIoc.Default.GetInstance<ScanKegsViewModel>().BarcodeCollection, SimpleIoc.Default.GetInstance<ScanKegsViewModel>().Tags, PartnerModel, new List<NewPallet>(), new List<NewBatch>(), new List<string>(), 2, SimpleIoc.Default.GetInstance<ScanKegsViewModel>().SelectedBrand.BrandName);
                DraftManifestModel draftManifestModel = new DraftManifestModel()
                {
                    ManifestId = ManifestId,
                    DraftManifestJson = JsonConvert.SerializeObject(manifestPostModel)
                };

                try
                {
                    await SQLiteServiceClient.Db.InsertAsync(draftManifestModel);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

                Loader.StopLoading();
                await Application.Current.MainPage.Navigation.PushModalAsync(new ManifestsView());
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

        private async void ScanKegsCommadRecieverAsync()
        {
            if (!string.IsNullOrEmpty(PartnerModel.PartnerId))
            {
                await Application.Current.MainPage.Navigation.PushModalAsync(new ScanKegsView());
                SimpleIoc.Default.GetInstance<ScanKegsViewModel>().AssignInitialValue(Barcode);
            }
            else
                await Application.Current.MainPage.DisplayAlert("Error", "Please select a destination first.", "Ok");
        }

        internal void AssignInitialValue(string _kegId, string _barcode)
        {
            Barcode = _barcode;
            ManifestId = _kegId;
            AddKegs = string.Format("{0} Item", 1);
        }

        #endregion
    }
}
