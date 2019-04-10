using UIKit;
using Xamarin.Forms;

namespace KegID.iOS.Extensions
{
    /// <summary>
    /// Color extensions
    /// </summary>
    public static class CheckColorExtensions
    {
        internal static readonly UIColor SeventyPercentGrey = new UIColor(0.7f, 0.7f, 0.7f, 1);
        public static bool IsDefault(this Color color) => Color.Default == color;
    }
}