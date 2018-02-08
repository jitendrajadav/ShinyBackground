using GalaSoft.MvvmLight.Ioc;
using KegID.Services;
using KegID.SQLiteClient;
using KegID.View;
using KegID.ViewModel;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
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
            AppCenter.Start("uwp=78996084-2974-4e93-ae0b-c8357d2b172d;" +
                   "android=31ceef42-fd24-49d3-8e7e-21f144355dde;" +
                   "ios=b80b8476-04cf-4fc3-b7f7-be06ba7f2213",
                   typeof(Analytics), typeof(Crashes));
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
