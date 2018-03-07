using KegID.Controls;
using KegID.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomNavigationPage), typeof(BlueNavigationRenderer))]
namespace KegID.iOS.Renderers
{
    public class BlueNavigationRenderer : NavigationRenderer
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.NavigationBar.BarTintColor = UIColor.FromPatternImage(UIImage.FromFile("topbar_bg@2x.png"));
        }
    }
}
