using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Model;
using KegID.Views;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class MaintainDetailViewModel : BaseViewModel
    {
        #region Properties

        #region TrackingNo

        /// <summary>
        /// The <see cref="TrackingNo" /> property's name.
        /// </summary>
        public const string TrackingNoPropertyName = "TrackingNo";

        private string _TrackingNo = string.Empty;

        /// <summary>
        /// Sets and gets the TrackingNo property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string TrackingNo
        {
            get
            {
                return _TrackingNo;
            }

            set
            {
                if (_TrackingNo == value)
                {
                    return;
                }

                _TrackingNo = value;
                RaisePropertyChanged(TrackingNoPropertyName);
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

        #region MaintenanceCollection

        /// <summary>
        /// The <see cref="MaintenanceCollection" /> property's name.
        /// </summary>
        public const string MaintenanceCollectionPropertyName = "MaintenanceCollection";

        private IList<string> _MaintenanceCollection = null;

        /// <summary>
        /// Sets and gets the MaintenanceCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<string> MaintenanceCollection
        {
            get
            {
                return _MaintenanceCollection;
            }

            set
            {
                if (_MaintenanceCollection == value)
                {
                    return;
                }

                _MaintenanceCollection = value;
                RaisePropertyChanged(MaintenanceCollectionPropertyName);
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
        public RelayCommand GridTappedCommand { get; }

        #endregion

        #region Constructor

        public MaintainDetailViewModel()
        {
            HomeCommand = new RelayCommand(HomeCommandCommandRecieverAsync);
            GridTappedCommand = new RelayCommand(GridTappedCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void GridTappedCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PushModalAsync(new ContentTagsView());

        private async void HomeCommandCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PopModalAsync();

        internal void LoadInfo(IList<Barcode> barcodeCollection)
        {
            TrackingNo = Uuid.GetUuId();

            StockLocation = SimpleIoc.Default.GetInstance<MaintainViewModel>().PartnerModel.FullName + "\n" + SimpleIoc.Default.GetInstance<MaintainViewModel>().PartnerModel.PartnerTypeName;
            MaintenanceCollection = SimpleIoc.Default.GetInstance<MaintainViewModel>().MaintenancePerformedCollection;
            ItemCount = barcodeCollection.Count;
            SimpleIoc.Default.GetInstance<ContentTagsViewModel>().ContentCollection = barcodeCollection.Select(x => x.Id).ToList();

            Contents = string.Empty;
        }

        #endregion

    }
}
