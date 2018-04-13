using GalaSoft.MvvmLight.Command;
using KegID.Model;
using KegID.Services;
using KegID.Views;
using System.Linq;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Collections.Generic;
using System;
using KegID.SQLiteClient;
using System.Diagnostics;
using KegID.Common;
using GalaSoft.MvvmLight.Ioc;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;

namespace KegID.ViewModel
{
    public class ScanKegsViewModel : BaseViewModel
    {
        #region Properties

        public bool IsFromScanned { get; set; }
        public IMoveService _moveService { get; set; }

        #region BarcodeCollection

        /// <summary>
        /// The <see cref="BarcodeCollection" /> property's name.
        /// </summary>
        public const string BarcodeCollectionPropertyName = "BarcodeCollection";

        private ObservableCollection<Barcode> _BarcodeCollection = new ObservableCollection<Barcode>();

        /// <summary>
        /// Sets and gets the BarcodeCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<Barcode> BarcodeCollection
        {
            get
            {
                return _BarcodeCollection;
            }

            set
            {
                if (_BarcodeCollection == value)
                {
                    return;
                }

                _BarcodeCollection = value;
                RaisePropertyChanged(BarcodeCollectionPropertyName);
            }
        }

        #endregion

        #region BrandCollection

        /// <summary>
        /// The <see cref="BrandCollection" /> property's name.
        /// </summary>
        public const string BrandCollectionPropertyName = "BrandCollection";

        private IList<BrandModel> _BrandCollection = null;

        /// <summary>
        /// Sets and gets the BrandCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<BrandModel> BrandCollection
        {
            get
            {
                return _BrandCollection;
            }

            set
            {
                if (_BrandCollection == value)
                {
                    return;
                }

                _BrandCollection = value;
                RaisePropertyChanged(BrandCollectionPropertyName);
            }
        }

        #endregion

        #region SelectedBrand

        /// <summary>
        /// The <see cref="SelectedBrand" /> property's name.
        /// </summary>
        public const string SelectedBrandPropertyName = "SelectedBrand";

        private BrandModel _SelectedBrand = null;

        /// <summary>
        /// Sets and gets the SelectedBrand property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public BrandModel SelectedBrand
        {
            get
            {
                return _SelectedBrand;
            }

            set
            {
                if (_SelectedBrand == value)
                {
                    return;
                }

                _SelectedBrand = value;
                RaisePropertyChanged(SelectedBrandPropertyName);
            }
        }


        #endregion

        #region ManaulBarcode

        /// <summary>
        /// The <see cref="ManaulBarcode" /> property's name.
        /// </summary>
        public const string ManaulBarcodePropertyName = "ManaulBarcode";

        private string _ManaulBarcode = default(string);

        /// <summary>
        /// Sets and gets the ManaulBarcode property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ManaulBarcode
        {
            get
            {
                return _ManaulBarcode;
            }

            set
            {
                if (_ManaulBarcode == value)
                {
                    return;
                }

                _ManaulBarcode = value;
                RaisePropertyChanged(ManaulBarcodePropertyName);
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

        #region TagsStr

        /// <summary>
        /// The <see cref="TagsStr" /> property's name.
        /// </summary>
        public const string TagsStrPropertyName = "TagsStr";

        private string _tagsStr = default(string);

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

        #region Batch

        /// <summary>
        /// The <see cref="Batch" /> property's name.
        /// </summary>
        public const string BatchPropertyName = "Batch";

        private string _Batch = default(string);

        /// <summary>
        /// Sets and gets the Batch property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Batch
        {
            get
            {
                return _Batch;
            }

            set
            {
                if (_Batch == value)
                {
                    return;
                }

                _Batch = value;
                TagsStr = "Batch : " +_Batch;
                RaisePropertyChanged(BatchPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public RelayCommand DoneCommand { get; }
        public RelayCommand BarcodeScanCommand { get; }
        public RelayCommand BarcodeManualCommand { get; }
        public RelayCommand AddTagsCommand { get; }
        public RelayCommand<Barcode> IconItemTappedCommand { get; }
        public RelayCommand<Barcode> LabelItemTappedCommand { get; }

        #endregion

        #region Constructor

        public ScanKegsViewModel(IMoveService moveService)
        {
            _moveService = moveService;

            DoneCommand = new RelayCommand(DoneCommandRecieverAsync);
            BarcodeScanCommand = new RelayCommand(BarcodeScanCommandReciever);
            BarcodeManualCommand = new RelayCommand(BarcodeManualCommandRecieverAsync);
            AddTagsCommand = new RelayCommand(AddTagsCommandRecieverAsync);
            LabelItemTappedCommand = new RelayCommand<Barcode>((model) => LabelItemTappedCommandRecieverAsync(model));
            IconItemTappedCommand = new RelayCommand<Barcode>((model) => IconItemTappedCommandRecieverAsync(model));
            LoadBrand();
        }

        #endregion

        #region Methods

        public async void LoadBrand() => BrandCollection = await LoadBrandAsync();

        public async Task<IList<BrandModel>> LoadBrandAsync()
        {
            IList<BrandModel> model = await SQLiteServiceClient.Db.Table<BrandModel>().ToListAsync();
            try
            {
                if (model.Count > 0)
                    return model;
                else
                {
                    Loader.StartLoading();
                    var value = await _moveService.GetBrandListAsync(AppSettings.User.SessionId);

                    if (value.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        model = value.BrandModel;
                        await SQLiteServiceClient.Db.InsertAllAsync(model);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                Loader.StopLoading();
            }
            return model;
        }

        private async void LabelItemTappedCommandRecieverAsync(Barcode model)
        {

            if (model.PartnerCount > 1)
            {
                List<Barcode> modelList = new List<Barcode>
                    {
                        model
                    };
                await NavigateToValidatePartner(modelList);
            }
            else
            {
                IsFromScanned = true;
                await Application.Current.MainPage.Navigation.PushModalAsync(new AddTagsView());
            }
        }

        private async void IconItemTappedCommandRecieverAsync(Barcode model)
        {
            if (model.PartnerCount > 1)
            {
                List<Barcode> modelList = new List<Barcode>
                    {
                        model
                    };
                await NavigateToValidatePartner(modelList);
            }
            else
            {
                await Application.Current.MainPage.Navigation.PushModalAsync(new ScanInfoView());
                SimpleIoc.Default.GetInstance<ScanInfoViewModel>().AssignInitialValue(model.Id);
            }
        }

        private static async Task NavigateToValidatePartner(List<Barcode> model)
        {
            await Application.Current.MainPage.Navigation.PushPopupAsync(new ValidateBarcodeView());
            await SimpleIoc.Default.GetInstance<ValidateBarcodeViewModel>().LoadBarcodeValue(model);
        }

        private async void AddTagsCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new AddTagsView());
        }

        private async void DoneCommandRecieverAsync()
        {
            switch ((ViewTypeEnum)Enum.Parse(typeof(ViewTypeEnum), Application.Current.MainPage.Navigation.ModalStack[Application.Current.MainPage.Navigation.ModalStack.Count - 2].GetType().Name))
            {
                case ViewTypeEnum.MoveView:
                    SimpleIoc.Default.GetInstance<MoveViewModel>().AssingScanKegsValue(BarcodeCollection);
                    break;

                case ViewTypeEnum.PalletizeView:
                    SimpleIoc.Default.GetInstance<PalletizeViewModel>().AssingScanKegsValue(BarcodeCollection);
                    break;

                default:
                    break;
            }

            if (BarcodeCollection.Any(x=>x.PartnerCount > 1))
                await NavigateToValidatePartner(BarcodeCollection.Where(x => x.PartnerCount > 1).ToList());
            else
                await Application.Current.MainPage.Navigation.PopModalAsync();

            TagsStr = default(string);
        }

        internal void AssignInitialValue(string _barcode)
        {
            if (!string.IsNullOrEmpty(_barcode))
                BarcodeCollection.Add(new Barcode { Id = _barcode });
        }

        private async void BarcodeManualCommandRecieverAsync()
        {
            var value = await BarcodeScanner.ValidateBarcodeInsertIntoLocalDB(ManaulBarcode,_moveService);
            ManaulBarcode = string.Empty;
            BarcodeCollection = new ObservableCollection<Barcode>(value);
        }

        internal async void BarcodeScanCommandReciever()
        {
           var value = await BarcodeScanner.BarcodeScanAsync(_moveService);
            BarcodeCollection = new ObservableCollection<Barcode>(value);
        }

        internal void AssignValidatedValue(Partner model)
        {
            BarcodeCollection.Where(x => x.Id == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Icon = GetIconByPlatform.GetIcon("validationquestion.png");
            BarcodeCollection.Where(x => x.Id == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().PartnerCount = 1;
        }

        public override void Cleanup()
        {
            BarcodeCollection.Clear();
            base.Cleanup();
        }

        #endregion
    }
}
