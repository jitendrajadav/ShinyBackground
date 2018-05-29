using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using KegID.Common;
using KegID.PrintTemplates;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class PrinterSettingViewModel : BaseViewModel
    {
        #region Properties

        #region SelectedPrinter

        /// <summary>
        /// The <see cref="SelectedPrinter" /> property's name.
        /// </summary>
        public const string SelectedPrinterPropertyName = "SelectedPrinter";

        private string _SelectedPrinter = "No printers found";

        /// <summary>
        /// Sets and gets the SelectedPrinter property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string SelectedPrinter
        {
            get
            {
                return _SelectedPrinter;
            }

            set
            {
                if (_SelectedPrinter == value)
                {
                    return;
                }

                _SelectedPrinter = value;
                RaisePropertyChanged(SelectedPrinterPropertyName);
            }
        }

        #endregion

        #region IsBluetoothOn

        /// <summary>
        /// The <see cref="IsBluetoothOn" /> property's name.
        /// </summary>
        public const string IsBluetoothOnPropertyName = "IsBluetoothOn";

        private bool _IsBluetoothOn = false;

        /// <summary>
        /// Sets and gets the IsBluetoothOn property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsBluetoothOn
        {
            get
            {
                return _IsBluetoothOn;
            }

            set
            {
                if (_IsBluetoothOn == value)
                {
                    return;
                }

                _IsBluetoothOn = value;
                RaisePropertyChanged(IsBluetoothOnPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public RelayCommand CancelCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand SelectPrinterCommand { get; }
        public RelayCommand PrinterTestCommand { get; }

        #endregion

        #region Constructor

        public PrinterSettingViewModel()
        {
            CancelCommand = new RelayCommand(CancelCommandRecieverAsync);
            SaveCommand = new RelayCommand(SaveCommandRecieverAsync);
            SelectPrinterCommand = new RelayCommand(SelectPrinterCommandRecieverAsync);
            PrinterTestCommand = new RelayCommand(PrinterTestCommandReciever);
        }

        #endregion

        #region Methods
        private async void SelectPrinterCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new SelectPrinterView());
        }

        private void PrinterTestCommandReciever()
        {
            new Task(new Action(() => {
                ZebraPrinterManager.SendZplPallet(ZebraPrinterManager.testPrint);
            })).Start();
        }

        private async void CancelCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PopModalAsync();

        private async void SaveCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PopModalAsync();

        #endregion
    }
}
