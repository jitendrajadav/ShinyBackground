using Android.OS;
using KegID.DependencyServices;
using KegID.Droid.DependencyServices;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidCloseApplication))]
namespace KegID.Droid.DependencyServices
{
    public class AndroidCloseApplication : ICloseApplication
    {
        public void KillApplication()
        {
            Process.KillProcess(Process.MyPid());
        }
    }
}