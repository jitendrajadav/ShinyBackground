using System.Collections.ObjectModel;
using KegID.Services;
using System.Linq;
using System.Collections.Generic;
using KegID.Model;
using System;
using Microsoft.AppCenter.Crashes;
using Realms;
using KegID.LocalDb;
using Prism.Commands;
using Prism.Navigation;

namespace KegID.ViewModel
{
    public class DashboardPartnersViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;
        private readonly IDashboardService _dashboardService;
        public IList<PossessorResponseModel> AllPartners { get; set; }

        #region IsWorking

        /// <summary>
        /// The <see cref="IsWorking" /> property's name.
        /// </summary>
        public const string IsWorkingPropertyName = "IsWorking";

        private bool _IsWorking = false;

        /// <summary>
        /// Sets and gets the IsWorking property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsWorking
        {
            get
            {
                return _IsWorking;
            }

            set
            {
                if (_IsWorking == value)
                {
                    return;
                }

                _IsWorking = value;
                RaisePropertyChanged(IsWorkingPropertyName);
            }
        }

        #endregion

        #region InternalBackgroundColor

        /// <summary>
        /// The <see cref="InternalBackgroundColor" /> property's name.
        /// </summary>
        public const string InternalBackgroundColorPropertyName = "InternalBackgroundColor";

        private string _InternalBackgroundColor = "Transparent";

        /// <summary>
        /// Sets and gets the InternalBackgroundColor property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string InternalBackgroundColor
        {
            get
            {
                return _InternalBackgroundColor;
            }

            set
            {
                if (_InternalBackgroundColor == value)
                {
                    return;
                }

                _InternalBackgroundColor = value;
                RaisePropertyChanged(InternalBackgroundColorPropertyName);
            }
        }

        #endregion

        #region InternalTextColor

        /// <summary>
        /// The <see cref="InternalTextColor" /> property's name.
        /// </summary>
        public const string InternalTextColorPropertyName = "InternalTextColor";

        private string _InternalTextColor = "#4E6388";

        /// <summary>
        /// Sets and gets the InternalTextColor property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string InternalTextColor
        {
            get
            {
                return _InternalTextColor;
            }

            set
            {
                if (_InternalTextColor == value)
                {
                    return;
                }

                _InternalTextColor = value;
                RaisePropertyChanged(InternalTextColorPropertyName);
            }
        }

        #endregion

        #region AlphabeticalBackgroundColor

        /// <summary>
        /// The <see cref="AlphabeticalBackgroundColor" /> property's name.
        /// </summary>
        public const string AlphabeticalBackgroundColorPropertyName = "AlphabeticalBackgroundColor";

        private string _AlphabeticalBackgroundColor = "Transparent";

        /// <summary>
        /// Sets and gets the AlphabeticalBackgroundColor property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string AlphabeticalBackgroundColor
        {
            get
            {
                return _AlphabeticalBackgroundColor;
            }

            set
            {
                if (_AlphabeticalBackgroundColor == value)
                {
                    return;
                }

                _AlphabeticalBackgroundColor = value;
                RaisePropertyChanged(AlphabeticalBackgroundColorPropertyName);
            }
        }

        #endregion

        #region AlphabeticalTextColor

        /// <summary>
        /// The <see cref="AlphabeticalTextColor" /> property's name.
        /// </summary>
        public const string AlphabeticalTextColorPropertyName = "AlphabeticalTextColor";

        private string _AlphabeticalTextColor = "#4E6388";

        /// <summary>
        /// Sets and gets the AlphabeticalTextColor property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string AlphabeticalTextColor
        {
            get
            {
                return _AlphabeticalTextColor;
            }

            set
            {
                if (_AlphabeticalTextColor == value)
                {
                    return;
                }

                _AlphabeticalTextColor = value;
                RaisePropertyChanged(AlphabeticalTextColorPropertyName);
            }
        }

        #endregion

        #region KegsHeldBackgroundColor

        /// <summary>
        /// The <see cref="KegsHeldBackgroundColor" /> property's name.
        /// </summary>
        public const string KegsHeldBackgroundColorPropertyName = "KegsHeldBackgroundColor";

        private string _KegsHeldBackgroundColor = "#4E6388";

        /// <summary>
        /// Sets and gets the KegsHeldBackgroundColor property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string KegsHeldBackgroundColor
        {
            get
            {
                return _KegsHeldBackgroundColor;
            }

            set
            {
                if (_KegsHeldBackgroundColor == value)
                {
                    return;
                }

                _KegsHeldBackgroundColor = value;
                RaisePropertyChanged(KegsHeldBackgroundColorPropertyName);
            }
        }

        #endregion

        #region KegsHeldTextColor

        /// <summary>
        /// The <see cref="KegsHeldTextColor" /> property's name.
        /// </summary>
        public const string KegsHeldTextColorPropertyName = "KegsHeldTextColor";

        private string _KegsHeldTextColor = "White";

        /// <summary>
        /// Sets and gets the KegsHeldTextColor property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string KegsHeldTextColor
        {
            get
            {
                return _KegsHeldTextColor;
            }

            set
            {
                if (_KegsHeldTextColor == value)
                {
                    return;
                }

                _KegsHeldTextColor = value;
                RaisePropertyChanged(KegsHeldTextColorPropertyName);
            }
        }

        #endregion

        #region PartnerCollection

        /// <summary>
        /// The <see cref="PartnerCollection" /> property's name.
        /// </summary>
        public const string PartnerCollectionPropertyName = "PartnerCollection";

        private ObservableCollection<PossessorResponseModel> _PartnerCollection = null;

        /// <summary>
        /// Sets and gets the PartnerCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<PossessorResponseModel> PartnerCollection
        {
            get
            {
                return _PartnerCollection;
            }

            set
            {
                if (_PartnerCollection == value)
                {
                    return;
                }

                _PartnerCollection = value;
                RaisePropertyChanged(PartnerCollectionPropertyName);
            }
        }

        #endregion

        #region PartnerName

        /// <summary>
        /// The <see cref="PartnerName" /> property's name.
        /// </summary>
        public const string PartnerNamePropertyName = "PartnerName";

        private string _PartnerName = default(string);

        /// <summary>
        /// Sets and gets the PartnerName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string PartnerName
        {
            get
            {
                return _PartnerName;
            }

            set
            {
                if (_PartnerName == value)
                {
                    return;
                }

                _PartnerName = value;
                RaisePropertyChanged(PartnerNamePropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand InternalCommand { get;}
        public DelegateCommand AlphabeticalCommand { get; }
        public DelegateCommand<PossessorResponseModel> ItemTappedCommand { get;}
        public DelegateCommand AddNewPartnerCommand { get; }
        public DelegateCommand BackCommand { get; }
        public DelegateCommand TextChangedCommand { get;}
        public DelegateCommand KegsHeldCommand { get;}
        
        #endregion

        #region Constructor

        public DashboardPartnersViewModel(IDashboardService dashboardService, INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            _dashboardService = dashboardService;

            InternalCommand = new DelegateCommand(InternalCommandReciever);
            AlphabeticalCommand = new DelegateCommand(AlphabeticalCommandReciever);
            ItemTappedCommand = new DelegateCommand<PossessorResponseModel>((model) => ItemTappedCommandRecieverAsync(model));
            AddNewPartnerCommand = new DelegateCommand(AddNewPartnerCommandRecieverAsync);
            BackCommand = new DelegateCommand(BackCommandRecieverAsync);
            TextChangedCommand = new DelegateCommand(TextChangedCommandRecieverAsync);
            KegsHeldCommand = new DelegateCommand(KegsHeldCommandReciever);

            InitialSetting();
            LoadPartners();
        }

        #endregion

        #region Methods

        private void LoadPartners()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            AllPartners = RealmDb.All<PossessorResponseModel>().ToList();
            try
            {
                if (AllPartners.Count > 0)
                    PartnerCollection = new ObservableCollection<PossessorResponseModel>(AllPartners.OrderByDescending(x => x.KegsHeld));
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void KegsHeldCommandReciever()
        {
            try
            {
                InitialSetting();
                PartnerCollection = new ObservableCollection<PossessorResponseModel>(AllPartners.OrderByDescending(x => x.KegsHeld));
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void InitialSetting()
        {
            try
            {
                KegsHeldBackgroundColor = "#4E6388";
                KegsHeldTextColor = "White";

                AlphabeticalBackgroundColor = "White";
                AlphabeticalTextColor = "#4E6388";

                InternalBackgroundColor = "White";
                InternalTextColor = "#4E6388";
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void TextChangedCommandRecieverAsync()
        {
            try
            {
                var result = AllPartners.Where(x => x.Location.FullName.ToLower().Contains(PartnerName.ToLower()));
                PartnerCollection = new ObservableCollection<PossessorResponseModel>(result);
            }
            catch (Exception ex)
            {
                 Crashes.TrackError(ex);
            }
        }

        private async void ItemTappedCommandRecieverAsync(PossessorResponseModel model)
        {
            try
            {
                if (model != null)
                {
                    try
                    {
                        ConstantManager.DBPartnerId = model.Location.PartnerId;
                    }
                    catch (Exception)
                    {
                    }
                    await _navigationService.NavigateAsync("PartnerInfoView", new NavigationParameters
                    {
                        { "PartnerModel", model }
                    }, animated: false);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void AlphabeticalCommandReciever()
        {
            try
            {
                AlphabeticalBackgroundColor = "#4E6388";
                AlphabeticalTextColor = "White";

                InternalBackgroundColor = "White";
                InternalTextColor = "#4E6388";

                KegsHeldBackgroundColor = "White";
                KegsHeldTextColor = "#4E6388";

                PartnerCollection = new ObservableCollection<PossessorResponseModel>(AllPartners.OrderBy(x => x.Location.FullName));
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void InternalCommandReciever()
        {
            try
            {
                InternalBackgroundColor = "#4E6388";
                InternalTextColor = "White";

                AlphabeticalBackgroundColor = "White";
                AlphabeticalTextColor = "#4E6388";

                KegsHeldBackgroundColor = "White";
                KegsHeldTextColor = "#4E6388";

                PartnerCollection = new ObservableCollection<PossessorResponseModel>(AllPartners);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void BackCommandRecieverAsync()
        {
            await _navigationService.GoBackAsync(animated: false);
        }

        private async void AddNewPartnerCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync("AddPartnerView", animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("BackCommandRecieverAsync"))
            {
                BackCommandRecieverAsync();
            }
        }

        #endregion
    }
}
