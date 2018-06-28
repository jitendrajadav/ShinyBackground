using System;
using System.Collections.Generic;
using KegID.Common;
using KegID.Model;
using KegID.Views;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class MaintainDetailViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;

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
            //await Application.Current.MainPage.Navigation.PushModalAsync(new ContentTagsView(), animated: false);
            await _navigationService.NavigateAsync(new Uri("ContentTagsView", UriKind.Relative), useModalNavigation: true, animated: false);
        }

        private async void HomeCommandCommandRecieverAsync()
        {
            //await Application.Current.MainPage.Navigation.PopModalAsync();
            await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
        }

        internal void LoadInfo(IList<BarcodeModel> barcodeCollection)
        {
            try
            {
                TrackingNo = Uuid.GetUuId();

                //StockLocation = SimpleIoc.Default.GetInstance<MaintainViewModel>().PartnerModel.FullName + "\n" + SimpleIoc.Default.GetInstance<MaintainViewModel>().PartnerModel.PartnerTypeName;
                //ItemCount = barcodeCollection.Count;
                //SimpleIoc.Default.GetInstance<ContentTagsViewModel>().ContentCollection = barcodeCollection.Select(x => x.Barcode).ToList();

                Contents = string.Empty;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {

        }

        #endregion

    }
}
