using System;
using Xamarin.Forms;

namespace KegID.PrintTemplates
{
    public class TabbedDemoPage : TabbedPage
    {
        public TabbedDemoPage()
        {
            Title = "Xamarin Developer Demos";
            Children.Add(new ContentPage
            {
                Title = "Receipt Demo",
                Content = new ReceiptDemoView()
            });
            Children.Add(new ContentPage
            {
                Title = "Status Demo",
                Content = new StatusDemoView()
            });
            Children.Add(new FormatDemoCarousel());
            BaseDemoView.OnErrorAlert += BaseDemoView_OnErrorAlert;
            //BaseDemoView.OnAboutChosen += BaseDemoView_OnAboutChosen;
            BaseDemoView.OnAlert += BaseDemoView_OnAlert;
        }

        //private void BaseDemoView_OnAboutChosen()
        //{
        //    string message = "Developer Demo " + App.APP_Version + " {" + App.GIT_APP_HASH + "}" + Environment.NewLine
        //        + "Using SDK " + App.API_Version + " {" + App.GIT_API_HASH + "}" + Environment.NewLine + "Copyright Zebra Technologies 2016";
        //    Device.BeginInvokeOnMainThread(() =>
        //    {
        //        DisplayAlert("About", message, "OK");
        //    });
        //}

        private void BaseDemoView_OnErrorAlert(string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                DisplayAlert("Error", message, "OK");
            });
        }
        private void BaseDemoView_OnAlert(string message, string title)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                DisplayAlert(title, message, "OK");
            });
        }
    }

}
