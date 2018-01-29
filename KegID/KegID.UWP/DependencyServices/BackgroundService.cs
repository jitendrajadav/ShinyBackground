using KegID.DependencyServices;
using KegID.UWP.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(BackgroundService))]
namespace KegID.UWP.DependencyServices
{
    public class BackgroundService : IBackgroundService
    {
        public void Start()
        {
            MessagingCenter.Send<object, string>(this, "UpdateLabel", "Hello from UWP");
        }

        public void Stop()
        {
            MessagingCenter.Send<object, string>(this, "UpdateLabel", "Hello from UWP");
        }
    }
}
