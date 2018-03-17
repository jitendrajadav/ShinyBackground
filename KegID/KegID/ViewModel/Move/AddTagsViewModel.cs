using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;

namespace KegID.ViewModel
{
    public class AddTagsViewModel : BaseViewModel
    {
        #region Properties

        #region ProductionDate

        /// <summary>
        /// The <see cref="ProductionDate" /> property's name.
        /// </summary>
        public const string ProductionDatePropertyName = "ProductionDate";

        private DateTime _ProductionDate = DateTime.Now.Date;

        /// <summary>
        /// Sets and gets the ProductionDate property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DateTime ProductionDate
        {
            get
            {
                return _ProductionDate;
            }

            set
            {
                if (_ProductionDate == value)
                {
                    return;
                }

                _ProductionDate = value;
                RaisePropertyChanged(ProductionDatePropertyName);
            }
        }

        #endregion

        #region BestByDataDate

        /// <summary>
        /// The <see cref="BestByDataDate" /> property's name.
        /// </summary>
        public const string BestByDataDatePropertyName = "BestByDataDate";

        private DateTime _BestByDataDate = DateTime.Now;

        /// <summary>
        /// Sets and gets the BestByDataDate property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DateTime BestByDataDate
        {
            get
            {
                return _BestByDataDate;
            }

            set
            {
                if (_BestByDataDate == value)
                {
                    return;
                }

                _BestByDataDate = value;
                RaisePropertyChanged(BestByDataDatePropertyName);
            }
        }

        #endregion


        #endregion

        #region Commands

        public RelayCommand SaveCommand { get; }

        #endregion

        #region Contructor

        public AddTagsViewModel()
        {

        }

        #endregion

        #region Methods

        #endregion
    }
}
