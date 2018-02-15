using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KegID.Common
{
    public static class Utils
    {
        public static async Task<bool> CheckPermissions(Permission permission)
        {
            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
            bool request = false;
            if (permissionStatus == PermissionStatus.Denied)
            {
                if (Device.RuntimePlatform == Device.iOS)
                {

                    var title = $"{permission} Permission";
                    var question = $"To use this plugin the {permission} permission is required. Please go into Settings and turn on {permission} for the app.";
                    var positive = "Settings";
                    var negative = "Maybe Later";
                    var task = Application.Current?.MainPage?.DisplayAlert(title, question, positive, negative);
                    if (task == null)
                        return false;

                    var result = await task;
                    if (result)
                    {
                        CrossPermissions.Current.OpenAppSettings();
                    }

                    return false;
                }

                request = true;

            }

            if (request || permissionStatus != PermissionStatus.Granted)
            {
                var newStatus = await CrossPermissions.Current.RequestPermissionsAsync(permission);
                if (newStatus.ContainsKey(permission) && newStatus[permission] != PermissionStatus.Granted)
                {
                    var title = $"{permission} Permission";
                    var question = $"To use the plugin the {permission} permission is required.";
                    var positive = "Settings";
                    var negative = "Maybe Later";
                    var task = Application.Current?.MainPage?.DisplayAlert(title, question, positive, negative);
                    if (task == null)
                        return false;

                    var result = await task;
                    if (result)
                    {
                        CrossPermissions.Current.OpenAppSettings();
                    }
                    return false;
                }
            }

            return true;
        }

        public static int CalculateCheckDigit(string barCodeWithoutCheckDigit)
        {
            int chkDigitSum = 0; // this will be a running total
            int weight = 0;
            int nextMultipleOf10 = 0;
            int checkDigit = 0;

            // loop through digits from right to left
            for (int i = 0; i < barCodeWithoutCheckDigit.Length; i++)
            {
                //set ch to "current" character to be processed
                char ch = barCodeWithoutCheckDigit[barCodeWithoutCheckDigit.Length - i - 1];

                // our "digit" is calculated using ASCII value - 48
                int digit = (int)ch - 48;

                // weight will be the current digit's contribution to the running total
                if (i % 2 == 0)
                {
                    weight = (3 * digit);
                }
                else
                {
                    // even-positioned digits just contribute their ascii value minus 48
                    weight = digit;
                }

                // keep a running total of weights
                chkDigitSum += weight;
            }

            if (chkDigitSum % 10 == 0)
                nextMultipleOf10 = chkDigitSum;
            else
                nextMultipleOf10 = chkDigitSum - (chkDigitSum % 10) + 10;

            checkDigit = nextMultipleOf10 - chkDigitSum;

            return (checkDigit);

        }
    }
}
