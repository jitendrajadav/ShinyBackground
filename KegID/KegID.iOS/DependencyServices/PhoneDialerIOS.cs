using Foundation;
using KegID.DependencyServices;
using KegID.iOS.DependencyServices;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(PhoneDialerIOS))]
namespace KegID.iOS.DependencyServices
{
    class PhoneDialerIOS : IDialer
    {
        public Task<bool> DialAsync(string number)
        {
            return Task.FromResult(UIApplication.SharedApplication.OpenUrl(new NSUrl("tel:" + number)));
        }

    }
}
