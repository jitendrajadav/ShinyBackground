using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Model;
using Microsoft.AppCenter.Crashes;
using Realms;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class AssignSizesViewModel : BaseViewModel
    {
        #region Properties

        public List<Barcode> VerifiedBarcodes { get; set; }

        #region MaintenaceCollection

        /// <summary>
        /// The <see cref="MaintenaceCollection" /> property's name.
        /// </summary>
        public const string MaintenaceCollectionPropertyName = "MaintenaceCollection";

        private ObservableCollection<MoveMaintenanceAlertModel> _maintenaceCollection = null;

        /// <summary>
        /// Sets and gets the MaintenaceCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<MoveMaintenanceAlertModel> MaintenaceCollection
        {
            get
            {
                return _maintenaceCollection;
            }

            set
            {
                if (_maintenaceCollection == value)
                {
                    return;
                }

                _maintenaceCollection = value;
                RaisePropertyChanged(MaintenaceCollectionPropertyName);
            }
        }

        #endregion

        #region TypeCollection

        /// <summary>
        /// The <see cref="TypeCollection" /> property's name.
        /// </summary>
        public const string TypeCollectionPropertyName = "TypeCollection";

        private IList<AssetTypeModel> _typeCollection = null;

        /// <summary>
        /// Sets and gets the TypeCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<AssetTypeModel> TypeCollection
        {
            get
            {
                return _typeCollection;
            }

            set
            {
                if (_typeCollection == value)
                {
                    return;
                }

                _typeCollection = value;
                RaisePropertyChanged(TypeCollectionPropertyName);
            }
        }

        #endregion

        #region SelectedType

        /// <summary>
        /// The <see cref="SelectedType" /> property's name.
        /// </summary>
        public const string SelectedTypePropertyName = "SelectedType";

        private AssetTypeModel _selectedType = null;

        /// <summary>
        /// Sets and gets the SelectedType property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public AssetTypeModel SelectedType
        {
            get
            {
                return _selectedType;
            }

            set
            {
                if (_selectedType == value)
                {
                    return;
                }

                _selectedType = value;
                RaisePropertyChanged(SelectedTypePropertyName);
            }
        }

        #endregion

        #region SizeCollection

        /// <summary>
        /// The <see cref="SizeCollection" /> property's name.
        /// </summary>
        public const string SizeCollectionPropertyName = "SizeCollection";

        private IList<AssetSizeModel> _sizeCollection = null;

        /// <summary>
        /// Sets and gets the SizeCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<AssetSizeModel> SizeCollection
        {
            get
            {
                return _sizeCollection;
            }

            set
            {
                if (_sizeCollection == value)
                {
                    return;
                }

                _sizeCollection = value;
                RaisePropertyChanged(SizeCollectionPropertyName);
            }
        }

        #endregion

        #region SelectedSize

        /// <summary>
        /// The <see cref="SelectedSize" /> property's name.
        /// </summary>
        public const string SelectedSizePropertyName = "SelectedSize";

        private AssetSizeModel _selectedSize = null;

        /// <summary>
        /// Sets and gets the SelectedSize property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public AssetSizeModel SelectedSize
        {
            get
            {
                return _selectedSize;
            }

            set
            {
                if (_selectedSize == value)
                {
                    return;
                }

                _selectedSize = value;
                RaisePropertyChanged(SelectedSizePropertyName);
            }
        }

        #endregion

        #region OwnerCollection

        /// <summary>
        /// The <see cref="OwnerCollection" /> property's name.
        /// </summary>
        public const string OwnerCollectionPropertyName = "OwnerCollection";

        private IList<OwnerModel> _ownerCollection = null;

        /// <summary>
        /// Sets and gets the OwnerCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<OwnerModel> OwnerCollection
        {
            get
            {
                return _ownerCollection;
            }

            set
            {
                if (_ownerCollection == value)
                {
                    return;
                }

                _ownerCollection = value;
                RaisePropertyChanged(OwnerCollectionPropertyName);
            }
        }

        #endregion

        #region SelectedOwner

        /// <summary>
        /// The <see cref="SelectedOwner" /> property's name.
        /// </summary>
        public const string SelectedOwnerPropertyName = "SelectedOwner";

        private OwnerModel _selectedOwner = null;

        /// <summary>
        /// Sets and gets the SelectedOwner property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public OwnerModel SelectedOwner
        {
            get
            {
                return _selectedOwner;
            }

            set
            {
                if (_selectedOwner == value)
                {
                    return;
                }

                _selectedOwner = value;
                RaisePropertyChanged(SelectedOwnerPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public RelayCommand ApplyToAllCommand { get; }
        public RelayCommand DoneCommand { get; }

        #endregion

        #region Constructor

        #endregion

        #region Methods

        public AssignSizesViewModel()
        {
            ApplyToAllCommand = new RelayCommand(ApplyToAllCommandReciever);
            DoneCommand = new RelayCommand(DoneCommandReciever);
            MaintenaceCollection = new ObservableCollection<MoveMaintenanceAlertModel>();
        }

        public void MaintenanceVerified()
        {
            try
            {
                VerifiedBarcodes.FirstOrDefault().HasMaintenaceVerified = true;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void DoneCommandReciever()
        {
            try
            {
                SimpleIoc.Default.GetInstance<ScanKegsViewModel>().AssignSizesValue(VerifiedBarcodes);
                Application.Current.MainPage.Navigation.PopModalAsync();
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
                var RealmDb = Realm.GetInstance();
                OwnerCollection = RealmDb.All<OwnerModel>().ToList();//await SQLiteServiceClient.Db.Table<OwnerModel>().ToListAsync();
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
                var RealmDb = Realm.GetInstance();
                SizeCollection = RealmDb.All<AssetSizeModel>().ToList(); //await SQLiteServiceClient.Db.Table<AssetSizeModel>().ToListAsync();
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
                var RealmDb = Realm.GetInstance();
                TypeCollection = RealmDb.All<AssetTypeModel>().ToList(); //await SQLiteServiceClient.Db.Table<AssetTypeModel>().ToListAsync();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal void AssignInitialValueAsync(List<Barcode> _alerts)
        {
            try
            {
                VerifiedBarcodes = _alerts;

                LoadOwnderAsync();
                LoadAssetSizeAsync();
                LoadAssetTypeAsync();

                foreach (var item in _alerts)
                {
                    AssetTypeModel selectedType = null;
                    AssetSizeModel selectedSize = null;
                    OwnerModel selectedOwner = OwnerCollection.Where(x => x.FullName == item?.Partners?.FirstOrDefault()?.FullName).FirstOrDefault();

                    if (selectedOwner!= null)
                    {
                        selectedOwner.HasInitial = true;
                    }
                    if (item.Tags.Count > 2)
                    {
                        selectedType = TypeCollection.Where(x => x.AssetType == item.Tags?[2]?.Value).FirstOrDefault();
                        selectedType.HasInitial = true;
                    }
                    if (item.Tags.Count > 3)
                    {
                        selectedSize = SizeCollection.Where(x => x.AssetSize == item.Tags?[3]?.Value).FirstOrDefault();
                        selectedSize.HasInitial = true;
                    }

                    MaintenaceCollection.Add(
                        new MoveMaintenanceAlertModel
                        {
                            UOwnerCollection = OwnerCollection.ToList(),
                            USizeCollection = SizeCollection.ToList(),
                            UTypeCollection = TypeCollection.ToList(),
                            BarcodeId = item.Id,
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

        #endregion
    }
}
