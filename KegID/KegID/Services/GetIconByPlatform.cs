using Xamarin.Forms;

namespace KegID.Services
{
    public class GetIconByPlatform : IGetIconByPlatform
    {
        public string GetIcon(string image)
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    return image;
                case Device.Android:
                    return image;
                case Device.UWP:
                    return "Assets/" + image;
            }
            return string.Empty;
        }

    }
}
