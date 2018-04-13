using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Model;
using KegID.Views;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class FillViewModel : BaseViewModel
    {
        #region Properties

        #region NewBatchModel

        /// <summary>
        /// The <see cref="NewBatchModel" /> property's name.
        /// </summary>
        public const string NewBatchModelPropertyName = "NewBatchModel";

        private BatchModel _NewBatchModel = new BatchModel();

        /// <summary>
        /// Sets and gets the NewBatchModel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public BatchModel NewBatchModel
        {
            get
            {
                return _NewBatchModel;
            }

            set
            {
                if (_NewBatchModel == value)
                {
                    return;
                }

                _NewBatchModel = value;
                RaisePropertyChanged(NewBatchModelPropertyName);
            }
        }

        #endregion

        #region BatchButtonTitle

        /// <summary>
        /// The <see cref="BatchButtonTitle" /> property's name.
        /// </summary>
        public const string BatchButtonTitlePropertyName = "BatchButtonTitle";

        private string _BatchButtonTitle = "Select batch";

        /// <summary>
        /// Sets and gets the BatchButtonTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string BatchButtonTitle
        {
            get
            {
                return _BatchButtonTitle;
            }

            set
            {
                if (_BatchButtonTitle == value)
                {
                    return;
                }

                _BatchButtonTitle = value;
                RaisePropertyChanged(BatchButtonTitlePropertyName);
            }
        }

        #endregion

        #region SizeButtonTitle

        /// <summary>
        /// The <see cref="SizeButtonTitle" /> property's name.
        /// </summary>
        public const string SizeButtonTitlePropertyName = "SizeButtonTitle";

        private string _SizeButtonTitle = "Select size";

        /// <summary>
        /// Sets and gets the SizeButtonTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string SizeButtonTitle
        {
            get
            {
                return _SizeButtonTitle;
            }

            set
            {
                if (_SizeButtonTitle == value)
                {
                    return;
                }

                _SizeButtonTitle = value;
                RaisePropertyChanged(SizeButtonTitlePropertyName);
            }
        }

        #endregion

        #region DestinationTitle

        /// <summary>
        /// The <see cref="DestinationTitle" /> property's name.
        /// </summary>
        public const string DestinationTitlePropertyName = "DestinationTitle";

        private string _DestinationTitle = "Barcode Brewing";

        /// <summary>
        /// Sets and gets the DestinationTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string DestinationTitle
        {
            get
            {
                return _DestinationTitle;
            }

            set
            {
                if (_DestinationTitle == value)
                {
                    return;
                }

                _DestinationTitle = value;
                RaisePropertyChanged(DestinationTitlePropertyName);
            }
        }

        #endregion

        #region PartnerModel

        /// <summary>
        /// The <see cref="PartnerModel" /> property's name.
        /// </summary>
        public const string PartnerModelPropertyName = "PartnerModel";

        private PartnerModel _PartnerModel = new PartnerModel();

        /// <summary>
        /// Sets and gets the PartnerModel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public PartnerModel PartnerModel
        {
            get
            {
                return _PartnerModel;
            }

            set
            {
                if (_PartnerModel == value)
                {
                    return;
                }

                _PartnerModel = value;
                DestinationTitle = _PartnerModel.FullName;
                RaisePropertyChanged(PartnerModelPropertyName);
            }
        }

        #endregion

        #region IsPalletze

        /// <summary>
        /// The <see cref="IsPalletze" /> property's name.
        /// </summary>
        public const string IsPalletzePropertyName = "IsPalletze";

        private bool _IsPalletze = true;

        /// <summary>
        /// Sets and gets the IsPalletze property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsPalletze
        {
            get
            {
                return _IsPalletze;
            }

            set
            {
                if (_IsPalletze == value)
                {
                    return;
                }

                _IsPalletze = value;
                RaisePropertyChanged(IsPalletzePropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public RelayCommand BatchCommand { get; }
        public RelayCommand SizeCommand { get;}
        public RelayCommand DestinationCommand { get; }
        public RelayCommand NextCommand { get; }
        public RelayCommand CancelCommand { get; }

        #endregion

        #region Constructor

        public FillViewModel()
        {
            BatchCommand = new RelayCommand(BatchCommandRecieverAsync);
            SizeCommand = new RelayCommand(SizeCommandRecieverAsync);
            DestinationCommand = new RelayCommand(DestinationCommandRecieverAsync);
            NextCommand = new RelayCommand(NextCommandRecieverAsync);
            CancelCommand = new RelayCommand(CancelCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void CancelCommandRecieverAsync()
        {
            var result = await Application.Current.MainPage.DisplayActionSheet("Cancel? \n You have like to save this manifest as a draft or delete?", null, null, "Delete manifest", "Save as draft");
            if (result == "Delete manifest")
            {
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }

        private async void NextCommandRecieverAsync()
        {
            if (!BatchButtonTitle.Contains("Select batch"))
            {
                if (IsPalletze)
                {
                    SimpleIoc.Default.GetInstance<AddPalletsViewModel>().AddPalletsTitle = "Filling " + SizeButtonTitle + " kegs with " + BatchButtonTitle + "\n" + DestinationTitle;
                    await Application.Current.MainPage.Navigation.PushModalAsync(new AddPalletsView());
                }
                else
                {
                    SimpleIoc.Default.GetInstance<FillScanViewModel>().IsPalletze = IsPalletze;
                    SimpleIoc.Default.GetInstance<FillScanViewModel>().ManifestId = "Filling " + SizeButtonTitle + " kegs with " + BatchButtonTitle + "\n" + DestinationTitle;
                    await Application.Current.MainPage.Navigation.PushModalAsync(new FillScanView());
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Batch is required.", "Ok");
            }
        }

        private async void DestinationCommandRecieverAsync()
        {
            SimpleIoc.Default.GetInstance<PartnersViewModel>().BrewerStockOn = true;
            await Application.Current.MainPage.Navigation.PushModalAsync(new PartnersView());
        }

        private async void BatchCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PushModalAsync(new BatchView());

        private async void SizeCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PushModalAsync(new SizeView());

        #endregion
    }
}
