using Microsoft.AppCenter.Crashes;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
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
