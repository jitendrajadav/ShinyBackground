using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KegID.Response;
using KegID.Services;
using System.Diagnostics;
using System.Globalization;

namespace KegID.ViewModel
{
    public class DashboardViewModel : ViewModelBase
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

        #endregion

        #region Commands

        public RelayCommand RefreshDashboard { get; set; }

        #endregion

        #region Constructor
        public DashboardViewModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
            RefreshDashboard = new RelayCommand(RefreshDashboardRecieverAsync);
            RefreshDashboardRecieverAsync();
        }
        
        #endregion

        #region Methods
        private async void RefreshDashboardRecieverAsync()
        {
            DashboardModel Result = null;
            try
            {
                Result = await _dashboardService.GetDeshboardDetailAsync(Configuration.SessionId);
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
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                Result = null;
            }
        }
        #endregion
    }
}
