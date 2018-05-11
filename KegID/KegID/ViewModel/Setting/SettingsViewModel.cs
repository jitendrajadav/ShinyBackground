using GalaSoft.MvvmLight;
using KegID.Localization;
using Microsoft.AppCenter.Crashes;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        public List<string> Languages { get; set; } = new List<string>()
        {
            "EN",
            "NL",
            "FR"
        };

        private string _SelectedLanguage;

        public string SelectedLanguage
        {
            get { return _SelectedLanguage; }
            set
            {
                _SelectedLanguage = value;
                SetLanguage();
            }
        }

        public SettingsViewModel()
        {
            _SelectedLanguage = App.CurrentLanguage;
        }

        private void SetLanguage()
        {
            try
            {
                App.CurrentLanguage = SelectedLanguage;
                MessagingCenter.Send<object, CultureChangedMessage>(this,
                        string.Empty, new CultureChangedMessage(SelectedLanguage));

            }
            catch (System.Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
    }
}
