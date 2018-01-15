using GalaSoft.MvvmLight;

namespace KegID.ViewModel
{
    public class SearchPartnersViewModel : ViewModelBase
    {
        #region Properties
        
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

        #endregion
        
        #region Commands

        #endregion

        #region Contructor

        public SearchPartnersViewModel()
        {
            BackPartners = "< Partners";
        }

        #endregion

        #region Methods

        #endregion
    }
}
