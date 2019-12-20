using Android.Content;
using KegID.Android.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Label), typeof(ZebraLabelRenderer))]
namespace KegID.Android.Renderers
{
    public class ZebraLabelRenderer : LabelRenderer
    {
        public ZebraLabelRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                if (LineBreakMode.WordWrap.Equals(Element.GetValue(Label.LineBreakModeProperty)))
                {
                    Control.SetSingleLine(false); // Workaround for Xamarin.Forms Android bug in prerelease 3.3.0.840541-pre1 package: https://forums.xamarin.com/discussion/139984/why-are-my-labels-truncating-my-text-at-n
                }
            }
        }
    }
}