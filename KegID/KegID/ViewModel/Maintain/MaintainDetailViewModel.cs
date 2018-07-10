using System;
using System.Collections.Generic;
using System.Linq;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;

namespace KegID.ViewModel
{
    public class MaintainDetailViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;
        public List<string> Barcodes { get; private set; }

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

        public DelegateCommand HomeCommand { get; }
        public DelegateCommand GridTappedCommand { get; }

        #endregion

        #region Constructor

        public MaintainDetailViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            HomeCommand = new DelegateCommand(HomeCommandCommandRecieverAsync);
            GridTappedCommand = new DelegateCommand(GridTappedCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void GridTappedCommandRecieverAsync()
        {
            var param = new NavigationParameters
                            {
                                { "Barcode", Barcodes }
                            };
            await _navigationService.NavigateAsync(new Uri("ContentTagsView", UriKind.Relative), param, useModalNavigation: true, animated: false);
        }

        private async void HomeCommandCommandRecieverAsync()
        {
            await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
        }

        internal void LoadInfo(IList<BarcodeModel> barcodeCollection)
        {
            try
            {
                TrackingNo = Uuid.GetUuId();
                StockLocation = ConstantManager.Partner.FullName + "\n" + ConstantManager.Partner.PartnerTypeName;
                ItemCount = barcodeCollection.Count;
                Barcodes = barcodeCollection.Select(x => x.Barcode).ToList();
                Contents = string.Empty;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("BarcodeModel"))
            {
                LoadInfo(parameters.GetValue<IList<BarcodeModel>>("BarcodeModel"));
            }
        }

        #endregion

    }
}
