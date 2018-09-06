using Prism.Mvvm;
using System.Collections.Generic;

namespace KegID.Model
{
    public class PalletModel : BindableBase
    {
        public string ManifestId { get; set; }
        public string BatchId { get; set; }

        #region Count

        /// <summary>
        /// The <see cref="Count" /> property's name.
        /// </summary>
        public const string CountPropertyName = "Count";

        private int _Count = 0;

        /// <summary>
        /// Sets and gets the Count property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int Count
        {
            get
            {
                return _Count;
            }

            set
            {
                if (_Count == value)
                {
                    return;
                }

                _Count = value;
                RaisePropertyChanged(CountPropertyName);
            }
        }

        #endregion

        public IList<BarcodeModel> Barcode { get; set; }
    }
}
