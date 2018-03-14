//using System.ComponentModel;
//using Android.Content;
//using KegID.Droid.Renderers;
//using Xamarin.Forms;
//using Xamarin.Forms.Platform.Android;

//[assembly: ExportRenderer(typeof(Button), typeof(CustomButtonRenderer))]
//namespace KegID.Droid.Renderers
//{
//    public class CustomButtonRenderer : ButtonRenderer
//    {
//        public CustomButtonRenderer(Context context) : base(context)
//        {

//        }

//        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
//        {
//            base.OnElementChanged(e);
//            if (Control != null)
//            {
//                Control.SetAllCaps(false);
//            }
//        }
//        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
//        {
//            base.OnElementPropertyChanged(sender, e);
//        }
//    }
//}