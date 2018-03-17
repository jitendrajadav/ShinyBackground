using GalaSoft.MvvmLight;
using KegID.Localization;

namespace KegID.ViewModel
{
    public class BaseViewModel : ViewModelBase
    {
        public LocalizedResources Resources
        {
            get;
            private set;
        }

        public BaseViewModel()
        {
            Resources = new LocalizedResources(typeof(KegIDResource), App.CurrentLanguage);
        }
    }
}
