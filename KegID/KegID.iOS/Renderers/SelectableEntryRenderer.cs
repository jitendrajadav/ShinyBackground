using System;
using KegID.Common;
using KegID.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SelectableEntry), typeof(SelectableEntryRenderer))]
namespace KegID.iOS.Renderers
{
    public class SelectableEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            UITextField nativeTextField = Control;

            nativeTextField.EditingDidBegin += (object sender, EventArgs eIos) =>
            {
                nativeTextField.PerformSelector(new ObjCRuntime.Selector("selectAll"), null, 0.0f);
            };
        }
    }
}