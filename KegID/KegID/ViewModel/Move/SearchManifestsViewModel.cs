using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using KegID.Views;
using Microsoft.AppCenter.Crashes;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class SearchManifestsViewModel : BaseViewModel
    {
        #region Properties

        public bool IsManifestDestination { get; set; }
        public IMoveService _moveService { get; set; }

        #region TrackingNumber

        /// <summary>
        /// The <see cref="TrackingNumber" /> property's name.
        /// </summary>
        public const string TrackingNumberPropertyName = "TrackingNumber";

        private string _TrackingNumber = default(string);

        /// <summary>
        /// Sets and gets the TrackingNumber property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string TrackingNumber
        {
            get
            {
                return _TrackingNumber;
            }

            set
            {
                if (_TrackingNumber == value)
                {
                    return;
                }

                _TrackingNumber = value;
                RaisePropertyChanged(TrackingNumberPropertyName);
            }
        }

        #endregion

        #region Barcode

        /// <summary>
        /// The <see cref="Barcode" /> property's name.
        /// </summary>
        public const string BarcodePropertyName = "Barcode";

        private string _Barcode = default(string);

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

        #region ManifestSender

        /// <summary>
        /// The <see cref="ManifestSender" /> property's name.
        /// </summary>
        public const string ManifestSenderPropertyName = "ManifestSender";

        private string _ManifestSender = default(string);

        /// <summary>
        /// Sets and gets the ManifestSender property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ManifestSender
        {
            get
            {
                return _ManifestSender;
            }

            set
            {
                if (_ManifestSender == value)
                {
                    return;
                }

                _ManifestSender = value;
                RaisePropertyChanged(ManifestSenderPropertyName);
            }
        }

        #endregion

        #region ManifestDestination

        /// <summary>
        /// The <see cref="ManifestDestination" /> property's name.
        /// </summary>
        public const string ManifestDestinationPropertyName = "ManifestDestination";

        private string _ManifestDestination = default(string);

        /// <summary>
        /// Sets and gets the ManifestDestination property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ManifestDestination
        {
            get
            {
                return _ManifestDestination;
            }

            set
            {
                if (_ManifestDestination == value)
                {
                    return;
                }

                _ManifestDestination = value;
                RaisePropertyChanged(ManifestDestinationPropertyName);
            }
        }

        #endregion

        #region Referencekey

        /// <summary>
        /// The <see cref="Referencekey" /> property's name.
        /// </summary>
        public const string ReferencekeyPropertyName = "Referencekey";

        private string _Referencekey = default(string);

        /// <summary>
        /// Sets and gets the Referencekey property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Referencekey
        {
            get
            {
                return _Referencekey;
            }

            set
            {
                if (_Referencekey == value)
                {
                    return;
                }

                _Referencekey = value;
                RaisePropertyChanged(ReferencekeyPropertyName);
            }
        }

        #endregion

        #region FromDate

        /// <summary>
        /// The <see cref="FromDate" /> property's name.
        /// </summary>
        public const string FromDatePropertyName = "FromDate";

        private DateTime _FromDate = DateTime.Today;

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

        private DateTime _ToDate = DateTime.Today;

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

        #endregion

        #region Commands

        public RelayCommand ManifestsCommand { get; }
        public RelayCommand ManifestSenderCommand { get; }
        public RelayCommand ManifestDestinationCommand { get; }
        public RelayCommand SearchCommand { get; }

        #endregion

        #region Constructor

        public SearchManifestsViewModel(IMoveService moveService)
        {
            _moveService = moveService;

            ManifestsCommand = new RelayCommand(ManifestsCommandRecieverAsync);
            ManifestSenderCommand = new RelayCommand(ManifestSenderCommandRecieverAsync);
            ManifestDestinationCommand = new RelayCommand(ManifestDestinationCommandRecieverAsync);
            SearchCommand = new RelayCommand(SearchCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void SearchCommandRecieverAsync()
        {
            try
            {
                var value = await _moveService.GetManifestSearchAsync(AppSettings.User.SessionId, TrackingNumber, Barcode, ManifestSender, ManifestDestination, Referencekey, FromDate.ToString("MM/dd/yyyy", CultureInfo.CreateSpecificCulture("en-US")), ToDate.ToString("MM/dd/yyyy", CultureInfo.CreateSpecificCulture("en-US")));
                SimpleIoc.Default.GetInstance<SearchedManifestsListViewModel>().SearchManifestsCollection = value.ManifestSearchResponseModel;
                await Application.Current.MainPage.Navigation.PushModalAsync(new SearchedManifestsListView(), animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void ManifestDestinationCommandRecieverAsync()
        {
            try
            {
                IsManifestDestination = true;
                await Application.Current.MainPage.Navigation.PushModalAsync(new PartnersView(), animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void ManifestSenderCommandRecieverAsync()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushModalAsync(new PartnersView(), animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void ManifestsCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PopModalAsync();

        internal void AssignPartnerValue(PartnerModel model)
        {
            try
            {
                if (IsManifestDestination)
                {
                    IsManifestDestination = false;
                    ManifestDestination = model.FullName;
                }
                else
                    ManifestSender = model.FullName;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        #endregion
    }
}
