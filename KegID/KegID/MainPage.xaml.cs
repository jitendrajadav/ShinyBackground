using Microsoft.AppCenter.Crashes;
using System;
using Xamarin.Forms;

namespace KegID
{
    public partial class MainPage : CarouselPage
    {
		public MainPage()
		{
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                 Crashes.TrackError(ex);
            }
        }
	}
}
