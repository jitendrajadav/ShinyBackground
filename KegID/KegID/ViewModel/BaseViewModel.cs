using KegID.Localization;
using Prism.Mvvm;
using Prism.Navigation;

namespace KegID.ViewModel
{
    public abstract class BaseViewModel : BindableBase, INavigationAware
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

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {

        }
    }
}
