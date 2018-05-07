﻿using Android.Content;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using KegID.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(BottomTabbedPageRenderer))]
namespace KegID.Droid.Renderers
{
    public class BottomTabbedPageRenderer : TabbedPageRenderer
    {
        public BottomTabbedPageRenderer(Context context) : base(context)
        {

        }
        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            InvertLayoutThroughScale();

            base.OnLayout(changed, l, t, r, b);
        }

        private void InvertLayoutThroughScale()
        {
            ViewGroup.ScaleY = -1;

            TabLayout tabLayout = null;
            ViewPager viewPager = null;

            for (int i = 0; i < ChildCount; ++i)
            {
                Android.Views.View view = GetChildAt(i);
                if (view is TabLayout) tabLayout = (TabLayout)view;
                else if (view is ViewPager) viewPager = (ViewPager)view;
            }

            tabLayout.ScaleY = viewPager.ScaleY = -1;
            viewPager.SetPadding(0, -tabLayout.MeasuredHeight, 0, 0);
        }
    }
}
