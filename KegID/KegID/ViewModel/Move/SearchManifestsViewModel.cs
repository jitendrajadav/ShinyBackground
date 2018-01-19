using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KegID.View;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class SearchManifestsViewModel : ViewModelBase
    {
        #region Properties

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

        #endregion

        #region Commands

        public RelayCommand ManifestsCommand { get; set; }
        public RelayCommand ManifestSenderCommand { get; set; }
        public RelayCommand ManifestDestinationCommand { get; set; }
        public RelayCommand SearchCommand { get; set; }

        #endregion

        #region Constructor

        public SearchManifestsViewModel()
        {
            ManifestsCommand = new RelayCommand(ManifestsCommandRecieverAsync);
            ManifestSenderCommand = new RelayCommand(ManifestSenderCommandRecieverAsync);
            ManifestDestinationCommand = new RelayCommand(ManifestDestinationCommandRecieverAsync);
            SearchCommand = new RelayCommand(SearchCommandRecieverAsync);
        }

        #endregion

        #region Methods
        private async void SearchCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PushModalAsync(new SearchedManifestsListView());

        private async void ManifestDestinationCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PushModalAsync(new PartnersView());

        private async void ManifestSenderCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PushModalAsync(new PartnersView());

        private async void ManifestsCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PopModalAsync();

        #endregion
    }
}
