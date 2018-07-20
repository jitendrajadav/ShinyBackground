using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Linq;

namespace KegID.ViewModel
{
    public class FillScanReviewViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;
        private readonly IUuidManager _uuidManager;

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

        public DelegateCommand ScanCommand { get;}
        public DelegateCommand SubmitCommand { get; }

        #endregion

        #region Constructor

        public FillScanReviewViewModel(INavigationService navigationService, IUuidManager uuidManager)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
            _uuidManager = uuidManager;

            ScanCommand = new DelegateCommand(ScanCommandRecieverAsync);
            SubmitCommand = new DelegateCommand(SubmitCommandReciever);
        }

        #endregion

        #region Methods

        private void SubmitCommandReciever()
        {
            try
            {
                try
                {
                    //var param = new NavigationParameters
                    //{
                    //    { "SubmitCommandRecieverAsync", "SubmitCommandRecieverAsync" }
                    //};
                    var formsNav = ((Prism.Common.IPageAware)_navigationService).Page;
                    var page = formsNav.Navigation.ModalStack.Last();
                    (page?.BindingContext as INavigationAware)?.OnNavigatingTo(new NavigationParameters
                    {
                        { "SubmitCommandRecieverAsync", "SubmitCommandRecieverAsync" }
                    });
                }
                catch (Exception)
                {

                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void ScanCommandRecieverAsync()
        {
            await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
        }

        internal void AssignInitialValue(string _manifestId,int _count)
        {
            try
            {
                var partner = ConstantManager.Partner;
                var content = "";//BatchButtonTitle;
                //var content = SimpleIoc.Default.GetInstance<FillViewModel>().BatchButtonTitle;
                TrackingNumber = _uuidManager.GetUuId();
                ManifestTo = partner.FullName + "\n" + partner.PartnerTypeCode;
                ItemCount = _count;
                Contents = !string.IsNullOrEmpty(content) ? content : "No contens";
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("BatchId"))
            {
                AssignInitialValue(parameters.GetValue<string>("BatchId"), parameters.GetValue<int>("Count"));
            }
        }

        #endregion
    }
}
