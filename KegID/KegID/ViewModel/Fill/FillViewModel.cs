using KegID.Messages;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class FillViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;

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
                BatchButtonTitle = _NewBatchModel.BrandName + "-" + _NewBatchModel.BatchCode;
                IsRequiredVisible = false;
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

        #region IsRequiredVisible

        /// <summary>
        /// The <see cref="IsRequiredVisible" /> property's name.
        /// </summary>
        public const string IsRequiredVisiblePropertyName = "IsRequiredVisible";

        private bool _IsRequiredVisible = true;

        /// <summary>
        /// Sets and gets the IsRequiredVisible property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsRequiredVisible
        {
            get
            {
                return _IsRequiredVisible;
            }

            set
            {
                if (_IsRequiredVisible == value)
                {
                    return;
                }

                _IsRequiredVisible = value;
                RaisePropertyChanged(IsRequiredVisiblePropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand BatchCommand { get; }
        public DelegateCommand SizeCommand { get;}
        public DelegateCommand DestinationCommand { get; }
        public DelegateCommand NextCommand { get; }
        public DelegateCommand CancelCommand { get; }

        #endregion

        #region Constructor

        public FillViewModel(INavigationService navigationService, IPageDialogService dialogService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
            _dialogService = dialogService;
            BatchCommand = new DelegateCommand(BatchCommandRecieverAsync);
            SizeCommand = new DelegateCommand(SizeCommandRecieverAsync);
            DestinationCommand = new DelegateCommand(DestinationCommandRecieverAsync);
            NextCommand = new DelegateCommand(NextCommandRecieverAsync);
            CancelCommand = new DelegateCommand(CancelCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void CancelCommandRecieverAsync()
        {
            try
            {
                var result = await _dialogService.DisplayActionSheetAsync("Cancel? \n You have like to save this manifest as a draft or delete?", null, null, "Delete manifest", "Save as draft");
                if (result == "Delete manifest")
                {
                    await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
                }
                else
                {
                    //Save Draft Manifest logic here...
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void NextCommandRecieverAsync()
        {
            try
            {
                FillToFillScanPagesMsg msg = new FillToFillScanPagesMsg
                {
                    BatchModel = NewBatchModel,
                    PartnerModel = PartnerModel,
                    SizeButtonTitle = SizeButtonTitle
                };
                MessagingCenter.Send(msg, "FillToFillScanPagesMsg");

                if (!BatchButtonTitle.Contains("Select batch"))
                {
                    if (IsPalletze)
                    {
                        await _navigationService.NavigateAsync(new Uri("AddPalletsView", UriKind.Relative), new NavigationParameters
                        {
                            { "AddPalletsTitle", "Filling " + SizeButtonTitle + " kegs with " + BatchButtonTitle + "\n" + DestinationTitle }
                        }, useModalNavigation: true, animated: false);
                    }
                    else
                    {
                        await _navigationService.NavigateAsync(new Uri("FillScanView", UriKind.Relative), new NavigationParameters
                        {
                            { "IsPalletze",IsPalletze},{ "ManifestId","Filling " + SizeButtonTitle + " kegs with " + BatchButtonTitle + " " + DestinationTitle},
                        }, useModalNavigation: true, animated: false);
                    }
                }
                else
                {
                    await _dialogService.DisplayAlertAsync("Error", "Batch is required.", "Ok");
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void DestinationCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync(new Uri("PartnersView", UriKind.Relative), new NavigationParameters
                    {
                        { "BrewerStockOn", true }
                    }, useModalNavigation: true, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void BatchCommandRecieverAsync()
        {
            await _navigationService.NavigateAsync(new Uri("BatchView", UriKind.Relative), useModalNavigation: true, animated: false);
        }

        private async void SizeCommandRecieverAsync()
        {
            await _navigationService.NavigateAsync(new Uri("SizeView", UriKind.Relative), useModalNavigation: true, animated: false);
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("MoveHome"))
            {
                await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
            }
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("model"))
            {
                PartnerModel = parameters.GetValue<PartnerModel>("model");
                ConstantManager.Partner = PartnerModel;
            }
            if (parameters.ContainsKey("BatchModel"))
            {
                NewBatchModel = parameters.GetValue<BatchModel>("BatchModel");
            }
            if (parameters.ContainsKey("SizeModel"))
            {
                SizeButtonTitle = parameters.GetValue<string>("SizeModel");
            }
            if (parameters.ContainsKey("NewBatchModel"))
            {
                NewBatchModel = parameters.GetValue<BatchModel>("NewBatchModel");
            }
        }
        
        #endregion
    }
}
