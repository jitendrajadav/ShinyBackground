using System.Linq;
using System.Threading.Tasks;
using Android.Content;
using Android.Net;
using Android.Telephony;
using KegID.DependencyServices;
using KegID.Droid.DependencyServices;

[assembly: Xamarin.Forms.Dependency(typeof(PhoneDialerAndroid))]
namespace KegID.Droid.DependencyServices
{
    class PhoneDialerAndroid : IDialer
    {
        public Task<bool> DialAsync(string number)
        {
            var context = global::Android.App.Application.Context;
            if (context == null)
                return Task.FromResult(false);

            var intent = new Intent(Intent.ActionCall);
            intent.AddFlags(ActivityFlags.NewTask);
            intent.SetData(Uri.Parse("tel:" + number));

            if (IsIntentAvailable(context, intent))
            {
                context.StartActivity(intent);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public static bool IsIntentAvailable(Context context, Intent intent)
        {
            var packageManager = context.PackageManager;

            var list = packageManager.QueryIntentServices(intent, 0)
                .Union(packageManager.QueryIntentActivities(intent, 0));

            if (list.Any())
                return true;

            var manager = TelephonyManager.FromContext(context);
            return manager.PhoneType != PhoneType.None;
        }
    }
}