using KegID.Common;
using KegID.Localization;
using KegID.Services;
using Prism.Mvvm;
using Prism.Navigation;

namespace KegID.ViewModel
{
    public abstract class BaseViewModel : BindableBase, INavigationAware
    {
        public ILoader  Loader { get; set; }
        public LocalizedResources Resources
        {
            get;
            private set;
        }

        public BaseViewModel()
        {
            Loader = new Loader();
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
