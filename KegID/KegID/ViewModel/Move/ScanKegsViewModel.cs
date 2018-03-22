using GalaSoft.MvvmLight.Command;
using KegID.Model;
using KegID.Services;
using KegID.Views;
using System.Linq;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using System.Collections.Generic;
using System;
using KegID.SQLiteClient;
using System.Diagnostics;
using KegID.Common;
using GalaSoft.MvvmLight.Ioc;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Newtonsoft.Json;

namespace KegID.ViewModel
{
    public class ScanKegsViewModel : BaseViewModel
    {
        ZXingScannerPage scanPage;

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
                var value = await SQLiteServiceClient.Db.Table<ValidatePartnerModel>().Where(x => x.Barcode == model.Id).ToListAsync();

                SimpleIoc.Default.GetInstance<ScanInfoViewModel>().Barcode = string.Format(" Barcode {0} ", model.Id);
                SimpleIoc.Default.GetInstance<ScanInfoViewModel>().Ownername = value.FirstOrDefault().FullName;
                SimpleIoc.Default.GetInstance<ScanInfoViewModel>().Size = value.FirstOrDefault().Size;
                SimpleIoc.Default.GetInstance<ScanInfoViewModel>().Contents = value.FirstOrDefault().Contents;
                SimpleIoc.Default.GetInstance<ScanInfoViewModel>().Batch = value.FirstOrDefault().Batch;
                SimpleIoc.Default.GetInstance<ScanInfoViewModel>().Location = value.FirstOrDefault().Location;
            }
        }

        private static async Task NavigateToValidatePartner(List<Barcode> model)
        {
            await Application.Current.MainPage.Navigation.PushPopupAsync(new ValidateBarcodeView());
            await SimpleIoc.Default.GetInstance<ValidateBarcodeViewModel>().LoadBardeValue(model);
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
                    if (BarcodeCollection.Count > 1)
                        SimpleIoc.Default.GetInstance<MoveViewModel>().AddKegs = string.Format("{0} Items", BarcodeCollection.Count);
                    else if (BarcodeCollection.Count == 1)
                        SimpleIoc.Default.GetInstance<MoveViewModel>().AddKegs = string.Format("{0} Item", BarcodeCollection.Count);
                    if (!SimpleIoc.Default.GetInstance<MoveViewModel>().IsSubmitVisible)
                        SimpleIoc.Default.GetInstance<MoveViewModel>().IsSubmitVisible = true;
                    break;
                case ViewTypeEnum.PalletizeView:
                    if (BarcodeCollection.Count > 1)
                        SimpleIoc.Default.GetInstance<PalletizeViewModel>().AddKegs = string.Format("{0} Items", BarcodeCollection.Count);
                    else if (BarcodeCollection.Count == 1)
                        SimpleIoc.Default.GetInstance<PalletizeViewModel>().AddKegs = string.Format("{0} Item", BarcodeCollection.Count);
                    if (!SimpleIoc.Default.GetInstance<PalletizeViewModel>().IsSubmitVisible)
                        SimpleIoc.Default.GetInstance<PalletizeViewModel>().IsSubmitVisible = true;
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

        private async void BarcodeManualCommandRecieverAsync()
        {
            await ValidateBarcodeInsertIntoLocalDB(ManaulBarcode);
            ManaulBarcode = string.Empty;
        }

        public async void BarcodeScanCommandReciever()
        {
            // Create our custom overlay
            var customOverlay = new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowSpacing = 0
            };

            customOverlay.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
            customOverlay.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            customOverlay.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });

            var torch = new Button
            {
                Text = "Toggle Torch"
            };
            torch.Clicked += delegate
            {
                scanPage.ToggleTorch();
            };
            var title = new Label
            {
                TextColor = Color.White,
                Margin = new Thickness(10, 0, 0, 0),
                VerticalTextAlignment = TextAlignment.End

            };
            var done = new Button
            {
                VerticalOptions = LayoutOptions.End,
                Text = "Done",
                TextColor = Color.Blue
            };
            done.Clicked += async delegate
            {
                switch ((ViewTypeEnum)Enum.Parse(typeof(ViewTypeEnum), Application.Current.MainPage.Navigation.ModalStack[Application.Current.MainPage.Navigation.ModalStack.Count - 2].GetType().Name))
                {
                    case ViewTypeEnum.FillScanView:
                        SimpleIoc.Default.GetInstance<FillScanViewModel>().BarcodeCollection = BarcodeCollection;
                        break;
                    case ViewTypeEnum.MaintainScanView:
                        SimpleIoc.Default.GetInstance<MaintainScanViewModel>().BarcodeCollection = BarcodeCollection;
                        break;
                }
                await Application.Current.MainPage.Navigation.PopModalAsync();
            };

            customOverlay.Children.Add(torch, 0, 0);
            customOverlay.Children.Add(title, 0, 1);
            customOverlay.Children.Add(done, 0, 2);


            scanPage = new ZXingScannerPage(customOverlay: customOverlay);
            scanPage.OnScanResult += (result) =>
            Device.BeginInvokeOnMainThread(async () =>
            {
                var check = BarcodeCollection.ToList().Any(x => x.Id == result.Text);

                if (!check)
                {
                    title.Text = "Last scan: " + result.Text;
                    await ValidateBarcodeInsertIntoLocalDB(result.Text);
                }
            });

            await Application.Current.MainPage.Navigation.PushModalAsync(scanPage);
        }


        private async Task ValidateBarcodeInsertIntoLocalDB(string barcodeId)
        {
            ValidateBarcodeModel validateBarcodeModel = await _moveService.GetValidateBarcodeAsync(AppSettings.User.SessionId, barcodeId);

            Barcode barcode = new Barcode
            {
                Id = barcodeId,
                PartnerCount = validateBarcodeModel.Kegs.Partners.Count,
                Icon = validateBarcodeModel.Kegs.Partners.Count > 1 ? GetIconByPlatform.GetIcon("validationerror.png") : GetIconByPlatform.GetIcon("validationquestion.png"),
            };

            BarcodeModel barcodeModel = new BarcodeModel()
            {
                Barcode = barcodeId,
                BarcodeJson = JsonConvert.SerializeObject(validateBarcodeModel)
            };
            try
            {
                // The item does not exists in the database so lets insert it
                await SQLiteServiceClient.Db.InsertAsync(barcodeModel);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            var isNew = BarcodeCollection.ToList().Any(x => x.Id == barcode.Id);
            if (!isNew)
                BarcodeCollection.Add(barcode);
        }

        public override void Cleanup()
        {
            BarcodeCollection.Clear();
            base.Cleanup();
        }

        #endregion
    }
}
