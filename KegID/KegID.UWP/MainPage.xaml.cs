using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Xaml;
using Xamarin.Forms;
using System;
using KegID.Common;

namespace KegID.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            Xamarin.FormsMaps.Init(AppSettings.BingMapsApiKey);

            LoadApplication(new KegID.App());

            //if (IsRegistered())
            //    Deregister();

            //Loaded += MainPage_Loaded;

        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            await BackgroundTaskAsync();
        }

        private void Deregister()
        {
            var taskName = "BackgroundTask";

            foreach (var task in BackgroundTaskRegistration.AllTasks)
                if (task.Value.Name == taskName)
                    task.Value.Unregister(true);
        }

        private bool IsRegistered()
        {
            var taskName = "BackgroundTask";

            foreach (var task in BackgroundTaskRegistration.AllTasks)
                if (task.Value.Name == taskName)
                    return true;

            return false;
        }

        private async Task BackgroundTaskAsync()
        {
            BackgroundExecutionManager.RemoveAccess();

            await BackgroundExecutionManager.RequestAccessAsync();

            var builder = new BackgroundTaskBuilder
            {
                Name = "BackgroundTask",
                TaskEntryPoint = "KegID.RuntimeComponent.BackgroundTask"
            };
            builder.SetTrigger(new SystemTrigger(SystemTriggerType.InternetAvailable, false));

            BackgroundTaskRegistration task = builder.Register();

            task.Completed += Task_Completed;
        }

        private void Task_Completed(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var key = "BackgroundTask";
            var message = settings.Values[key].ToString();

            // Run your background task code here
            MessagingCenter.Send<object, string>(this, "UpdateLabel", message);
        }

    }
}
