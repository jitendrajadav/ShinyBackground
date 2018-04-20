using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Services;
using KegID.SQLiteClient;
using KegID.Views;
using KegID.ViewModel;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace KegID
{
    public partial class App : Application
	{
        public static string CurrentLanguage = "EN";

        private static ViewModelLocator _locator;

        public static ViewModelLocator Locator
        {
            get
            {
                return _locator ?? (_locator = new ViewModelLocator());
            }
        }

        public App()
        {
            InitializeComponent();

            #region Services Register
            if (!SimpleIoc.Default.IsRegistered<IAccountService>())
                SimpleIoc.Default.Register<IAccountService, AccountService>();

            if (!SimpleIoc.Default.IsRegistered<IDashboardService>())
                SimpleIoc.Default.Register<IDashboardService, DashboardService>();

            if (!SimpleIoc.Default.IsRegistered<IMoveService>())
                SimpleIoc.Default.Register<IMoveService, MoveService>();

            if (!SimpleIoc.Default.IsRegistered<IFillService>())
                SimpleIoc.Default.Register<IFillService, FillService>();

            if (!SimpleIoc.Default.IsRegistered<IPalletizeService>())
                SimpleIoc.Default.Register<IPalletizeService, PalletizeService>();

            if (!SimpleIoc.Default.IsRegistered<IMaintainService>())
                SimpleIoc.Default.Register<IMaintainService, MaintainService>();

            #endregion

            if (AppSettings.User != null)
                MainPage = new MainPage();
            else
            MainPage = new LoginView();

            // for Nagivation use will check later...
            //MainPage = new NavigationPage(new LoginView())
            //{
            //    BarBackgroundColor = Color.White,
            //};
        }

        protected override void OnStart ()
		{
            //AppCenter.LogLevel = LogLevel.Verbose;
            AppCenter.Start("uwp=0404c586-124c-4b55-8848-910689b6881b;" +
                   "android=31ceef42-fd24-49d3-8e7e-21f144355dde;" +
                   "ios=b80b8476-04cf-4fc3-b7f7-be06ba7f2213",
                   typeof(Analytics), typeof(Crashes));
            // Handle when your app starts
            SQLiteServiceClient.Instance.CreateDbIfNotExist();
            Geolocation.GetGPS();
           //var value= Geolocation.GetCurrentLocation();
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
