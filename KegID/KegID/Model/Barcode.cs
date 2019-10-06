using PropertyChanged;
using Realms;
using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public class Barcode : RealmObject
    {
        [DoNotNotify]
        public string Page { get; set; }
        [DoNotNotify]
        public bool HasMaintenaceVerified { get; set; }
        [DoNotNotify]
        public bool IsScanned { get; set; }
        [DoNotNotify]
        public string Id { get; internal set; }
        [DoNotNotify]
        #region Icon
        /// <summary>
        /// The <see cref="Icon" /> property's name.
        /// </summary>
        public const string IconPropertyName = "Icon";

        private string _Icon = default;

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
        [DoNotNotify]
        public IList<Partner> Partners { get; }
        [DoNotNotify]
        public string PalletId { get; set; }
        [DoNotNotify]
        public DateTimeOffset ScanDate { get; set; }
        [DoNotNotify]
        public long ValidationStatus { get; set; }
        [DoNotNotify]
        public DateTimeOffset DateScanned { get; set; }
        [DoNotNotify]
        public string Contents { get; set; }
        [DoNotNotify]
        public bool IsActive { get; set; }
        [DoNotNotify]
        public string RemovedManifest { get; set; }
        [DoNotNotify]
        public string TagsStr { get; set; }
        [DoNotNotify]
        public IList<MaintenanceItem> MaintenanceItems { get; }
        [DoNotNotify]
        public string Ownername { get; internal set; }
        [DoNotNotify]
        public string Batch { get; internal set; }
        [DoNotNotify]
        public string Location { get; internal set; }
    }
}
