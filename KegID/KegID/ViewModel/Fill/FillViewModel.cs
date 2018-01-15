using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KegID.View;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class FillViewModel : ViewModelBase
    {
        #region Properties

        #region BatchButtonTitle

        /// <summary>
        /// The <see cref="BatchButtonTitle" /> property's name.
        /// </summary>
        public const string BatchButtonTitlePropertyName = "BatchButtonTitle";

        private string _BatchButtonTitle = "Select batch";

        /// <summary>
        /// Sets and gets the BatchButtonTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string BatchButtonTitle
        {
            get
            {
                return _BatchButtonTitle;
            }

            set
            {
                if (_BatchButtonTitle == value)
                {
                    return;
                }

                _BatchButtonTitle = value;
                RaisePropertyChanged(BatchButtonTitlePropertyName);
            }
        }

        #endregion

        #region SizeButtonTitle

        /// <summary>
        /// The <see cref="SizeButtonTitle" /> property's name.
        /// </summary>
        public const string SizeButtonTitlePropertyName = "SizeButtonTitle";

        private string _SizeButtonTitle = "Select size";

        /// <summary>
        /// Sets and gets the SizeButtonTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string SizeButtonTitle
        {
            get
            {
                return _SizeButtonTitle;
            }

            set
            {
                if (_SizeButtonTitle == value)
                {
                    return;
                }

                _SizeButtonTitle = value;
                RaisePropertyChanged(SizeButtonTitlePropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands
        public RelayCommand BatchCommand { get; set; }

        public RelayCommand SizeCommand { get; set; }

        #endregion

        #region Constructor
        public FillViewModel()
        {
            BatchCommand = new RelayCommand(BatchCommandRecieverAsync);
            SizeCommand = new RelayCommand(SizeCommandRecieverAsync);
        }

        #endregion

        #region Methods
        private async void BatchCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new BatchView());
        }
        private async void SizeCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new SizeView());
        }

        #endregion
    }
}
