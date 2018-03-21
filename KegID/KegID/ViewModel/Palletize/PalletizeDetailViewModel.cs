using System;
using GalaSoft.MvvmLight.Command;
using KegID.Views;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class PalletizeDetailViewModel : BaseViewModel
    {
        #region Properties

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

        #endregion

        #region Commands

        public RelayCommand HomeCommand { get; }
        public RelayCommand ShareCommand { get; }
        public RelayCommand GridTappedCommand { get; }

        #endregion

        #region Constructor

        public PalletizeDetailViewModel()
        {
            HomeCommand = new RelayCommand(HomeCommandCommandRecieverAsync);
            ShareCommand = new RelayCommand(ShareCommandReciever);
            GridTappedCommand = new RelayCommand(GridTappedCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void GridTappedCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PushModalAsync(new ContentTagsView());

        private void ShareCommandReciever()
        {
           
        }

        private async void HomeCommandCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PopModalAsync();

        #endregion

    }
}
