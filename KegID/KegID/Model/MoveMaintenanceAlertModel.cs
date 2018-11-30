using KegID.LocalDb;
using KegID.Services;
using Prism.Mvvm;
using Realms;
using System.Collections.Generic;
using System.Linq;

namespace KegID.Model
{
    public class MoveMaintenanceAlertModel : BindableBase
    {
        #region BarcodeId

        /// <summary>
        /// The <see cref="BarcodeId" /> property's name.
        /// </summary>
        public const string BarcodeIdPropertyName = "BarcodeId";

        private string _BarcodeId = default;

        /// <summary>
        /// Sets and gets the BarcodeId property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string BarcodeId
        {
            get
            {
                return _BarcodeId;
            }

            set
            {
                if (_BarcodeId == value)
                {
                    return;
                }

                _BarcodeId = value;
                RaisePropertyChanged(BarcodeIdPropertyName);
            }
        }

        #endregion

        #region UTypeCollection

        /// <summary>
        /// The <see cref="UTypeCollection" /> property's name.
        /// </summary>
        public const string UTypeCollectionPropertyName = "UTypeCollection";

        private IList<AssetTypeModel> _uTypeCollection = null;

        /// <summary>
        /// Sets and gets the UTypeCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<AssetTypeModel> UTypeCollection
        {
            get
            {
                return _uTypeCollection;
            }

            set
            {
                if (_uTypeCollection == value)
                {
                    return;
                }

                _uTypeCollection = value;
                RaisePropertyChanged(UTypeCollectionPropertyName);
            }
        }

        #endregion

        #region SelectedUType

        /// <summary>
        /// The <see cref="SelectedUType" /> property's name.
        /// </summary>
        public const string SelectedUTypePropertyName = "SelectedUType";

        private AssetTypeModel _selectedUType = null;

        /// <summary>
        /// Sets and gets the SelectedUType property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public AssetTypeModel SelectedUType
        {
            get
            {
                return _selectedUType;
            }

            set
            {
                if (_selectedUType == value)
                {
                    return;
                }
                _selectedUType = value;

                if (!_selectedUType.HasInitial)
                {
                    using (var db = Realm.GetInstance(RealmDbManager.GetRealmDbConfig()).BeginWrite())
                    {
                        ConstantManager.VerifiedBarcodes.FirstOrDefault().HasMaintenaceVerified = true;
                        db.Commit();
                    }
                    var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                    RealmDb.Write(() =>
                    {
                        _selectedUType.HasInitial = true;
                    });
                }

                RaisePropertyChanged(SelectedUTypePropertyName);
            }
        }

        #endregion

        #region USizeCollection

        /// <summary>
        /// The <see cref="USizeCollection" /> property's name.
        /// </summary>
        public const string USizeCollectionPropertyName = "USizeCollection";

        private IList<AssetSizeModel> _uSizeCollection = null;

        /// <summary>
        /// Sets and gets the USizeCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<AssetSizeModel> USizeCollection
        {
            get
            {
                return _uSizeCollection;
            }

            set
            {
                if (_uSizeCollection == value)
                {
                    return;
                }

                _uSizeCollection = value;
                RaisePropertyChanged(USizeCollectionPropertyName);
            }
        }

        #endregion

        #region SelectedUSize

        /// <summary>
        /// The <see cref="SelectedUSize" /> property's name.
        /// </summary>
        public const string SelectedUSizePropertyName = "SelectedUSize";

        private AssetSizeModel _selectedUSize = null;

        /// <summary>
        /// Sets and gets the SelectedUSize property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public AssetSizeModel SelectedUSize
        {
            get
            {
                return _selectedUSize;
            }

            set
            {
                if (_selectedUSize == value)
                {
                    return;
                }
                _selectedUSize = value;

                if (!_selectedUSize.HasInitial)
                {
                    var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                    RealmDb.Write(() =>
                    {
                        _selectedUSize.HasInitial = true;
                    });
                    using (var db = Realm.GetInstance(RealmDbManager.GetRealmDbConfig()).BeginWrite())
                    {
                        ConstantManager.VerifiedBarcodes.FirstOrDefault().HasMaintenaceVerified = true;
                        db.Commit();
                    }
                }

                RaisePropertyChanged(SelectedUSizePropertyName);
            }
        }

        #endregion

        #region UOwnerCollection

        /// <summary>
        /// The <see cref="UOwnerCollection" /> property's name.
        /// </summary>
        public const string UOwnerCollectionPropertyName = "UOwnerCollection";

        private IList<OwnerModel> _uOwnerCollection = null;

        /// <summary>
        /// Sets and gets the UOwnerCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<OwnerModel> UOwnerCollection
        {
            get
            {
                return _uOwnerCollection;
            }

            set
            {
                if (_uOwnerCollection == value)
                {
                    return;
                }

                _uOwnerCollection = value;
                RaisePropertyChanged(UOwnerCollectionPropertyName);
            }
        }

        #endregion

        #region SelectedUOwner

        /// <summary>
        /// The <see cref="SelectedUOwner" /> property's name.
        /// </summary>
        public const string SelectedUOwnerPropertyName = "SelectedUOwner";

        private OwnerModel _selectedUOwner = null;

        /// <summary>
        /// Sets and gets the SelectedUOwner property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public OwnerModel SelectedUOwner
        {
            get
            {
                return _selectedUOwner;
            }

            set
            {
                if (_selectedUOwner == value)
                {
                    return;
                }
                _selectedUOwner = value;

                if (!_selectedUOwner.HasInitial)
                {
                    using (var db = Realm.GetInstance(RealmDbManager.GetRealmDbConfig()).BeginWrite())
                    {
                        ConstantManager.VerifiedBarcodes.FirstOrDefault().HasMaintenaceVerified = true;
                        db.Commit();
                    }
                    var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                    RealmDb.Write(() =>
                    {
                        _selectedUOwner.HasInitial = true;
                    });
                    
                }
                RaisePropertyChanged(SelectedUOwnerPropertyName);
            }
        }

        #endregion
    }
}
