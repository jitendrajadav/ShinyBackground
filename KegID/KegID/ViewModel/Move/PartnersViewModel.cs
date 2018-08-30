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
    public class PartnersViewModel : BaseViewModel
    {
        #region Properties

        private readonly IMoveService _moveService;
        private readonly INavigationService _navigationService;
        public bool BrewerStockOn { get; set; }

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

        private string _InternalTextColor = "White";

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

        #region PartnerCollection

        /// <summary>
        /// The <see cref="PartnerCollection" /> property's name.
        /// </summary>
        public const string PartnerCollectionPropertyName = "PartnerCollection";

        private ObservableCollection<PartnerModel> _PartnerCollection = null;

        /// <summary>
        /// Sets and gets the PartnerCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<PartnerModel> PartnerCollection
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

        public IList<PartnerModel> AllPartners { get; set; }

        #endregion

        #region Commands

        public DelegateCommand InternalCommand { get; }
        public DelegateCommand AlphabeticalCommand { get; }
        public DelegateCommand<PartnerModel> ItemTappedCommand { get; }
        public DelegateCommand SearchPartnerCommand { get; }
        public DelegateCommand AddNewPartnerCommand { get; }
        public DelegateCommand BackCommand { get; }
        public DelegateCommand TextChangedCommand { get; }
        
        #endregion

        #region Constructor

        public PartnersViewModel(IMoveService moveService, INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            _moveService = moveService;

            InternalCommand = new DelegateCommand(InternalCommandReciever);
            AlphabeticalCommand = new DelegateCommand(AlphabeticalCommandReciever);
            ItemTappedCommand = new DelegateCommand<PartnerModel>((model) => ItemTappedCommandRecieverAsync(model));
            SearchPartnerCommand = new DelegateCommand(SearchPartnerCommandRecieverAsync);
            AddNewPartnerCommand = new DelegateCommand(AddNewPartnerCommandRecieverAsync);
            BackCommand = new DelegateCommand(BackCommandRecieverAsync);
            TextChangedCommand = new DelegateCommand(TextChangedCommandRecieverAsync);
            InternalBackgroundColor = "#4E6388";
            InternalTextColor = "White";
        }

        #endregion

        #region Methods

        private void TextChangedCommandRecieverAsync()
        {
            try
            {
                var notNullPartners = AllPartners.Where(x => x.FullName != null).ToList();
                var result = notNullPartners.Where(x => x.FullName.ToLower().Contains(PartnerName.ToLower())).ToList();
                PartnerCollection = new ObservableCollection<PartnerModel>(result);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void ItemTappedCommandRecieverAsync(PartnerModel model)
        {
            try
            {
                if (model != null)
                {
                    ConstantManager.Partner = model;
                    await _navigationService.GoBackAsync(new NavigationParameters
                    {
                        { "model", model }
                    }, useModalNavigation: true, animated: false);

                    Cleanup();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public void LoadPartnersAsync()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            AllPartners = RealmDb.All<PartnerModel>().ToList();
            try
            {
                PartnerCollection = null;
                if (AllPartners.Count > 0)
                {
                    if (BrewerStockOn)
                        PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners.Where(x => x.PartnerTypeName == "Brewer - Stock").ToList());
                    else
                        PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                Loader.StopLoading();
            }
        }

        private void AlphabeticalCommandReciever()
        {
            try
            {
                if (BrewerStockOn)
                    PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners.OrderBy(x => x.FullName).Where(x => x.PartnerTypeName == "Brewer - Stock").ToList());
                else
                    PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners.OrderBy(x => x.FullName));

                AlphabeticalBackgroundColor = "#4E6388";
                AlphabeticalTextColor = "White";

                InternalBackgroundColor = "White";
                InternalTextColor = "#4E6388";
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
                if (BrewerStockOn)
                    PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners.Where(x => x.PartnerTypeName == "Brewer - Stock").ToList());
                else
                    PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners);

                InternalBackgroundColor = "#4E6388";
                InternalTextColor = "White";

                AlphabeticalBackgroundColor = "White";
                AlphabeticalTextColor = "#4E6388";
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void BackCommandRecieverAsync()
        {
            try
            {
                await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
                Cleanup();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void AddNewPartnerCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync(new Uri("AddPartnerView", UriKind.Relative), useModalNavigation: true, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void SearchPartnerCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync(new Uri("SearchPartnersView", UriKind.Relative), useModalNavigation: true, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public void Cleanup()
        {
            try
            {
                BrewerStockOn = false;
                PartnerCollection = null;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("GoBackPartner"))
            {
                await _navigationService.GoBackAsync(new NavigationParameters
                    {
                        { "model", ConstantManager.Partner }
                    }, useModalNavigation: true, animated: false);
            }
            if (parameters.ContainsKey("BackCommandRecieverAsync"))
            {
                BackCommandRecieverAsync();
            }
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("BrewerStockOn"))
                BrewerStockOn = true;
            else
                BrewerStockOn = false;
            LoadPartnersAsync();
        }

        #endregion
    }
}
