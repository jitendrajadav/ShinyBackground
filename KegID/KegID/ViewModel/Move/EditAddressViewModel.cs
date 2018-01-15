using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace KegID.ViewModel
{
    public class EditAddressViewModel : ViewModelBase
    {
        #region Properties

        #region AddressTitle

        /// <summary>
        /// The <see cref="AddressTitle" /> property's name.
        /// </summary>
        public const string AddressTitlePropertyName = "AddressTitle";

        private string _AddressTitle = default(string);

        /// <summary>
        /// Sets and gets the AddressTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string AddressTitle
        {
            get
            {
                return _AddressTitle;
            }

            set
            {
                if (_AddressTitle == value)
                {
                    return;
                }

                _AddressTitle = value;
                RaisePropertyChanged(AddressTitlePropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands
        public RelayCommand BackCommand { get; }

        public RelayCommand DoneCommand { get; set; }
       
        #endregion

        #region Constructor
        public EditAddressViewModel()
        {
            BackCommand = new RelayCommand(BackCommandReciever);
            DoneCommand = new RelayCommand(DoneCommandReciever);
        }

        #endregion

        #region Methods
        private void DoneCommandReciever()
        {
            
        }

        private void BackCommandReciever()
        {
            
        }

        #endregion
    }
}
