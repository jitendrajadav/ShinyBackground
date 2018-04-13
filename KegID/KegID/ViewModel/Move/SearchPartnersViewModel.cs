using System;
using System.Collections.Generic;
using System.Diagnostics;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class SearchPartnersViewModel : BaseViewModel
    {
        #region Properties

        public IMoveService _moveService { get; set; }

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

        public RelayCommand BackPartnersCommand { get; }
        public RelayCommand PartnerSearchCommand { get; }
        public RelayCommand<PartnerModel> ItemTappedCommand { get; }

        #endregion

        #region Contructor

        public SearchPartnersViewModel(IMoveService moveService)
        {
            _moveService = moveService;

            BackPartners = "< Partners";
            BackPartnersCommand = new RelayCommand(BackPartnersCommandRecieverAsync);
            PartnerSearchCommand = new RelayCommand(PartnerSearchCommandRecieverAsync);
            ItemTappedCommand = new RelayCommand<PartnerModel>((model) => ItemTappedCommandRecieverAsync(model));
        }

        #endregion

        #region Methods

        private async void PartnerSearchCommandRecieverAsync()
        {
            try
            {
                Loader.StartLoading();
                var value = await _moveService.GetPartnerSearchAsync(AppSettings.User.SessionId, PartnerSearch, false, false);

                if (value.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    PartnerSearchCollection = value.PartnerModel;
                }
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                Loader.StopLoading();
            }
        }

        private async void BackPartnersCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PopModalAsync();

        private async void ItemTappedCommandRecieverAsync(PartnerModel model)
        {
            if (model != null)
            {
                SimpleIoc.Default.GetInstance<MoveViewModel>().PartnerModel = model;
                await Application.Current.MainPage.Navigation.PopModalAsync();
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }

        #endregion
    }
}
