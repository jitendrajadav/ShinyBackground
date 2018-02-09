using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.View;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class FillScanViewModel : ViewModelBase
    {
        #region Properties

        #region Pallet

        /// <summary>
        /// The <see cref="Pallet" /> property's name.
        /// </summary>
        public const string PalletPropertyName = "Pallet";

        private string _Pallet = string.Empty;

        /// <summary>
        /// Sets and gets the Pallet property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Pallet
        {
            get
            {
                return _Pallet;
            }

            set
            {
                if (_Pallet == value)
                {
                    return;
                }

                _Pallet = value;
                RaisePropertyChanged(PalletPropertyName);
            }
        }

        #endregion

        #region Barcode

        /// <summary>
        /// The <see cref="Barcode" /> property's name.
        /// </summary>
        public const string BarcodePropertyName = "Barcode";

        private string _Barcode = default(string);

        /// <summary>
        /// Sets and gets the Barcode property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Barcode
        {
            get
            {
                return _Barcode;
            }

            set
            {
                if (_Barcode == value)
                {
                    return;
                }

                _Barcode = value;
                RaisePropertyChanged(BarcodePropertyName);
            }
        }

        #endregion

        #region TagsStr

        /// <summary>
        /// The <see cref="TagsStr" /> property's name.
        /// </summary>
        public const string TagsStrPropertyName = "TagsStr";

        private string _TagsStr = default(string);

        /// <summary>
        /// Sets and gets the TagsStr property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string TagsStr
        {
            get
            {
                return _TagsStr;
            }

            set
            {
                if (_TagsStr == value)
                {
                    return;
                }

                _TagsStr = value;
                RaisePropertyChanged(TagsStrPropertyName);
            }
        }

        #endregion

        #region IsPalletVisible

        /// <summary>
        /// The <see cref="IsPalletVisible" /> property's name.
        /// </summary>
        public const string IsPalletVisiblePropertyName = "IsPalletVisible";

        private bool _IsPalletVisible = true;

        /// <summary>
        /// Sets and gets the IsPalletVisible property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsPalletVisible
        {
            get
            {
                return _IsPalletVisible;
            }

            set
            {
                if (_IsPalletVisible == value)
                {
                    return;
                }

                _IsPalletVisible = value;
                RaisePropertyChanged(IsPalletVisiblePropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public RelayCommand CancelCommand { get; set; }
        public RelayCommand BarcodeScanCommand { get; set; }
        public RelayCommand AddTagsCommand { get; set; }
        public RelayCommand PrintCommand { get; set; }
        public RelayCommand IsPalletVisibleCommand { get; set; }
        
        #endregion

        #region Constructor

        public FillScanViewModel()
        {
            CancelCommand = new RelayCommand(CancelCommandRecieverAsync);
            BarcodeScanCommand = new RelayCommand(BarcodeScanCommandRecieverAsync);
            AddTagsCommand = new RelayCommand(AddTagsCommandRecieverAsync);
            PrintCommand = new RelayCommand(PrintCommandReciever);
            IsPalletVisibleCommand = new RelayCommand(IsPalletVisibleCommandReciever);

            Pallet = "Pallet# -10000000084004099351";
        }

        #endregion

        #region Methods
        private void IsPalletVisibleCommandReciever()
        {
            IsPalletVisible = !IsPalletVisible;
        }

        private void PrintCommandReciever()
        {

        }

        private async void CancelCommandRecieverAsync()
        {
           await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private void BarcodeScanCommandRecieverAsync()
        {
            SimpleIoc.Default.GetInstance<ScanKegsViewModel>().BarcodeScanCommandReciever();
        }
        private async void AddTagsCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new AddTagsView());
        }

        #endregion

    }
}
