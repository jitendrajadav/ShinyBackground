using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class FillScanReviewViewModel : BaseViewModel
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

        #region ManifestTo

        /// <summary>
        /// The <see cref="ManifestTo" /> property's name.
        /// </summary>
        public const string ManifestToPropertyName = "ManifestTo";

        private string _ManifestTo = default(string);

        /// <summary>
        /// Sets and gets the ManifestTo property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ManifestTo
        {
            get
            {
                return _ManifestTo;
            }

            set
            {
                if (_ManifestTo == value)
                {
                    return;
                }

                _ManifestTo = value;
                RaisePropertyChanged(ManifestToPropertyName);
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

        #endregion

        #region Commands

        public RelayCommand ScanCommand { get;}
        public RelayCommand SubmitCommand { get; }

        #endregion

        #region Constructor

        public FillScanReviewViewModel()
        {
            ScanCommand = new RelayCommand(ScanCommandRecieverAsync);
            SubmitCommand = new RelayCommand(SubmitCommandReciever);
        }

        #endregion

        #region Methods

        private void SubmitCommandReciever()
        {
            SimpleIoc.Default.GetInstance<AddPalletsViewModel>().SubmitCommandRecieverAsync();
        }

        private async void ScanCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PopModalAsync();

        internal void AssignInitialValue(string _manifestId,int _count)
        {
            var partner = SimpleIoc.Default.GetInstance<FillViewModel>().PartnerModel;
            var content = SimpleIoc.Default.GetInstance<FillViewModel>().BatchButtonTitle;
            TrackingNumber = Uuid.GetUuId();
            ManifestTo = partner.FullName + "\n" + partner.PartnerTypeCode;
            ItemCount = _count;
            Contents = !string.IsNullOrEmpty(content) ? content : "No contens";
        }

        #endregion
    }
}
