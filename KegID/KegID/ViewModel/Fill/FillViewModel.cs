﻿using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
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

        #region DestinationTitle

        /// <summary>
        /// The <see cref="DestinationTitle" /> property's name.
        /// </summary>
        public const string DestinationTitlePropertyName = "DestinationTitle";

        private string _DestinationTitle = "Barcode Brewing";

        /// <summary>
        /// Sets and gets the DestinationTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string DestinationTitle
        {
            get
            {
                return _DestinationTitle;
            }

            set
            {
                if (_DestinationTitle == value)
                {
                    return;
                }

                _DestinationTitle = value;
                RaisePropertyChanged(DestinationTitlePropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands
        public RelayCommand BatchCommand { get; set; }
        public RelayCommand SizeCommand { get; set; }
        public RelayCommand DestinationCommand { get; set; }
        public RelayCommand NextCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        #endregion

        #region Constructor
        public FillViewModel()
        {
            BatchCommand = new RelayCommand(BatchCommandRecieverAsync);
            SizeCommand = new RelayCommand(SizeCommandRecieverAsync);
            DestinationCommand = new RelayCommand(DestinationCommandRecieverAsync);
            NextCommand = new RelayCommand(NextCommandRecieverAsync);
            CancelCommand = new RelayCommand(CancelCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void CancelCommandRecieverAsync()
        {
            var result = await Application.Current.MainPage.DisplayActionSheet("Cancel? \n You have like to save this manifest as a draft or delete?", null, null, "Delete manifest", "Save as draft");
            if (result == "Delete manifest")
            {
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }

        private async void NextCommandRecieverAsync()
        {
            if (!BatchButtonTitle.Contains("Select batch"))
            {
                SimpleIoc.Default.GetInstance<AddPalletsViewModel>().AddPalletsTitle = "Filling " + SizeButtonTitle + " kegs with " + BatchButtonTitle + "\n" + DestinationTitle;
                await Application.Current.MainPage.Navigation.PushModalAsync(new AddPalletsView());
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Batch is required.", "Ok");
            }
        }

        private async void DestinationCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PushModalAsync(new PartnersView());

        private async void BatchCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PushModalAsync(new BatchView());

        private async void SizeCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PushModalAsync(new SizeView());

        #endregion
    }
}
