using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        private readonly IUuidManager _uuidManager;

        public List<string> Barcodes { get; private set; }
        public string TrackingNo { get; set; }
        public string StockLocation { get; set; }
        public int ItemCount { get; set; }
        public string Contents { get; set; }

        #endregion

        #region Commands

        public DelegateCommand HomeCommand { get; }
        public DelegateCommand GridTappedCommand { get; }

        #endregion

        #region Constructor

        public MaintainDetailViewModel(INavigationService navigationService, IUuidManager uuidManager) : base(navigationService)
        {
            _uuidManager = uuidManager;

            HomeCommand = new DelegateCommand(HomeCommandCommandRecieverAsync);
            GridTappedCommand = new DelegateCommand(GridTappedCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void GridTappedCommandRecieverAsync()
        {
            await _navigationService.NavigateAsync("ContentTagsView", new NavigationParameters
                            {
                                { "Barcode", Barcodes }
                            }, animated: false);
        }

        private async void HomeCommandCommandRecieverAsync()
        {
            await _navigationService.GoBackToRootAsync();
        }

        internal void LoadInfo(IList<BarcodeModel> barcodeCollection)
        {
            TrackingNo = _uuidManager.GetUuId();
            StockLocation = ConstantManager.Partner.FullName + "\n" + ConstantManager.Partner.PartnerTypeName;
            ItemCount = barcodeCollection.Count;
            Barcodes = barcodeCollection.Select(x => x.Barcode).ToList();
            Contents = string.Empty;
        }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("BarcodeModel"))
            {
                LoadInfo(parameters.GetValue<IList<BarcodeModel>>("BarcodeModel"));
            }
            return base.InitializeAsync(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("HomeCommandCommandRecieverAsync"))
            {
                HomeCommandCommandRecieverAsync();
            }
        }

        #endregion

    }
}
