using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public class Barcode : ViewModelBase
    {
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
        public List<Tag> Tags { get; set; }
        public int PartnerCount { get; set; }
        public string PalletId { get; set; }
        public DateTime ScanDate { get; set; }
        public long ValidationStatus { get; set; }
        public DateTime DateScanned { get; set; }
        public string Contents { get; set; }
        public bool IsActive { get; set; }
        public string RemovedManifest { get; set; }
        public string TagsStr { get; set; }
    }
}
