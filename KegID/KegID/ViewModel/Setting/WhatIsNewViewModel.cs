using FFImageLoading.Forms;
using KegID.Common;
using KegID.Messages;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class WhatIsNewViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;
        private readonly IInitializeMetaData _initializeMetaData;

        #region WhatsNewItemsSource

        /// <summary>
        /// The <see cref="WhatsNewItemsSource" /> property's name.
        /// </summary>
        public const string WhatsNewItemsSourcePropertyName = "WhatsNewItemsSource";

        private ObservableCollection<View> _WhatsNewItemsSource = null;

        /// <summary>
        /// Sets and gets the WhatsNewItemsSource property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<View> WhatsNewItemsSource
        {
            get
            {
                return _WhatsNewItemsSource;
            }

            set
            {
                if (_WhatsNewItemsSource == value)
                {
                    return;
                }

                _WhatsNewItemsSource = value;
                RaisePropertyChanged(WhatsNewItemsSourcePropertyName);
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

            WhatsNewItemsSource = new ObservableCollection<View>()
            {
                new CachedImage() { Source = "new0.png", DownsampleToViewSize = false, Aspect = Aspect.Fill },
                new CachedImage() { Source = "new1.png", DownsampleToViewSize = false, Aspect = Aspect.Fill },
                new CachedImage() { Source = "new2.png", DownsampleToViewSize = false, Aspect = Aspect.Fill },
                new CachedImage() { Source = "new3.png", DownsampleToViewSize = false, Aspect = Aspect.Fill }
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
                        await _navigationService.NavigateAsync(new Uri("/MainPage", UriKind.Absolute), animated: false);
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
