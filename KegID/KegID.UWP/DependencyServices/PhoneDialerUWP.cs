using System.Threading.Tasks;
using KegID.DependencyServices;
using KegID.UWP.DependencyServices;
using Windows.ApplicationModel.Calls;
using Windows.UI.Popups;
using Xamarin.Forms;

[assembly: Dependency(typeof(PhoneDialerUWP))]
namespace KegID.UWP.DependencyServices
{
    class PhoneDialerUWP : IDialer
    {
        bool dialled = false;

        public async Task<bool> DialAsync(string number)
        {
            await DialNumber(number);
            return dialled;
        }

        async Task DialNumber(string number)
        {
            var phoneLine = await GetDefaultPhoneLineAsync();
            if (phoneLine != null)
            {
                phoneLine.Dial(number, number);
                dialled = true;
            }
            else
            {
                var dialog = new MessageDialog("No line found to place the call");
                await dialog.ShowAsync();
                dialled = false;
            }
        }

        async Task<PhoneLine> GetDefaultPhoneLineAsync()
        {
            var phoneCallStore = await PhoneCallManager.RequestStoreAsync();
            var lineId = phoneCallStore.GetDefaultLineAsync();
            return await PhoneLine.FromIdAsync(lineId);
        }
    }
}
