using System.Threading.Tasks;
using KegID.Localization;
using KegID.Services;
using Prism.Mvvm;
using Prism.Navigation;

namespace KegID.ViewModel
{
    public abstract class BaseViewModel : BindableBase, INavigationAware, IInitializeAsync
    {
        /*
         * Define Fields
         */
        protected INavigationService _navigationService { get; }

        public ILoader  Loader { get; set; }
        public LocalizedResources Resources
        {
            get;
            private set;
        }

        public BaseViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            Loader = new Loader();
            Resources = new LocalizedResources(typeof(KegIDResource), App.CurrentLanguage);
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }


        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual async Task InitializeAsync(INavigationParameters parameters)
        {

        }
    }
}
