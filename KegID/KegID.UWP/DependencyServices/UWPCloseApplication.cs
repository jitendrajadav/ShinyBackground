using KegID.DependencyServices;
using KegID.UWP.DependencyServices;
using Windows.ApplicationModel.Core;

[assembly: Xamarin.Forms.Dependency(typeof(UWPCloseApplication))]
namespace KegID.UWP.DependencyServices
{
    public class UWPCloseApplication : ICloseApplication
    {
        public UWPCloseApplication()
        {

        }

        public void KillApplication()
        {
            // Application.Current.Exit();
            CoreApplication.Exit();
        }
    }
}
