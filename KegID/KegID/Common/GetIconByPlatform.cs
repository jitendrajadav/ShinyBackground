using Xamarin.Forms;

namespace KegID.Common
{
    public static class GetIconByPlatform
    {
        public static string GetIcon(string image)
        {
            string value = string.Empty;
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    value = image;
                    break;
                case Device.Android:
                    value = image;
                    break;
                case Device.UWP:
                    value = "Assets/" + image;
                    break;
            }
            return value;
        }

    }
}
