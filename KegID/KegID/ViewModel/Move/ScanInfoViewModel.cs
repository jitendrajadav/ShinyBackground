using KegID.Model;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Linq;

namespace KegID.ViewModel
{
    public class ScanInfoViewModel  : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;

        #region Barcode

        /// <summary>
        /// The <see cref="Barcode" /> property's name.
        /// </summary>
        public const string BarcodePropertyName = "Barcode";

        private string _Barcode = "Barcode ";

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

        #region AltBarcode

        /// <summary>
        /// The <see cref="AltBarcode" /> property's name.
        /// </summary>
        public const string AltBarcodePropertyName = "AltBarcode";

        private string _AltBarcode = default;

        /// <summary>
        /// Sets and gets the AltBarcode property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string AltBarcode
        {
            get
            {
                return _AltBarcode;
            }

            set
            {
                if (_AltBarcode == value)
                {
                    return;
                }

                _AltBarcode = value;
                RaisePropertyChanged(AltBarcodePropertyName);
            }
        }

        #endregion

        #region Ownername

        /// <summary>
        /// The <see cref="Ownername" /> property's name.
        /// </summary>
        public const string OwnernamePropertyName = "Ownername";

        private string _Ownername = default;

        /// <summary>
        /// Sets and gets the Ownername property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Ownername
        {
            get
            {
                return _Ownername;
            }

            set
            {
                if (_Ownername == value)
                {
                    return;
                }

                _Ownername = value;
                RaisePropertyChanged(OwnernamePropertyName);
            }
        }
        #endregion

        #region Size

        /// <summary>
        /// The <see cref="Size" /> property's name.
        /// </summary>
        public const string SizePropertyName = "Size";

        private string _Size = default;

        /// <summary>
        /// Sets and gets the Size property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Size
        {
            get
            {
                return _Size;
            }

            set
            {
                if (_Size == value)
                {
                    return;
                }

                _Size = value;
                RaisePropertyChanged(SizePropertyName);
            }
        }

        #endregion

        #region Contents

        /// <summary>
        /// The <see cref="Contents" /> property's name.
        /// </summary>
        public const string ContentsPropertyName = "Contents";

        private string _Contents = default;

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

        #region Batch

        /// <summary>
        /// The <see cref="Batch" /> property's name.
        /// </summary>
        public const string BatchPropertyName = "Batch";

        private string _Batch = default;

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
                RaisePropertyChanged(BatchPropertyName);
            }
        }

        #endregion

        #region Location

        /// <summary>
        /// The <see cref="Location" /> property's name.
        /// </summary>
        public const string LocationPropertyName = "Location";

        private string _Location = default;

        /// <summary>
        /// Sets and gets the Location property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Location
        {
            get
            {
                return _Location;
            }

            set
            {
                if (_Location == value)
                {
                    return;
                }

                _Location = value;
                RaisePropertyChanged(LocationPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand DoneCommand { get; }

        #endregion

        #region Constructor

        public ScanInfoViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            DoneCommand = new DelegateCommand(DoneCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void DoneCommandRecieverAsync()
        {
            try
            {
                await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal void AssignInitialValue(BarcodeModel _barcode)
        {
            try
            {
                Barcode = string.Format(" Barcode {0} ", _barcode.Barcode);
                //AltBarcode = _barcode.Barcode;
                Ownername = _barcode?.Kegs?.Partners?.FirstOrDefault()?.FullName;
                try
                {
                    Size = _barcode?.Tags[3]?.Value;
                }
                catch (Exception ex)
                {
                    Size = _barcode?.Kegs.Sizes.FirstOrDefault();
                    Crashes.TrackError(ex);
                }
                Contents = _barcode.Contents;
                Batch = _barcode.Kegs.Batches.FirstOrDefault();
                Location = _barcode.Kegs.Locations.FirstOrDefault().Name;
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
                AssignInitialValue(parameters.GetValue<BarcodeModel>("model"));
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("DoneCommandRecieverAsync"))
            {
                DoneCommandRecieverAsync();
            }
        }

        #endregion
    }
}
