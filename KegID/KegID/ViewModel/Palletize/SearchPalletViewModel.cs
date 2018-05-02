using System;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Model;
using KegID.Views;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class SearchPalletViewModel : BaseViewModel
    {
        #region Properties

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

        private DateTime _FromDate = DateTime.Now;

        /// <summary>
        /// Sets and gets the FromDate property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DateTime FromDate
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

        private DateTime _ToDate = DateTime.Now;

        /// <summary>
        /// Sets and gets the ToDate property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DateTime ToDate
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

        public RelayCommand HomeCommand { get; }
        public RelayCommand SearchCommand { get; }
        public RelayCommand LocationCreatedCommand { get; }

        #endregion

        #region Contructor

        public SearchPalletViewModel()
        {
            HomeCommand = new RelayCommand(HomeCommandRecieverAsync);
            SearchCommand = new RelayCommand(SearchCommandRecieverAsync);
            LocationCreatedCommand = new RelayCommand(LocationCreatedCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void LocationCreatedCommandRecieverAsync()
        {
          await Application.Current.MainPage.Navigation.PushModalAsync(new PartnersView(), animated: false);
        }

        private async void HomeCommandRecieverAsync()
        {
           await Application.Current.MainPage.Navigation.PopModalAsync();
        }
        private async void SearchCommandRecieverAsync()
        {
            SimpleIoc.Default.GetInstance<PalletSearchedListViewModel>().GetPalletSearchAsync(PartnerModel?.PartnerId,FromDate.ToShortDateString(),ToDate.ToShortDateString(),string.Empty,string.Empty);
            await Application.Current.MainPage.Navigation.PushModalAsync(new PalletSearchedListView(), animated: false);
        }

        #endregion
    }
}
