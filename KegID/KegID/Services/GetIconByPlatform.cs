using Xamarin.Forms;

namespace KegID.Services
{
    public class GetIconByPlatform : IGetIconByPlatform
    {
        public string GetIcon(string image)
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
