using System;
using KegID.Model;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;

namespace KegID.ViewModel
{
    public class SearchPalletViewModel : BaseViewModel
    {
        #region Properties

        //private readonly INavigationService _navigationService;

        #region PalletBarcode

        /// <summary>
        /// The <see cref="PalletBarcode" /> property's name.
        /// </summary>
        public const string PalletBarcodePropertyName = "PalletBarcode";

        private string _PalletBarcode = string.Empty;

        /// <summary>
        /// Sets and gets the PalletBarcode property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string PalletBarcode
        {
            get
            {
                return _PalletBarcode;
            }

            set
            {
                if (_PalletBarcode == value)
                {
                    return;
                }

                _PalletBarcode = value;
                RaisePropertyChanged(PalletBarcodePropertyName);
            }
        }

        #endregion

        #region Barcode

        /// <summary>
        /// The <see cref="Barcode" /> property's name.
        /// </summary>
        public const string BarcodePropertyName = "Barcode";

        private string _Barcode = string.Empty;

        /// <summary>
        /// Sets and gets the Barcode property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Barcode
        {
            get
            {
                return _Barcode;
            }

            set
            {
                if (_Barcode == value)
                {
                    return;
                }

                _Barcode = value;
                RaisePropertyChanged(BarcodePropertyName);
            }
        }

        #endregion

        #region LocationCreated

        /// <summary>
        /// The <see cref="LocationCreated" /> property's name.
        /// </summary>
        public const string LocationCreatedPropertyName = "LocationCreated";

        private string _LocationCreated = string.Empty;

        /// <summary>
        /// Sets and gets the LocationCreated property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string LocationCreated
        {
            get
            {
                return _LocationCreated;
            }

            set
            {
                if (_LocationCreated == value)
                {
                    return;
                }

                _LocationCreated = value;
                RaisePropertyChanged(LocationCreatedPropertyName);
            }
        }

        #endregion

        #region FromDate

        /// <summary>
        /// The <see cref="FromDate" /> property's name.
        /// </summary>
        public const string FromDatePropertyName = "FromDate";

        private DateTimeOffset _FromDate = DateTimeOffset.Now;

        /// <summary>
        /// Sets and gets the FromDate property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DateTimeOffset FromDate
        {
            get
            {
                return _FromDate;
            }

            set
            {
                if (_FromDate == value)
                {
                    return;
                }

                _FromDate = value;
                RaisePropertyChanged(FromDatePropertyName);
            }
        }

        #endregion

        #region ToDate

        /// <summary>
        /// The <see cref="ToDate" /> property's name.
        /// </summary>
        public const string ToDatePropertyName = "ToDate";

        private DateTimeOffset _ToDate = DateTimeOffset.Now;

        /// <summary>
        /// Sets and gets the ToDate property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DateTimeOffset ToDate
        {
            get
            {
                return _ToDate;
            }

            set
            {
                if (_ToDate == value)
                {
                    return;
                }

                _ToDate = value;
                RaisePropertyChanged(ToDatePropertyName);
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
                LocationCreated = PartnerModel.FullName;
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand HomeCommand { get; }
        public DelegateCommand SearchCommand { get; }
        public DelegateCommand LocationCreatedCommand { get; }

        #endregion

        #region Contructor

        public SearchPalletViewModel(INavigationService navigationService) : base(navigationService)
        {
            //_navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            HomeCommand = new DelegateCommand(HomeCommandRecieverAsync);
            SearchCommand = new DelegateCommand(SearchCommandRecieverAsync);
            LocationCreatedCommand = new DelegateCommand(LocationCreatedCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void LocationCreatedCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync("PartnersView", animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void HomeCommandRecieverAsync()
        {
            try
            {
                await _navigationService.GoBackAsync(animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
        private async void SearchCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync("PalletSearchedListView", new NavigationParameters
                    {
                        { "GetPalletSearchAsync", PartnerModel?.PartnerId },{ "FromDate", FromDate.Date.ToShortDateString() },{ "ToDate", ToDate.Date.ToShortDateString() }
                    }, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("model"))
            {
                PartnerModel = parameters.GetValue<PartnerModel>("model");
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("HomeCommandRecieverAsync"))
            {
                HomeCommandRecieverAsync();
            }
        }

        #endregion
    }
}
