using System.Threading.Tasks;
using Android;
using Android.Content.PM;
using Android.Support.V4.App;
using KegID.DependencyServices;
using KegID.Droid.DependencyServices;
using Plugin.CurrentActivity;

[assembly: Xamarin.Forms.Dependency(typeof(PermissionAndroid))]
namespace KegID.Droid.DependencyServices
{
    public class PermissionAndroid : IPermission
    {
        // Storage Permissions
        private const int REQUEST_EXTERNAL_STORAGE = 1;
        private static readonly string[] PERMISSIONS_STORAGE =
            {
                Manifest.Permission.ReadExternalStorage,
                Manifest.Permission.WriteExternalStorage
            };

        public Task<bool> VerifyStoragePermissions()
        {
            // Check if we have write permission
            Permission permission = global::Android.Support.V4.Content.ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.AppContext, Manifest.Permission.WriteExternalStorage);

            if (permission != Permission.Granted)
            {
                // We don't have permission so prompt the user
                ActivityCompat.RequestPermissions(CrossCurrentActivity.Current.Activity,
                        PERMISSIONS_STORAGE,
                        REQUEST_EXTERNAL_STORAGE
                );
                return Task.FromResult(true);
            }
            else
            {
                return Task.FromResult(false);
            }
        }
    }
}