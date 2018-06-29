using System;
using System.Collections.Generic;
using System.Linq;
using KegID.Common;
using KegID.DependencyServices;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class PalletizeDetailViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;
        public IMoveService _moveService { get; set; }
        public SearchPalletResponseModel Model { get; set; }
        public List<string> Barcodes { get; private set; }

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

        #region StockLocation

        /// <summary>
        /// The <see cref="StockLocation" /> property's name.
        /// </summary>
        public const string StockLocationPropertyName = "StockLocation";

        private string _StockLocation = default(string);

        /// <summary>
        /// Sets and gets the StockLocation property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string StockLocation
        {
            get
            {
                return _StockLocation;
            }

            set
            {
                if (_StockLocation == value)
                {
                    return;
                }

                _StockLocation = value;
                RaisePropertyChanged(StockLocationPropertyName);
            }
        }

        #endregion

        #region PartnerTypeName

        /// <summary>
        /// The <see cref="PartnerTypeName" /> property's name.
        /// </summary>
        public const string PartnerTypeNamePropertyName = "PartnerTypeName";

        private string _PartnerTypeName = default(string);

        /// <summary>
        /// Sets and gets the PartnerTypeName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string PartnerTypeName
        {
            get
            {
                return _PartnerTypeName;
            }

            set
            {
                if (_PartnerTypeName == value)
                {
                    return;
                }

                _PartnerTypeName = value;
                RaisePropertyChanged(PartnerTypeNamePropertyName);
            }
        }

        #endregion

        #region ShippingDate

        /// <summary>
        /// The <see cref="ShippingDate" /> property's name.
        /// </summary>
        public const string ShippingDatePropertyName = "ShippingDate";

        private DateTime _ShippingDate = DateTime.Today;

        /// <summary>
        /// Sets and gets the ShippingDate property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DateTime ShippingDate
        {
            get
            {
                return _ShippingDate;
            }

            set
            {
                if (_ShippingDate == value)
                {
                    return;
                }

                _ShippingDate = value;
                RaisePropertyChanged(ShippingDatePropertyName);
            }
        }

        #endregion

        #region TargetLocation

        /// <summary>
        /// The <see cref="TargetLocation" /> property's name.
        /// </summary>
        public const string TargetLocationPropertyName = "TargetLocation";

        private string _TargetLocation = default(string);

        /// <summary>
        /// Sets and gets the TargetLocation property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string TargetLocation
        {
            get
            {
                return _TargetLocation;
            }

            set
            {
                if (_TargetLocation == value)
                {
                    return;
                }

                _TargetLocation = value;
                RaisePropertyChanged(TargetLocationPropertyName);
            }
        }

        #endregion

        #region ItemCount

        /// <summary>
        /// The <see cref="ItemCount" /> property's name.
        /// </summary>
        public const string ItemCountPropertyName = "ItemCount";

        private int _ItemCount = 0;

        /// <summary>
        /// Sets and gets the ItemCount property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int ItemCount
        {
            get
            {
                return _ItemCount;
            }

            set
            {
                if (_ItemCount == value)
                {
                    return;
                }

                _ItemCount = value;
                RaisePropertyChanged(ItemCountPropertyName);
            }
        }

        #endregion

        #region Contents

        /// <summary>
        /// The <see cref="Contents" /> property's name.
        /// </summary>
        public const string ContentsPropertyName = "Contents";

        private string _Contents = "No contents";

        /// <summary>
        /// Sets and gets the Contents property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Contents
        {
            get
            {
                return _Contents;
            }

            set
            {
                if (_Contents == value)
                {
                    return;
                }

                _Contents = value;
                RaisePropertyChanged(ContentsPropertyName);
            }
        }

        #endregion

        #region IsFromDashboard

        /// <summary>
        /// The <see cref="IsFromDashboard" /> property's name.
        /// </summary>
        public const string IsFromDashboardPropertyName = "IsFromDashboard";

        private bool _IsFromDashboard = false;

        /// <summary>
        /// Sets and gets the IsFromDashboard property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsFromDashboard
        {
            get
            {
                return _IsFromDashboard;
            }

            set
            {
                if (_IsFromDashboard == value)
                {
                    return;
                }

                _IsFromDashboard = value;
                RaisePropertyChanged(IsFromDashboardPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand HomeCommand { get; }
        public DelegateCommand ShareCommand { get; }
        public DelegateCommand GridTappedCommand { get; }
        public DelegateCommand EditPalletCommand { get; }
        public DelegateCommand MovePalletCommand { get; }

        #endregion

        #region Constructor

        public PalletizeDetailViewModel(IMoveService moveService, INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            _moveService = moveService;
            HomeCommand = new DelegateCommand(HomeCommandCommandRecieverAsync);
            ShareCommand = new DelegateCommand(ShareCommandReciever);
            GridTappedCommand = new DelegateCommand(GridTappedCommandRecieverAsync);
            MovePalletCommand = new DelegateCommand(MovePalletCommandRecieverAsync);
            EditPalletCommand = new DelegateCommand(EditPalletCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void EditPalletCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync(new Uri("PalletizeView", UriKind.Relative), useModalNavigation: true, animated: false);
                //await Application.Current.MainPage.Navigation.PushModalAsync(new PalletizeView(), animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void MovePalletCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync(new Uri("MoveView", UriKind.Relative), useModalNavigation: true, animated: false);
                //await Application.Current.MainPage.Navigation.PushModalAsync(new MoveView(), animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void GridTappedCommandRecieverAsync()
        {
            //await Application.Current.MainPage.Navigation.PushModalAsync(new ContentTagsView(), animated: false);
            var param = new NavigationParameters
                            {
                                { "Barcode", Barcodes }
                            };
            await _navigationService.NavigateAsync(new Uri("ContentTagsView", UriKind.Relative), param,useModalNavigation: true, animated: false);
        }

        private void ShareCommandReciever()
        {
            try
            {
                var share = DependencyService.Get<IShare>();
                share.Show("Share", "Share", "https://www.slg.com/");
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void HomeCommandCommandRecieverAsync()
        {
            //await Application.Current.MainPage.Navigation.PopModalAsync();
            await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
        }

        internal void LoadInfo(PalletResponseModel value)
        {
            try
            {
                ManifestId = value.Barcode;
                PartnerTypeName = value.StockLocation.PartnerTypeName;
                StockLocation = value.StockLocation.FullName;
                TargetLocation = value.StockLocation.FullName;
                ShippingDate = value.BuildDate.Date;
                ItemCount = value.PalletItems.Count;
                Barcodes = value.PalletItems.Select(selector: x => x.Barcode).ToList();
                //SimpleIoc.Default.GetInstance<ContentTagsViewModel>().ContentCollection = value.PalletItems.Select(selector: x => x.Barcode).ToList();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal async void AssingIntialValueAsync(SearchPalletResponseModel model, bool v)
        {
            try
            {
                var manifest = await _moveService.GetManifestAsync(AppSettings.User.SessionId, model.Barcode);

                Model = model;
                IsFromDashboard = v;
                ManifestId = model.Barcode;
                PartnerTypeName = model.OwnerName;
                StockLocation = model.LocationName;
                TargetLocation = model.LocationName;
                ShippingDate = model.BuildDate.Date;
                ItemCount = (int)model.BuildCount;
                Barcodes = new List<string> { model.Barcode };
                //SimpleIoc.Default.GetInstance<ContentTagsViewModel>().ContentCollection = new List<string> { model.Barcode };
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("LoadInfo"))
            {
                LoadInfo(parameters.GetValue<PalletResponseModel>("LoadInfo"));
            }
            if (parameters.ContainsKey("model"))
            {
                AssingIntialValueAsync(parameters.GetValue<SearchPalletResponseModel>("model"),true);
            }
        }

        #endregion
    }
}
