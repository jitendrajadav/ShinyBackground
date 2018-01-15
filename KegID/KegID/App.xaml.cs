using GalaSoft.MvvmLight.Ioc;
using KegID.Services;
using KegID.SQLiteClient;
using KegID.View;
using KegID.ViewModel;
using Xamarin.Forms;

namespace KegID
{
    public partial class App : Application
	{
        private static ViewModelLocator _locator;

        public static ViewModelLocator Locator
        {
            get
            {
                return _locator ?? (_locator = new ViewModelLocator());
            }
        }

        public App ()
		{
			InitializeComponent();

            if (!SimpleIoc.Default.IsRegistered<IAccountService>())
                SimpleIoc.Default.Register<IAccountService, AccountService>();

            if (!SimpleIoc.Default.IsRegistered<IDashboardService>())
                SimpleIoc.Default.Register<IDashboardService, DashboardService>();

            if (!SimpleIoc.Default.IsRegistered<IMoveService>())
                SimpleIoc.Default.Register<IMoveService, MoveService>();

            if (!SimpleIoc.Default.IsRegistered<IFillService>())
                SimpleIoc.Default.Register<IFillService, FillService>();

            MainPage = new LoginView();
        }

        protected override void OnStart ()
		{

            // Handle when your app starts
            SQLiteServiceClient.Instance.CreateDbIfNotExist();
        }

        protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
