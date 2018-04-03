using System;
using GalaSoft.MvvmLight.Command;
using KegID.Model;
using KegID.Views;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class SearchPalletViewModel : BaseViewModel
    {
        #region Properties

        #region PalletBorcode

        /// <summary>
        /// The <see cref="PalletBorcode" /> property's name.
        /// </summary>
        public const string PalletBorcodePropertyName = "PalletBorcode";

        private string _PalletBorcode = string.Empty;

        /// <summary>
        /// Sets and gets the PalletBorcode property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string PalletBorcode
        {
            get
            {
                return _PalletBorcode;
            }

            set
            {
                if (_PalletBorcode == value)
                {
                    return;
                }

                _PalletBorcode = value;
                RaisePropertyChanged(PalletBorcodePropertyName);
            }
        }

        #endregion

        #region Borcode

        /// <summary>
        /// The <see cref="Borcode" /> property's name.
        /// </summary>
        public const string BorcodePropertyName = "Borcode";

        private string _Borcode = string.Empty;

        /// <summary>
        /// Sets and gets the Borcode property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Borcode
        {
            get
            {
                return _Borcode;
            }

            set
            {
                if (_Borcode == value)
                {
                    return;
                }

                _Borcode = value;
                RaisePropertyChanged(BorcodePropertyName);
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
            HomeCommand = new RelayCommand(HomeCommandReciever);
            SearchCommand = new RelayCommand(SearchCommandReciever);
            LocationCreatedCommand = new RelayCommand(LocationCreatedCommandReciever);
        }

        #endregion

        #region Methods

        private void LocationCreatedCommandReciever()
        {
            Application.Current.MainPage.Navigation.PushModalAsync(new PartnersView());
        }

        private void HomeCommandReciever()
        {
            throw new NotImplementedException();
        }
        private void SearchCommandReciever()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
