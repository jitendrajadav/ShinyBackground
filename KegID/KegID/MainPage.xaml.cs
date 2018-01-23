using System;
using System.Diagnostics;
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
                Debug.WriteLine(ex.Message);
            }
		}
	}
}
