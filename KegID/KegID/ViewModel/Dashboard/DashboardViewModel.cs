using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using KegID.SQLiteClient;
using KegID.Views;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Diagnostics;
using System.Globalization;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class DashboardViewModel : BaseViewModel
    {
        #region Properties

        public IDashboardService _dashboardService { get; set; }

        #region Stock

        /// <summary>
        /// The <see cref="Stock" /> property's name.
        /// </summary>
        public const string StockPropertyName = "Stock";

        private string _Stock = "0";

        /// <summary>
        /// Sets and gets the Stock property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Stock
        {
            get
            {
                return _Stock;
            }

            set
            {
                if (_Stock == value)
                {
                    return;
                }

                _Stock = value;
                RaisePropertyChanged(StockPropertyName);
            }
        }

        #endregion

        #region Empty

        /// <summary>
        /// The <see cref="Empty" /> property's name.
        /// </summary>
        public const string EmptyPropertyName = "Empty";

        private string _Empty = "0";

        /// <summary>
        /// Sets and gets the Empty property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Empty
        {
            get
            {
                return _Empty;
            }

            set
            {
                if (_Empty == value)
                {
                    return;
                }

                _Empty = value;
                RaisePropertyChanged(EmptyPropertyName);
            }
        }

        #endregion

        #region InUse

        /// <summary>
        /// The <see cref="InUse" /> property's name.
        /// </summary>
        public const string InUsePropertyName = "InUse";

        private string _InUse = "0";

        /// <summary>
        /// Sets and gets the InUse property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string InUse
        {
            get
            {
                return _InUse;
            }

            set
            {
                if (_InUse == value)
                {
                    return;
                }

                _InUse = value;
                RaisePropertyChanged(InUsePropertyName);
            }
        }

        #endregion

        #region Total

        /// <summary>
        /// The <see cref="Total" /> property's name.
        /// </summary>
        public const string TotalPropertyName = "Total";

        private string _Total = "0";

        /// <summary>
        /// Sets and gets the Total property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Total
        {
            get
            {
                return _Total;
            }

            set
            {
                if (_Total == value)
                {
                    return;
                }

                _Total = value;
                RaisePropertyChanged(TotalPropertyName);
            }
        }

        #endregion

        #region AverageCycle

        /// <summary>
        /// The <see cref="AverageCycle" /> property's name.
        /// </summary>
        public const string AverageCyclePropertyName = "AverageCycle";

        private string _AverageCycle = "0 day";

        /// <summary>
        /// Sets and gets the AverageCycle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string AverageCycle
        {
            get
            {
                return _AverageCycle;
            }

            set
            {
                if (_AverageCycle == value)
                {
                    return;
                }

                _AverageCycle = value;
                RaisePropertyChanged(AverageCyclePropertyName);
            }
        }

        #endregion

        #region Atriskegs

        /// <summary>
        /// The <see cref="Atriskegs" /> property's name.
        /// </summary>
        public const string AtriskegsPropertyName = "Atriskegs";

        private string _Atriskegs = "0";

        /// <summary>
        /// Sets and gets the Atriskegs property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Atriskegs
        {
            get
            {
                return _Atriskegs;
            }

            set
            {
                if (_Atriskegs == value)
                {
                    return;
                }

                _Atriskegs = value;
                RaisePropertyChanged(AtriskegsPropertyName);
            }
        }

        #endregion

        #region DraftmaniFests

        /// <summary>
        /// The <see cref="DraftmaniFests" /> property's name.
        /// </summary>
        public const string DraftmaniFestsPropertyName = "DraftmaniFests";

        private int _DraftmaniFests = default(int);

        /// <summary>
        /// Sets and gets the DraftmaniFests property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int DraftmaniFests
        {
            get
            {
                return _DraftmaniFests;
            }

            set
            {
                if (_DraftmaniFests == value)
                {
                    return;
                }

                _DraftmaniFests = value;
                RaisePropertyChanged(DraftmaniFestsPropertyName);
            }
        }

        #endregion

        #region BgImage

        /// <summary>
        /// The <see cref="BgImage" /> property's name.
        /// </summary>
        public const string BgImagePropertyName = "BgImage";

        private string _BgImage = "Assets/kegbg.png";

        /// <summary>
        /// Sets and gets the BgImage property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string BgImage
        {
            get
            {
                return _BgImage;
            }

            set
            {
                if (_BgImage == value)
                {
                    return;
                }

                _BgImage = value;
                RaisePropertyChanged(BgImagePropertyName);
            }
        }

        #endregion

        #region IsVisibleDraftmaniFestsLabel

        /// <summary>
        /// The <see cref="IsVisibleDraftmaniFestsLabel" /> property's name.
        /// </summary>
        public const string IsVisibleDraftmaniFestsLabelPropertyName = "IsVisibleDraftmaniFestsLabel";

        private bool _IsVisibleDraftmaniFestsLabel = false;

        /// <summary>
        /// Sets and gets the IsVisibleDraftmaniFestsLabel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsVisibleDraftmaniFestsLabel
        {
            get
            {
                return _IsVisibleDraftmaniFestsLabel;
            }

            set
            {
                if (_IsVisibleDraftmaniFestsLabel == value)
                {
                    return;
                }

                _IsVisibleDraftmaniFestsLabel = value;
                RaisePropertyChanged(IsVisibleDraftmaniFestsLabelPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public RelayCommand MoreCommand { get;}
        public RelayCommand MaintainCommand { get; }
        public RelayCommand PalletizeCommand { get; }
        public RelayCommand PalletsCommand { get; }
        public RelayCommand FillCommand { get; }
        public RelayCommand ManifestCommand { get; }
        public RelayCommand StockCommand { get; }
        public RelayCommand EmptyCommand { get; }
        public RelayCommand PartnerCommand { get; }
        public RelayCommand KegsCommand { get; }
        public RelayCommand InUsePartnerCommand { get; }
        public RelayCommand MoveCommand { get; }
        
        #endregion

        #region Constructor

        public DashboardViewModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
            BgImage = GetIconByPlatform.GetIcon("kegbg.png");

            MoveCommand = new RelayCommand(MoveCommandRecieverAsync);
            MoreCommand = new RelayCommand(MoreCommandRecieverAsync);
            MaintainCommand = new RelayCommand(MaintainCommandRecieverAsync);
            PalletizeCommand = new RelayCommand(PalletizeCommandRecieverAsync);
            PalletsCommand = new RelayCommand(PalletsCommandRecieverAsync);
            FillCommand = new RelayCommand(FillCommandRecieverAsync);
            ManifestCommand = new RelayCommand(ManifestCommandRecieverAsync);
            StockCommand = new RelayCommand(StockCommandRecieverAsync);
            EmptyCommand = new RelayCommand(EmptyCommandRecieverAsync);
            PartnerCommand = new RelayCommand(PartnerCommandRecieverAsync);
            KegsCommand = new RelayCommand(KegsCommandRecieverAsync);
            InUsePartnerCommand = new RelayCommand(InUsePartnerCommandRecieverAsync);
            RefreshDashboardRecieverAsync();
            CheckDraftmaniFestsAsync();
        }

        #endregion

        #region Methods

        private async void MoveCommandRecieverAsync()
        {
            SimpleIoc.Default.GetInstance<MoveViewModel>().ManifestId = Uuid.GetUuId();
            await Application.Current.MainPage.Navigation.PushModalAsync(new MoveView(), animated: false);
        }

        private async void InUsePartnerCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new DashboardPartnersView(), animated: false);
        }

        private async void KegsCommandRecieverAsync()
        {
          await Application.Current.MainPage.Navigation.PushModalAsync(new KegSearchView(), animated: false);
        }

        private async void PartnerCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new DashboardPartnersView(), animated: false);
        }

        private async void StockCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new InventoryView(), animated: false);
        }

        private async void EmptyCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new InventoryView(), animated: false);
        }

        private async void ManifestCommandRecieverAsync()
        {
            //SimpleIoc.Default.GetInstance<MoveViewModel>().ManifestId = Uuid.GetUuId();
            await Application.Current.MainPage.Navigation.PushModalAsync(new ManifestsView(), animated: false);
            await SimpleIoc.Default.GetInstance<ManifestsViewModel>().LoadDraftManifestAsync();
            //CheckDraftmaniFests();
        }

        internal async void CheckDraftmaniFestsAsync()
        {
            var collection = await SQLiteServiceClient.Db.Table<DraftManifestModel>().ToListAsync();
            if (collection.Count > 0)
            {
                DraftmaniFests = collection.Count;
                IsVisibleDraftmaniFestsLabel = true;
            }
        }

        private async void FillCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PushModalAsync(new FillView(), animated: false);

        private async void PalletizeCommandRecieverAsync()
        {
            SimpleIoc.Default.GetInstance<PalletizeViewModel>().GenerateManifestIdAsync(null);
            await Application.Current.MainPage.Navigation.PushModalAsync(new PalletizeView(), animated: false);
        }

        private async void PalletsCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new SearchPalletView(), animated: false);
        }

        private async void MaintainCommandRecieverAsync()
        {
            await SimpleIoc.Default.GetInstance<MaintainViewModel>().LoadMaintenanceTypeAsync();
            await Application.Current.MainPage.Navigation.PushModalAsync(new MaintainView(), animated: false);
        }

        public async void RefreshDashboardRecieverAsync(bool refresh = false)
        {
            DashboardResponseModel Result = null;
            try
            {
                if (refresh)
                    await Application.Current.MainPage.Navigation.PopPopupAsync();

                Result = await _dashboardService.GetDeshboardDetailAsync(AppSettings.User.SessionId);
                Stock = Result.Stock.ToString("0,0", CultureInfo.InvariantCulture);
                //Stock= System.String.Format(CultureInfo.InvariantCulture,
                //                 "{0:0,0}", Result.Stock.ToString("0,0", CultureInfo.InvariantCulture));
                Empty = Result.Empty.ToString("0,0", CultureInfo.InvariantCulture);
                InUse = Result.InUse.ToString("0,0", CultureInfo.InvariantCulture);
                var total  = Result.Stock+ Result.Empty+ Result.InUse;
                Total = total.ToString("0,0", CultureInfo.InvariantCulture);
                AverageCycle = Result.AverageCycle.ToString() + " days";
                Atriskegs = Result.InactiveKegs.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                Result = null;
            }
        }

        private async void MoreCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushPopupAsync(new SettingView());
        }

        #endregion
    }
}
