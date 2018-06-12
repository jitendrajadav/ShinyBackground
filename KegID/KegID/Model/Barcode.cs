using GalaSoft.MvvmLight;
using Realms;
using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public class Barcode : RealmObject
    {
        public bool HasMaintenaceVerified { get; set; }
        public bool IsScanned { get; set; }
        public string Id { get; internal set; }
        
        #region Icon
        /// <summary>
        /// The <see cref="Icon" /> property's name.
        /// </summary>
        public const string IconPropertyName = "Icon";

        private string _Icon = default(string);

        /// <summary>
        /// Sets and gets the Icon property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Icon
        {
            get
            {
                return _Icon;
            }

            set
            {
                if (_Icon == value)
                {
                    return;
                }

                _Icon = value;
                RaisePropertyChanged(IconPropertyName);
            }
        }

        #endregion
        public IList<Tag> Tags { get; }
        public IList<Partner> Partners { get; }
        public string PalletId { get; set; }
        public DateTimeOffset ScanDate { get; set; }
        public long ValidationStatus { get; set; }
        public DateTimeOffset DateScanned { get; set; }
        public string Contents { get; set; }
        public bool IsActive { get; set; }
        public string RemovedManifest { get; set; }
        public string TagsStr { get; set; }
        public IList<MaintenanceItem> MaintenanceItems { get; }
        public string Ownername { get; internal set; }
        public string Batch { get; internal set; }
        public string Location { get; internal set; }
    }
}
