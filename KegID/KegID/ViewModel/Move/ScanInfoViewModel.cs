using KegID.Model;
using Prism.Commands;
using Prism.Navigation;
using System.Linq;
using System.Threading.Tasks;

namespace KegID.ViewModel
{
    public class ScanInfoViewModel : BaseViewModel
    {
        #region Properties

        public string Barcode { get; set; } = "Barcode";
        public string AltBarcode { get; set; }
        public string Ownername { get; set; }
        public string Size { get; set; }
        public string Contents { get; set; }
        public string Batch { get; set; }
        public string Location { get; set; }

        #endregion

        #region Commands

        public DelegateCommand DoneCommand { get; }

        #endregion

        #region Constructor

        public ScanInfoViewModel(INavigationService navigationService) : base(navigationService)
        {
            DoneCommand = new DelegateCommand(DoneCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void DoneCommandRecieverAsync()
        {
            await _navigationService.GoBackAsync(animated: false);
        }

        internal void AssignInitialValue(BarcodeModel _barcode)
        {
            Barcode = string.Format(" Barcode {0} ", _barcode.Barcode);
            //AltBarcode = _barcode.Barcode;
            Ownername = _barcode?.Kegs?.Partners?.FirstOrDefault()?.FullName;
            Size = _barcode?.Tags[3]?.Value;
            Contents = _barcode.Contents;
            Batch = _barcode.Kegs.Batches.FirstOrDefault();
            Location = _barcode.Kegs.Locations.FirstOrDefault().Name;
        }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("model"))
            {
                AssignInitialValue(parameters.GetValue<BarcodeModel>("model"));
            }
            return base.InitializeAsync(parameters);
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
