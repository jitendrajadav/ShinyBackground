using System.Linq;
using System.Collections.Generic;
using KegID.Model;
using Prism.Commands;
using Prism.Navigation;
using System.Threading.Tasks;

namespace KegID.ViewModel
{
    public class ValidateBarcodeViewModel : BaseViewModel
    {
        #region Properties

        public List<BarcodeModel> Models { get; set; }
        public string MultipleKegsTitle { get; set; }
        public IList<Partner> PartnerCollection { get; set; }

        #endregion

        #region Commands

        public DelegateCommand CancelCommand { get; }
        public DelegateCommand<Partner> ItemTappedCommand { get; }

        #endregion

        #region Constructor

        public ValidateBarcodeViewModel(INavigationService navigationService) : base(navigationService)
        {
            CancelCommand = new DelegateCommand(CancelCommandRecievierAsync);
            ItemTappedCommand = new DelegateCommand<Partner>((model) => ItemTappedCommandRecieverAsync(model));
        }

        #endregion

        #region Methods

        private async void CancelCommandRecievierAsync()
        {
            await _navigationService.ClearPopupStackAsync(animated: false);
        }

        private void ItemTappedCommandRecieverAsync(Partner model)
        {
            var formsNav = ((Prism.Common.IPageAware)_navigationService).Page;
            var page = formsNav.Navigation.NavigationStack.Last();
            (page?.BindingContext as INavigationAware)?.OnNavigatedTo(new NavigationParameters
                    {
                        { "Partner", model }
                    });

            if (Models.Count > 0)
                Models.RemoveAt(0);

            if (Models.Count > 0)
                ValidateScannedBarcode();
        }

        public void LoadBarcodeValue(List<BarcodeModel> _models)
        {
            Models = _models;
            ValidateScannedBarcode();
        }

        private void ValidateScannedBarcode()
        {
            PartnerCollection = Models?.FirstOrDefault()?.Kegs?.Partners;
            MultipleKegsTitle = string.Format(" Multiple kgs were found with \n barcode {0}. \n Please select the correct one.", Models.FirstOrDefault().Barcode);
        }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("model"))
            {
                List<BarcodeModel> value = parameters.GetValue<List<BarcodeModel>>("model");
                LoadBarcodeValue(value);
            }
            return base.InitializeAsync(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("CancelCommandRecievierAsync"))
            {
                CancelCommandRecievierAsync();
            }
        }

        #endregion
    }
}
