using System;
using System.Linq;
using KegID.Extensions;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName(TintImageEffect.GroupName)]
[assembly: ExportEffect(typeof(TintImageEffect), TintImageEffect.Name)]
namespace KegID.iOS.Renderers
{
    public class TintImageEffectRenderer : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                var effect = (TintImageEffect)Element.Effects.FirstOrDefault(e => e is TintImageEffect);

                if (effect == null || !(Control is UIImageView image))
                    return;

                image.Image = image.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                image.TintColor = effect.TintColor.ToUIColor();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"An error occurred when setting the {typeof(TintImageEffect)} effect: {ex.Message}\n{ex.StackTrace}");
            }
        }

        protected override void OnDetached() { }
    }

}