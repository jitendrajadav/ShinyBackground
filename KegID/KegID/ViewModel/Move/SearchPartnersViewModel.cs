using System;
using System.Collections.Generic;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;

namespace KegID.ViewModel
{
    public class SearchPartnersViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;
        private readonly IMoveService _moveService;

        #region BackPartners

        /// <summary>
        /// The <see cref="BackPartners" /> property's name.
        /// </summary>
        public const string BackPartnersPropertyName = "BackPartners";

        private string _BackPartners = default(string);

        /// <summary>
        /// Sets and gets the BackPartners property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string BackPartners
        {
            get
            {
                return _BackPartners;
            }

            set
            {
                if (_BackPartners == value)
                {
                    return;
                }

                _BackPartners = value;
                RaisePropertyChanged(BackPartnersPropertyName);
            }
        }

        #endregion

        #region PartnerSearch

        /// <summary>
        /// The <see cref="PartnerSearch" /> property's name.
        /// </summary>
        public const string PartnerSearchPropertyName = "PartnerSearch";

        private string _PartnerSearch = string.Empty;

        /// <summary>
        /// Sets and gets the PartnerSearch property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string PartnerSearch
        {
            get
            {
                return _PartnerSearch;
            }

            set
            {
                if (_PartnerSearch == value)
                {
                    return;
                }

                _PartnerSearch = value;
                RaisePropertyChanged(PartnerSearchPropertyName);
            }
        }

        #endregion

        #region PartnerSearchCollection

        /// <summary>
        /// The <see cref="PartnerSearchCollection" /> property's name.
        /// </summary>
        public const string PartnerSearchCollectionPropertyName = "PartnerSearchCollection";

        private IList<PartnerModel> _PartnerSearchCollection = null;

        /// <summary>
        /// Sets and gets the PartnerSearchCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<PartnerModel> PartnerSearchCollection
        {
            get
            {
                return _PartnerSearchCollection;
            }

            set
            {
                if (_PartnerSearchCollection == value)
                {
                    return;
                }

                _PartnerSearchCollection = value;
                RaisePropertyChanged(PartnerSearchCollectionPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand BackPartnersCommand { get; }
        public DelegateCommand PartnerSearchCommand { get; }
        public DelegateCommand<PartnerModel> ItemTappedCommand { get; }

        #endregion

        #region Contructor

        public SearchPartnersViewModel(IMoveService moveService, INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            _moveService = moveService;

            BackPartners = "< Partners";
            BackPartnersCommand = new DelegateCommand(BackPartnersCommandRecieverAsync);
            PartnerSearchCommand = new DelegateCommand(PartnerSearchCommandRecieverAsync);
            ItemTappedCommand = new DelegateCommand<PartnerModel>((model) => ItemTappedCommandRecieverAsync(model));
        }

        #endregion

        #region Methods

        private async void PartnerSearchCommandRecieverAsync()
        {
            try
            {
                Loader.StartLoading();
                var value = await _moveService.GetPartnerSearchAsync(AppSettings.User.SessionId, PartnerSearch, false, false);

                if (value.Response.StatusCode == System.Net.HttpStatusCode.OK.ToString())
                {
                    PartnerSearchCollection = value.PartnerModel;
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

        private async void BackPartnersCommandRecieverAsync()
        {
            await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
        }

        private async void ItemTappedCommandRecieverAsync(PartnerModel model)
        {
            try
            {
                if (model != null)
                {
                    ConstantManager.Partner = model;
                    await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
                    await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        #endregion
    }
}
