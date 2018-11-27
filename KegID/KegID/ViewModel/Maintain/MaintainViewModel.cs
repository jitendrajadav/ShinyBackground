﻿using KegID.LocalDb;
using KegID.Messages;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class MaintainViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private ManifestModel ManifestModel;

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
                RaisePropertyChanged(PartnerModelPropertyName);
            }
        }

        #endregion

        #region Notes

        /// <summary>
        /// The <see cref="Notes" /> property's name.
        /// </summary>
        public const string NotesPropertyName = "Notes";

        private string _Notes = string.Empty;

        /// <summary>
        /// Sets and gets the Notes property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Notes
        {
            get
            {
                return _Notes;
            }

            set
            {
                if (_Notes == value)
                {
                    return;
                }

                _Notes = value;
                RaisePropertyChanged(NotesPropertyName);
            }
        }

        #endregion

        #region MaintainTypeCollection

        /// <summary>
        /// The <see cref="MaintainTypeCollection" /> property's name.
        /// </summary>
        public const string MaintainTypeCollectionPropertyName = "MaintainTypeCollection";

        private IList<MaintainTypeReponseModel> _maintainTypeCollection = null;

        /// <summary>
        /// Sets and gets the MaintainTypeCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<MaintainTypeReponseModel> MaintainTypeCollection
        {
            get
            {
                return _maintainTypeCollection;
            }

            set
            {
                if (_maintainTypeCollection == value)
                {
                    return;
                }

                _maintainTypeCollection = value;
                RaisePropertyChanged(MaintainTypeCollectionPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand HomeCommand { get; }
        public DelegateCommand NextCommand { get; }
        public DelegateCommand PartnerCommand { get; }
        public DelegateCommand<MaintainTypeReponseModel> ItemTappedCommand { get; }

        #endregion

        #region Constructor

        public MaintainViewModel(INavigationService navigationService, IPageDialogService dialogService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
            _dialogService = dialogService;
            HomeCommand = new DelegateCommand(HomeCommandRecieverAsync);
            PartnerCommand = new DelegateCommand(PartnerCommandRecieverAsync);
            NextCommand = new DelegateCommand(NextCommandRecieverAsync);
            PartnerModel.FullName = "Select a location";
            ItemTappedCommand = new DelegateCommand<MaintainTypeReponseModel>((model) => ItemTappedCommandReciever(model));
            
            LoadMaintenanceTypeAsync();

            HandleUnsubscribeMessages();
            HandleReceivedMessages();
        }

        private void HandleReceivedMessages()
        {
            MessagingCenter.Subscribe<MaintainDTToMaintMsg>(this, "MaintainDTToMaintMsg", message =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Cleanup();
                });
            });
        }

        private void HandleUnsubscribeMessages()
        {
            MessagingCenter.Unsubscribe<MaintainDTToMaintMsg>(this, "MaintainDTToMaintMsg");
        }

        #endregion

        #region Methods

        private void ItemTappedCommandReciever(MaintainTypeReponseModel model)
        {
            model.IsToggled = !model.IsToggled;
        }

        public void LoadMaintenanceTypeAsync()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                var result = RealmDb.All<MaintainTypeReponseModel>().ToList();

                ConstantManager.MaintainTypeCollection = result.Where(x => x.ActivationMethod == "ReverseOnly").OrderBy(x => x.Name).ToList();
                MaintainTypeCollection = ConstantManager.MaintainTypeCollection;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void HomeCommandRecieverAsync()
        {
            await _navigationService.GoBackAsync(animated: false);
        }

        private async void PartnerCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync("PartnersView", animated: false);
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
                var flag = ConstantManager.MaintainTypeCollection.Where(x => x.IsToggled == true);
                if (flag != null)
                {
                    await _navigationService.NavigateAsync("MaintainScanView",
                        new NavigationParameters
                        {
                            { "Notes", Notes },
                            { "PartnerModel", PartnerModel},
                            { "ManifestModel", ManifestModel }
                        }, animated: false);
                }
                else
                    await _dialogService.DisplayAlertAsync("Error", "Please select at least one maintenance item to perform.", "Ok");
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("MaintainHome"))
            {
                await _navigationService.GoBackAsync(animated: false);
            }
            if (parameters.ContainsKey("HomeCommandRecieverAsync"))
            {
                HomeCommandRecieverAsync();
            }
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            switch (parameters.Keys.FirstOrDefault())
            {
                case "model":
                    PartnerModel = parameters.GetValue<PartnerModel>("model");
                    break;
                case "AssignInitialValue":
                    AssignInitialValue(parameters.GetValue<ManifestModel>("AssignInitialValue"));
                    break;
                case "Cleanup":
                    Cleanup();
                    break;
                default:
                    break;
            }
        }

        private void AssignInitialValue(ManifestModel manifestModel)
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                var maintenance = RealmDb.All<MaintainTypeReponseModel>().ToList();

                ManifestModel = manifestModel;
                MaintainTypeCollection = maintenance.Where(x => x.ActivationMethod == "ReverseOnly").OrderBy(x => x.Name).ToList();
                foreach (var item in manifestModel.MaintenanceModels.MaintenanceDoneRequestModel.ActionsPerformed)
                {
                    var result = maintenance.Find(x => x.Id == item);
                    if (result != null)
                    {
                        MaintainTypeCollection.Where(x=>x.Id == result.Id).FirstOrDefault().IsToggled = true;
                    }
                }
                Notes = manifestModel?.MaintenanceModels?.MaintenanceDoneRequestModel?.Notes;
                PartnerModel = manifestModel?.MaintenanceModels?.MaintenanceDoneRequestModel?.PartnerModel;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void Cleanup()
        {
            try
            {
                //using (var trans = Realm.GetInstance(RealmDbManager.GetRealmDbConfig()).BeginWrite())
                //{
                PartnerModel.FullName = "Select a location";
                //trans.Commit();
                //}
                LoadMaintenanceTypeAsync();
                Notes = string.Empty;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        #endregion
    }
}
