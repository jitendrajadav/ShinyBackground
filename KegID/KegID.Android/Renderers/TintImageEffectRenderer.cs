//using System;
//using System.Linq;
//using Android.Graphics;
//using Android.Widget;
//using KegID.Extensions;
//using Xamarin.Forms;
//using Xamarin.Forms.Platform.Android;


//[assembly: ResolutionGroupName(TintImageEffect.GroupName)]
//[assembly: ExportEffect(typeof(TintImageEffect), TintImageEffect.Name)]
//namespace KegID.Droid.Renderers
//{
//    public class TintImageEffectRenderer : PlatformEffect
//    {
//        protected override void OnAttached()
//        {
//            try
//            {
//                var effect = (TintImageEffect)Element.Effects.FirstOrDefault(e => e is TintImageEffect);

//                if (effect == null || !(Control is ImageView image))
//                    return;

//                var filter = new PorterDuffColorFilter(effect.TintColor.ToAndroid(), PorterDuff.Mode.SrcIn);
//                image.SetColorFilter(filter);
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine(
//                    $"An error occurred when setting the {typeof(TintImageEffect)} effect: {ex.Message}\n{ex.StackTrace}");
//            }
//        }

//        protected override void OnDetached() { }
//    }

//}