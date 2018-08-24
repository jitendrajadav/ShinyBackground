using KegID.Common;
using KegID.Messages;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class WhatIsNewViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;
        private readonly IInitializeMetaData _initializeMetaData;

        #region ImageCollection

        /// <summary>
        /// The <see cref="ImageCollection" /> property's name.
        /// </summary>
        public const string ImageCollectionPropertyName = "ImageCollection";

        private IList<ImageClass> _imageCollection = null;

        /// <summary>
        /// Sets and gets the ImageCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<ImageClass> ImageCollection
        {
            get
            {
                return _imageCollection;
            }

            set
            {
                if (_imageCollection == value)
                {
                    return;
                }

                _imageCollection = value;
                RaisePropertyChanged(ImageCollectionPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        #endregion

        #region Constructor

        public WhatIsNewViewModel(INavigationService navigationService, IInitializeMetaData initializeMetaData)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
            _initializeMetaData = initializeMetaData;

            ImageCollection = new List<ImageClass>
            {
                new ImageClass{ ImageUri = "new0.png" },
                new ImageClass{ ImageUri = "new1.png" },
                new ImageClass{ ImageUri = "new2.png" },
                new ImageClass{ ImageUri = "new3.png" },
            };
        }

        #endregion

        #region Methods

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            HandleReceivedMessages();
        }

        private void HandleReceivedMessages()
        {
            MessagingCenter.Subscribe<WhatsNewViewToModel>(this, "WhatsNewViewToModel", message =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSettings.IsFreshInstall)
                    {
                        AppSettings.IsFreshInstall = false;
                        // If it is Android or iOS
                        await _navigationService.NavigateAsync(new Uri("/NavigationPage/MainPage", UriKind.Absolute), useModalNavigation: true, animated: false);
                        // If it is UWP
                        //await _navigationService.NavigateAsync(new Uri("/NavigationPage/MainPage", UriKind.RelativeOrAbsolute), useModalNavigation: true, animated: false);
                        try
                        {
                            Loader.StartLoading();
                            await _initializeMetaData.LoadInitializeMetaData();
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }
                        finally
                        {
                            AppSettings.IsMetaDataLoaded = true;
                            Loader.StopLoading();
                        }
                    }
                    else
                        await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
                });
            });
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            MessagingCenter.Unsubscribe<WhatsNewViewToModel>(this, "WhatsNewViewToModel");
        }

        #endregion
    }
}
