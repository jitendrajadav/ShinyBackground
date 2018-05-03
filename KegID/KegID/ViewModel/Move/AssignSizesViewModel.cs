using System.Collections.Generic;
using GalaSoft.MvvmLight.Command;
using KegID.Model;
using KegID.SQLiteClient;

namespace KegID.ViewModel
{
    public class AssignSizesViewModel : BaseViewModel
    {
        #region Properties

        #region BarcodeId

        /// <summary>
        /// The <see cref="BarcodeId" /> property's name.
        /// </summary>
        public const string BarcodeIdPropertyName = "BarcodeId";

        private string _BarcodeId = default(string);

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
                RaisePropertyChanged(SelectedUOwnerPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public RelayCommand ApplyToAllCommand { get; }

        #endregion

        #region Constructor

        public AssignSizesViewModel()
        {
            ApplyToAllCommand = new RelayCommand(ApplyToAllCommandReciever);
        }

        #endregion

        #region Methods

        private void ApplyToAllCommandReciever()
        {
            if (SelectedType != null)
            {
                SelectedUType = SelectedType; 
            }
            if (SelectedSize != null)
            {
                SelectedUSize = SelectedSize; 
            }
            if (SelectedOwner != null)
            {
                SelectedUOwner = SelectedOwner; 
            }
        }

        private async void LoadOwnderAsync()
        {
            OwnerCollection = await SQLiteServiceClient.Db.Table<OwnerModel>().ToListAsync();
            UOwnerCollection = OwnerCollection;
        }

        private async void LoadAssetSizeAsync()
        {
            SizeCollection = await SQLiteServiceClient.Db.Table<AssetSizeModel>().ToListAsync();
            USizeCollection = SizeCollection;
        }

        private async void LoadAssetTypeAsync()
        {
            TypeCollection = await SQLiteServiceClient.Db.Table<AssetTypeModel>().ToListAsync();
            UTypeCollection = TypeCollection;
        }

        internal void AssignInitialValue(List<Tag> _tags, Partner _partner)
        {
            LoadOwnderAsync();
            LoadAssetSizeAsync();
            LoadAssetTypeAsync();
            SelectedUOwner.FullName = _partner.FullName;
            SelectedUType.AssetType = _tags[2].Value;
            SelectedUSize.AssetSize = _tags[3].Value;
        }

        #endregion
    }
}
