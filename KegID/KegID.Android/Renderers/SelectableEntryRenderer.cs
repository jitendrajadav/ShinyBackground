using Android.Content;
using Android.Widget;
using KegID.Common;
using KegID.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SelectableEntry), typeof(SelectableEntryRenderer))]
namespace KegID.Droid.Renderers
{
    public class SelectableEntryRenderer : EntryRenderer
    {
        public SelectableEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                var nativeEditText = (EditText)Control;
                nativeEditText.SetSelectAllOnFocus(true);
            }
        }
    }
}