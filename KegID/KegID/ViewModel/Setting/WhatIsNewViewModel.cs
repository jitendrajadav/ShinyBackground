using FFImageLoading.Forms;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class WhatIsNewViewModel : ViewModelBase
    {
        #region Properties

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
        public WhatIsNewViewModel()
        {
            WhatsNewItemsSource = new ObservableCollection<Xamarin.Forms.View>()
            {
                new CachedImage() { Source = "new0.png", DownsampleToViewSize = false, Aspect = Aspect.Fill },
                new CachedImage() { Source = "new1.png", DownsampleToViewSize = false, Aspect = Aspect.Fill },
                new CachedImage() { Source = "new2.png", DownsampleToViewSize = false, Aspect = Aspect.Fill }
            };
        }

        #endregion

        #region Methods


        #endregion
    }
}
