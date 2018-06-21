using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Services;
using KegID.Views;
using KegID.ViewModel;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;
using System;

namespace KegID
{
    public partial class App : Application
	{
        public const string API_Version = "1.1.75";
        public const string APP_Version = "1.0.19";
        public const string GIT_API_HASH = "2e2d138ef76c5aa1c95195617734b97aa8e9a43e";
        public const string GIT_APP_HASH = "6a6eda468ca950a89e7b428f5042b13624156a93";

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

            //for Background Services 
            //_backgroundPage = new BackgroundPage();

            //var tabbedPage = new TabbedPage();
            //tabbedPage.Children.Add(_backgroundPage);
            //tabbedPage.Children.Add(new LongRunningPage());
            //tabbedPage.Children.Add(new DownloadPage());

            // for Zebra printing
            //MainPage = new MainNavigation();

            //for PdfSharp.Xamarin.Forms
            //MainPage = new MyPDFPage();

            if (AppSettings.User != null)
            {
                MainPage = new MainPage();
            }
            else
            {
                MainPage = new LoginView();
            }

            // for Nagivation use will check later...
            //MainPage = new NavigationPage(new LoginView())
            //{
            //    BarBackgroundColor = Color.White,
            //};
            SyncManager.NotifyConnectivityChanged();
        }

        protected override void OnStart ()
		{
            //AppCenter.LogLevel = LogLevel.Verbose;
            AppCenter.Start("uwp=0404c586-124c-4b55-8848-910689b6881b;" +
                   "android=31ceef42-fd24-49d3-8e7e-21f144355dde;" +
                   "ios=b80b8476-04cf-4fc3-b7f7-be06ba7f2213",
                   typeof(Analytics), typeof(Crashes));
            // Handle when your app starts
            //SQLiteServiceClient.Instance.CreateDbIfNotExist();
            LoadPersistedValues();
        }

        protected override void OnSleep ()
		{
            // Handle when your app sleeps
            Current.Properties["SleepDate"] = DateTimeOffset.Now.ToString("O");
            //Current.Properties["FirstName"] = _backgroundPage.FirstName;
        }

        protected override void OnResume ()
		{
            // Handle when your app resumes
            LoadPersistedValues();
        }

        private void LoadPersistedValues()
        {
            if (Current.Properties.ContainsKey("SleepDate"))
            {
                var value = (string)Current.Properties["SleepDate"];
                if (DateTimeOffset.TryParse(value, out DateTimeOffset sleepDate))
                {
                    //_backgroundPage.SleepDate = sleepDate;
                }
            }

            if (Current.Properties.ContainsKey("FirstName"))
            {
                var firstName = (string)Current.Properties["FirstName"];
                //_backgroundPage.FirstName = firstName;
            }
        }
    }
}
