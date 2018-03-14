using GalaSoft.MvvmLight;
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
using System.Linq;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class MoveViewModel : ViewModelBase
    {
        #region Properties

        public IMoveService _moveService { get; set; }

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
                        var result = await _moveService.PostManifestAsync(manifestPostModel, Configuration.SessionId, Configuration.NewManifest);

                        var manifest = await _moveService.GetManifestAsync(Configuration.SessionId, result.ManifestId);
                        if (manifest.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().TrackingNumber = manifest.TrackingNumber;

                            SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().ManifestTo = manifest.CreatorCompany.FullName + "\n" + manifest.CreatorCompany.PartnerTypeName;

                            SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().ShippingDate =  Convert.ToDateTime(manifest.ShipDate);
                            SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().ItemCount = manifest.ManifestItems.Count;
                            SimpleIoc.Default.GetInstance<ContentTagsViewModel>().ContentCollection = manifest.ManifestItems.Select(x=>x.Barcode).ToList();
                            
                            SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().Contents = !string.IsNullOrEmpty(manifest.ManifestItems.FirstOrDefault().Contents)? manifest.ManifestItems.FirstOrDefault().Contents :"No contens";

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

        //public async Task<ManifestModel> GetManifestDraft(EventTypeEnum eventTypeEnum)
        //{
        //    try
        //    {
        //        ManifestModel manifestModel = null;
        //        ValidateBarcodeModel validateBarcodeModel = null;
        //        List<ManifestItem> manifestItemlst = new List<ManifestItem>();
        //        ManifestItem manifestItem = null;
        //        IList<Barcode> barcodeCollection = null;
        //        IList<Tag> tags = null;
        //        string contents = string.Empty;
        //        PartnerModel partnerModel = null;

        //        switch ((ViewTypeEnum)Enum.Parse(typeof(ViewTypeEnum), Application.Current.MainPage.Navigation.ModalStack.LastOrDefault().GetType().Name))
        //        {
        //            case ViewTypeEnum.ScanKegsView:
        //                barcodeCollection = SimpleIoc.Default.GetInstance<ScanKegsViewModel>().BarcodeCollection;
        //                tags = SimpleIoc.Default.GetInstance<ScanKegsViewModel>().Tags;
        //                contents = SimpleIoc.Default.GetInstance<ScanKegsViewModel>().SelectedBrand.BrandName;
        //                partnerModel = PartnerModel;
        //                break;
        //            case ViewTypeEnum.AddPalletsView:
        //                barcodeCollection = SimpleIoc.Default.GetInstance<FillScanViewModel>().BarcodeCollection;
        //                tags = SimpleIoc.Default.GetInstance<FillScanViewModel>().Tags;
        //                partnerModel = SimpleIoc.Default.GetInstance<FillViewModel>().PartnerModel;
        //                break;
        //        }

        //        foreach (var item in barcodeCollection)
        //        {
        //            string barcodeId = item.Id;
        //            var barcodeResult = await SQLiteServiceClient.Db.Table<BarcodeModel>().Where(x => x.Barcode == barcodeId).FirstOrDefaultAsync();
        //            validateBarcodeModel = JsonConvert.DeserializeObject<ValidateBarcodeModel>(barcodeResult.BarcodeJson);

        //            manifestItem = new ManifestItem()
        //            {
        //                Barcode = barcodeResult.Barcode,
        //                ScanDate = DateTime.Today,
        //                ValidationStatus = 2,
        //                KegId = validateBarcodeModel.Kegs.Partners.FirstOrDefault().Kegs.FirstOrDefault().KegId,
        //                Tags = tags.ToList(),
        //                KegStatus = new List<KegStatus>()
        //                {
        //                    new KegStatus()
        //                    {
        //                        KegId= validateBarcodeModel.Kegs.Partners.FirstOrDefault().Kegs.FirstOrDefault().KegId,
        //                        Barcode=barcodeResult.Barcode,
        //                        AltBarcode=validateBarcodeModel.Kegs.Partners.FirstOrDefault().Kegs.FirstOrDefault().AltBarcode,
        //                        Contents = contents,
        //                        Batch =validateBarcodeModel.Kegs.Partners.FirstOrDefault().Kegs.FirstOrDefault().Batch.ToString(),
        //                        Size = tags.Any(x=>x.Property == "Size") ? tags.Where(x=>x.Property == "Size").Select(x=>x.Value).FirstOrDefault():string.Empty,
        //                        Alert = validateBarcodeModel.Kegs.Partners.FirstOrDefault().Kegs.FirstOrDefault().Alert,
        //                        Location = validateBarcodeModel.Kegs.Locations.FirstOrDefault(),
        //                        OwnerName = partnerModel.FullName,
        //                    }
        //                },
        //            };
        //            manifestItemlst.Add(manifestItem);
        //            barcodeId = string.Empty;
        //        }

        //        var tempManifestId = string.Empty;

        //        switch ((ViewTypeEnum)Enum.Parse(typeof(ViewTypeEnum), Application.Current.MainPage.Navigation.ModalStack.LastOrDefault().GetType().Name))
        //        {
        //            case ViewTypeEnum.ScanKegsView:
        //                tempManifestId = ManifestId;
        //                break;
        //            case ViewTypeEnum.AddPalletsView:
        //                tempManifestId = SimpleIoc.Default.GetInstance<FillScanViewModel>().ManifestId;
        //                break;
        //        }

        //        manifestModel = new ManifestModel()
        //        {
        //            ManifestId = tempManifestId,
        //            EventTypeId = (long)eventTypeEnum,
        //            Latitude = (long)Geolocation.savedPosition.Latitude,
        //            Longitude = (long)Geolocation.savedPosition.Longitude,
        //            SubmittedDate = DateTime.Today,
        //            ShipDate = DateTime.Today,

        //            SenderId = Configuration.CompanyId,
        //            ReceiverId = partnerModel.PartnerId,
        //            DestinationName = partnerModel.FullName,
        //            DestinationTypeCode = partnerModel.LocationCode,

        //            ManifestItems = manifestItemlst,
        //            NewPallets = new List<string>(),
        //            Tags = tags.ToList()
        //        };

        //        return manifestModel;
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //        return null;
        //    }
        //}

        private async void SaveDraftCommandRecieverAsync()
        {
            try
            {
                Loader.StartLoading();

                #region Old Code
                //DraftManifestModel manifestModel = new DraftManifestModel()
                //{
                //    ClosedBatches = 0,
                //    DestinationId = Destination.FullName,
                //    EffectiveDate = DateTime.Today,
                //    EventTypeId = 0,
                //    GS1GSIN = "",
                //    IsSendManifest = true,
                //    KegOrderId = "",
                //    Latitude = savedPosition.Latitude,
                //    Longitude = savedPosition.Longitude,
                //    NewBatch = "",
                //    NewBatches = 0,
                //    NewPallets = 0,
                //    OriginId = Destination.Address,
                //    PostedDate = DateTime.Today,
                //    SourceKey = "",
                //    SubmittedDate = DateTime.Today,
                //    Tags = TagsStr,
                //    ManifestItems = Convert.ToInt64(AddKegs.Split(' ').FirstOrDefault()),
                //    Id = 0,
                //    ManifestId = MenifestId.Split(':').LastOrDefault().Trim(),
                //    ReceiverId = Destination.FullName,
                //    SenderId = Destination.Address,
                //    ShipDate = DateTime.Today,
                //};
                //await SQLiteServiceClient.Db.InsertAsync(manifestModel); 
                #endregion

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
                //try
                //{
                //    await SQLiteServiceClient.Db.InsertAsync(manifestPostModel);
                //}
                //catch (Exception ex)
                //{
                //    Debug.WriteLine(ex.Message);
                //}

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
                await Application.Current.MainPage.Navigation.PushModalAsync(new ScanKegsView());
            else
                await Application.Current.MainPage.DisplayAlert("Error", "Please select a destination first.", "Ok");
        }

        #region Location Services

        #endregion

        #endregion
    }
}
