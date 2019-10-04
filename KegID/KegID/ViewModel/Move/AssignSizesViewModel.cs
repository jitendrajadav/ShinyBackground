using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using KegID.LocalDb;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Realms;

namespace KegID.ViewModel
{
    public class AssignSizesViewModel : BaseViewModel
    {
        #region Properties

        public ObservableCollection<MoveMaintenanceAlertModel> MaintenaceCollection { get; set; }
        public IList<AssetTypeModel> TypeCollection { get; set; }
        public AssetTypeModel SelectedType { get; set; }
        public IList<AssetSizeModel> SizeCollection { get; set; }
        public AssetSizeModel SelectedSize { get; set; }
        public IList<OwnerModel> OwnerCollection { get; set; }
        public OwnerModel SelectedOwner { get; set; }

        #endregion

        #region Commands

        public DelegateCommand ApplyToAllCommand { get; }
        public DelegateCommand DoneCommand { get; }

        #endregion

        #region Constructor

        public AssignSizesViewModel(INavigationService navigationService) : base(navigationService)
        {
            ApplyToAllCommand = new DelegateCommand(ApplyToAllCommandReciever);
            DoneCommand = new DelegateCommand(DoneCommandRecieverAsync);
            MaintenaceCollection = new ObservableCollection<MoveMaintenanceAlertModel>();
        }

        #endregion

        #region Methods

        private async void DoneCommandRecieverAsync()
        {
            try
            {
                await _navigationService.GoBackAsync(new NavigationParameters
                        {
                            { "AssignSizesValue", ConstantManager.VerifiedBarcodes }
                        }, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void ApplyToAllCommandReciever()
        {
            try
            {
                if (SelectedType != null)
                {
                    foreach (var item in MaintenaceCollection)
                    {
                        item.SelectedUType = SelectedType;
                    }
                }
                if (SelectedSize != null)
                {
                    foreach (var item in MaintenaceCollection)
                    {
                        item.SelectedUSize = SelectedSize;
                    }
                }
                if (SelectedOwner != null)
                {
                    foreach (var item in MaintenaceCollection)
                    {
                        item.SelectedUOwner = SelectedOwner;
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void LoadOwnderAsync()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                OwnerCollection = RealmDb.All<OwnerModel>().ToList();
                SelectedOwner = OwnerCollection.OrderBy(x => x.FullName).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void LoadAssetSizeAsync()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                SizeCollection = RealmDb.All<AssetSizeModel>().ToList(); 
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void LoadAssetTypeAsync()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                TypeCollection = RealmDb.All<AssetTypeModel>().ToList(); 
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal void AssignInitialValueAsync(List<BarcodeModel> _alerts)
        {
            try
            {
                ConstantManager.VerifiedBarcodes = _alerts;

                LoadOwnderAsync();
                LoadAssetSizeAsync();
                LoadAssetTypeAsync();
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());

                foreach (var item in _alerts)
                {
                    AssetTypeModel selectedType = null;
                    AssetSizeModel selectedSize = null;
                    OwnerModel selectedOwner = OwnerCollection.Where(x => x.FullName == item?.Kegs?.Partners?.FirstOrDefault()?.FullName).FirstOrDefault();

                    if (selectedOwner!= null)
                    {
                        RealmDb.Write(() =>
                        {
                            selectedOwner.HasInitial = true;
                        });
                    }
                    if (item.Tags.Count > 2)
                    {
                        selectedType = TypeCollection.Where(x => x.AssetType == item.Tags?[2]?.Value).FirstOrDefault();

                        RealmDb.Write(() =>
                        {
                            selectedType.HasInitial = true;
                        }); 
                    }
                    if (item.Tags.Count > 3)
                    {
                        selectedSize = SizeCollection.Where(x => x.AssetSize == item.Tags?[3]?.Value).FirstOrDefault();
                        RealmDb.Write(() =>
                        {
                            selectedSize.HasInitial = true;
                        });
                    }

                    MaintenaceCollection.Add(
                        new MoveMaintenanceAlertModel
                        {
                            UOwnerCollection = OwnerCollection.ToList(),
                            USizeCollection = SizeCollection.ToList(),
                            UTypeCollection = TypeCollection.ToList(),
                            BarcodeId = item.Barcode,
                            SelectedUOwner = selectedOwner ?? selectedOwner,
                            SelectedUSize = selectedSize ?? selectedSize,
                            SelectedUType = selectedType ?? selectedType
                        });
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("alert"))
            {
                AssignInitialValueAsync(parameters.GetValue<List<BarcodeModel>>("alert"));
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("DoneCommandRecieverAsync"))
            {
                DoneCommandRecieverAsync();
            }
        }

        #endregion
    }
}
