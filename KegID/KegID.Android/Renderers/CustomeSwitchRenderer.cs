using KegID.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using KegID.Controls;
using Android.Graphics;

[assembly: ExportRenderer(typeof(CustomSwitch), typeof(CustomeSwitchRenderer))]
namespace KegID.Droid.Renderers
{
    public class CustomeSwitchRenderer : SwitchRenderer
    {
        public CustomeSwitchRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Switch> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || e.NewElement == null)
                return;
            var view = (CustomSwitch)Element;
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.JellyBean)
            {
                if (!string.IsNullOrEmpty(view.SwitchThumbImage))
                {
                    //UpdateSwitchThumbImage(view);
                }
                else
                {
                    //Control.ThumbDrawable.SetColorFilter(view.SwitchThumbColor.ToAndroid(), PorterDuff.Mode.Multiply);
                }
                //Control.TrackDrawable.SetColorFilter(view.SwitchBGColor.ToAndroid(), PorterDuff.Mode.Multiply);
                // this is the xml static code
                //     Control.SetTrackResource(Resource.Drawable.track);
            }
        }

        private void UpdateSwitchThumbImage(CustomSwitch view)
        {
            if (!string.IsNullOrEmpty(view.SwitchThumbImage))
            {
                view.SwitchThumbImage = view.SwitchThumbImage.Replace(".jpg", "").Replace(".png", "");
                int imgid = (int)typeof(Resource.Drawable).GetField(view.SwitchThumbImage).GetValue(null);
                Control.SetThumbResource(Resource.Drawable.icon);
            }
            else
            {
                // Control.SetTrackResource(Resource.Drawable.track);
            }
        }

    }
}