using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Xaml;
using Xamarin.Forms;
using System;
using KegID.Common;
using Prism;
using Prism.Ioc;
using System.Reflection;

namespace KegID.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            SetupAdditionalEncodingProviders();
            Xamarin.FormsMaps.Init(AppSettings.BingMapsApiKey);
            LoadApplication(new KegID.App());
        }

        // setup additional encoding providers using reflection. In your own application, it's typically sufficient to just
        // call Encoding.RegisterProvider(CodePagesEncodingProvider.Instance). We use reflection to make this also work with
        // older .NET versions that don't yet have the functionality.
        private void SetupAdditionalEncodingProviders()
        {
            var encodingType = Type.GetType("System.Text.Encoding, Windows, ContentType=WindowsRuntime");
            var codePagesEncodingProviderType = Type.GetType("System.Text.CodePagesEncodingProvider, Windows, ContentType=WindowsRuntime");
            if (encodingType == null || codePagesEncodingProviderType == null)
            {
                return;
            }
            var registerProviderMethod = encodingType.GetRuntimeMethod("RegisterProvider", new Type[] { codePagesEncodingProviderType });
            var instanceProperty = codePagesEncodingProviderType.GetRuntimeProperty("Instance");
            if (registerProviderMethod == null || instanceProperty == null)
            {
                return;
            }
            try
            {
                var theInstance = instanceProperty.GetValue(null);
                if (theInstance == null)
                {
                    return;
                }
                registerProviderMethod.Invoke(null, new object[] { theInstance });
            }
            catch (TargetInvocationException)
            {
                return;
            }
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

    public class UwpInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }

}
