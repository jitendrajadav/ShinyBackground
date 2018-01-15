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

namespace KegID.ViewModel
{
    public class ScanKegsViewModel : ViewModelBase
    {
        ZXingScannerPage scanPage;

        #region Properties

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


        #endregion

        #region Commands

        public RelayCommand DoneCommand { get; set; }
        public RelayCommand BarcodeScanCommand { get; set; }
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
            AddTagsCommand = new RelayCommand(AddTagsCommandRecieverAsync);
            LabelItemTappedCommand = new RelayCommand<Barcode>((model) => LabelItemTappedCommandRecieverAsync(model));
            IconItemTappedCommand = new RelayCommand<Barcode>((model) => IconItemTappedCommandRecieverAsync(model));
            LoadBrandAsync();
        }
       
        #endregion

        #region Methods
        private async void LoadBrandAsync()
        {
            var model = await SQLiteServiceClient.Db.Table<BrandModel>().ToListAsync();
            try
            {
                if (model.Count > 0)
                    BrandCollection = model;
                else
                {
                    Loader.StartLoading();
                    BrandCollection = await _moveService.GetBrandListAsync(Configuration.SessionId);
                    await SQLiteServiceClient.Db.InsertAllAsync(BrandCollection);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                model = null;
                Loader.StopLoading();
            }
        }

        private async void LabelItemTappedCommandRecieverAsync(Barcode model)
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new ValidateBarcodeView());

            SimpleIoc.Default.GetInstance<ValidateBarcodeViewModel>().MultipleKegsTitle = string.Format(" Multiple kgs were found with \n barcode {0}. \n Please select the correct one.", model.Id);
            SimpleIoc.Default.GetInstance<ValidateBarcodeViewModel>().PartnerCollection = await SQLiteServiceClient.Db.Table<PartnerTable>().Where(x => x.Barcode == model.Id).ToListAsync();
        }

        private async void IconItemTappedCommandRecieverAsync(Barcode model)
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new ValidateBarcodeView());
        }

        private async void AddTagsCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new AddTagsView());
        }

        private async void DoneCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void BarcodeScanCommandReciever()
        {
            ValidateBarcodeModel validateBarcodeModel = null;

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
            var check = BarcodeCollection.ToList().Find(x => x.Id == result.Text);

            if (check == null)
            {
                validateBarcodeModel = await _moveService.GetValidateBarcodeAsync(Configuration.SessionId, result.Text);
                title.Text = "Last scan: " + result.Text;
                Barcode barcode = new Barcode
                {
                    Id = result.Text,
                    Icon = validateBarcodeModel.Kegs.Partners.Count > 1 ? "validationerror.png" : "validationquestion.png"
                };
                    var partnerTable = (from partner in validateBarcodeModel.Kegs.Partners
                                        select new PartnerTable()
                                        {
                                            Barcode = validateBarcodeModel.Kegs.Partners.FirstOrDefault().Kegs.FirstOrDefault().Barcode,
                                            ParentPartnerId = partner.ParentPartnerId,
                                            Address = partner.Address,
                                            Address1 = partner.Address1,
                                            City = partner.City,
                                            CompanyNo = partner.CompanyNo,
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
                        var rowsAffected = await SQLiteServiceClient.Db.UpdateAllAsync(partnerTable);
                        if (rowsAffected == 0)
                        {
                            // The item does not exists in the database so lets insert it
                            await SQLiteServiceClient.Db.InsertAllAsync(partnerTable);
                        }
                    }
                    catch (Exception ex)
                    {
                        await SQLiteServiceClient.Db.InsertAllAsync(partnerTable);
                        Debug.WriteLine(ex.Message);
                    }

                    BarcodeCollection.Add(barcode); 
                }
            });

            await Application.Current.MainPage.Navigation.PushModalAsync(scanPage);
        }

        #endregion
    }
}
