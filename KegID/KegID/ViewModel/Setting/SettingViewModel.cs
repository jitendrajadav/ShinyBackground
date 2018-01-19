using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KegID.View;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class SettingViewModel : ViewModelBase
    {
        #region Properties

        #endregion

        #region Commands

        public RelayCommand PrinterSettingCommand { get; set; }
        public RelayCommand WhatsNewCommand { get; set; }
        public RelayCommand SupportCommand { get; set; }

        #endregion

        #region Constructor

        public SettingViewModel()
        {
            WhatsNewCommand = new RelayCommand(WhatsNewCommandRecieverAsync);
            SupportCommand = new RelayCommand(SupportCommandReciever);
            PrinterSettingCommand = new RelayCommand(PrinterSettingCommandRecieverAsync);
        }

        #endregion

        #region Methods
        private async void PrinterSettingCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PushModalAsync(new PrinterSettingView());

        private async void WhatsNewCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PushModalAsync(new WhatIsNewView());

        private void SupportCommandReciever()
        {
            // You can remove the switch to UI Thread if you are already in the UI Thread.
            Device.BeginInvokeOnMainThread(() =>
            {
                Device.OpenUri(new Uri("https://www.slg.com/"));
            });
        }
        #endregion

    }
}
