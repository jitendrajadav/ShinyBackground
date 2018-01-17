using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KegID.Model;
using KegID.Response;
using KegID.Services;
using KegID.View;
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

namespace KegID.ViewModel
{
    public class ScanKegsViewModel : ViewModelBase
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

        private string _tags = default(string);

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

        #endregion

        #region Commands

        public RelayCommand DoneCommand { get; set; }
        public RelayCommand BarcodeScanCommand { get; set; }
        public RelayCommand BarcodeManualCommand { get; set; }
        public RelayCommand AddTagsCommand { get; set; }
        public RelayCommand<Barcode> IconItemTappedCommand { get; set; }
        public RelayCommand<Barcode> LabelItemTappedCommand { get; set; }

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

        public async void LoadBrand()
        {
            BrandCollection = await LoadBrandAsync();
        }

        #endregion

        #region Methods
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
                    model = await _moveService.GetBrandListAsync(Configuration.SessionId);
                    await SQLiteServiceClient.Db.InsertAllAsync(model);
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
            if (model.Icon == "validationerror.png")
                await NavigateToValidatePartner(model);
            else
            {
                IsFromScanned = true;
                await Application.Current.MainPage.Navigation.PushModalAsync(new AddTagsView());
            }
        }

        private async void IconItemTappedCommandRecieverAsync(Barcode model)
        {
            if (model.Icon == "validationerror.png")
                await NavigateToValidatePartner(model);
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

        private static async Task NavigateToValidatePartner(Barcode model)
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new ValidateBarcodeView());
            SimpleIoc.Default.GetInstance<ValidateBarcodeViewModel>().MultipleKegsTitle = string.Format(" Multiple kgs were found with \n barcode {0}. \n Please select the correct one.", model.Id);
            SimpleIoc.Default.GetInstance<ValidateBarcodeViewModel>().PartnerCollection = await SQLiteServiceClient.Db.Table<ValidatePartnerModel>().Where(x => x.Barcode == model.Id).ToListAsync();
        }

        private async void AddTagsCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new AddTagsView());
        }

        private async void DoneCommandRecieverAsync()
        {
            if (BarcodeCollection.Count > 1)
                SimpleIoc.Default.GetInstance<MoveViewModel>().AddKegs = string.Format("{0} Items", BarcodeCollection.Count);
            else if (BarcodeCollection.Count == 1)
                SimpleIoc.Default.GetInstance<MoveViewModel>().AddKegs = string.Format("{0} Item", BarcodeCollection.Count);

            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void BarcodeManualCommandRecieverAsync()
        {
            await ValidateBarcodeInsertIntoLocalDB(ManaulBarcode);
            ManaulBarcode = string.Empty;
        }

        private async void BarcodeScanCommandReciever()
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
            ValidateBarcodeModel validateBarcodeModel = await _moveService.GetValidateBarcodeAsync(Configuration.SessionId, barcodeId);

            Barcode barcode = new Barcode
            {
                Id = barcodeId,
                Icon = validateBarcodeModel.Kegs.Partners.Count > 1 ? "validationerror.png" : "validationquestion.png",
                Tags = Tags
            };
            var partnerTable = (from partner in validateBarcodeModel.Kegs.Partners
                                select new ValidatePartnerModel()
                                {
                                    Contents = validateBarcodeModel.Kegs.Contents.FirstOrDefault(),
                                    Size = validateBarcodeModel.Kegs.Sizes.FirstOrDefault(),
                                    Batch = validateBarcodeModel.Kegs.Batches.FirstOrDefault(),
                                    Location = validateBarcodeModel.Kegs.Locations.FirstOrDefault().Name,
                                    Barcode = validateBarcodeModel.Kegs.Partners.FirstOrDefault().Kegs.FirstOrDefault().Barcode,
                                    ParentPartnerId = partner.ParentPartnerId,
                                    Address = partner.Address,
                                    Address1 = partner.Address1,
                                    City = partner.City,
                                    CompanyNo = partner.CompanyNo ?? 0,
                                    Country = partner.Country,
                                    FullName = partner.FullName,
                                    IsActive = partner.IsActive,
                                    IsInternal = partner.IsInternal,
                                    IsShared = partner.IsShared,
                                    Lat = partner.Lat,
                                    LocationCode = partner.LocationCode,
                                    LocationStatus = partner.LocationStatus,
                                    Lon = partner.Lon,
                                    MasterCompanyId = partner.MasterCompanyId,
                                    ParentPartnerName = partner.ParentPartnerName,
                                    PartnerId = partner.PartnerId,
                                    PartnershipIsActive = partner.PartnershipIsActive,
                                    PartnerTypeCode = partner.PartnerTypeCode,
                                    PartnerTypeName = partner.PartnerTypeName,
                                    PhoneNumber = partner.PhoneNumber,
                                    PostalCode = partner.PostalCode,
                                    SourceKey = partner.SourceKey,
                                    State = partner.State
                                }).ToList();

            try
            {
                string tempBarcodeId = validateBarcodeModel.Kegs.Partners.FirstOrDefault().Kegs.FirstOrDefault().Barcode;
                var rowsAffected = await SQLiteServiceClient.Db.Table<ValidatePartnerModel>().Where(x => x.Barcode == tempBarcodeId).ToListAsync();
                if (rowsAffected.Count == 0)
                {
                    // The item does not exists in the database so lets insert it
                    await SQLiteServiceClient.Db.InsertAllAsync(partnerTable);
                }
                else
                    await SQLiteServiceClient.Db.UpdateAllAsync(partnerTable);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            var isNew = BarcodeCollection.ToList().Any(x => x.Id == barcode.Id);
            if (!isNew)
                BarcodeCollection.Add(barcode);
        }

        #endregion
    }
}
