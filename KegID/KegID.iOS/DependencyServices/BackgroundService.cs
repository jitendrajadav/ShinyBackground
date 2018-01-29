using KegID.DependencyServices;
using KegID.iOS.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(BackgroundService))]
namespace KegID.iOS.DependencyServices
{
    public class BackgroundService : IBackgroundService
    {
        public void Start()
        {
            MessagingCenter.Send<object, string>(this, "UpdateLabel", "Hello from iOS");
        }

        public void Stop()
        {
            MessagingCenter.Send<object, string>(this, "Stop Updating Label", "Hello from iOS");
        }
    }
}
