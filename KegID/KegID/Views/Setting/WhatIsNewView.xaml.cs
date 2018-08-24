using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using KegID.Messages;
using Microsoft.AppCenter.Crashes;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WhatIsNewView : ContentPage
    {
        public WhatIsNewView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            TapGestureRecognizer gesture = new TapGestureRecognizer();
            gesture.Command = new Command<int>((obj) =>
            {
                if (myCarouselViewCtrl.Position == 3)
                {
                    NavigateToKegFleet();
                }
            });
            gesture.CommandParameter = 1;
            myCarouselViewCtrl.GestureRecognizers.Add(gesture);
        }

        private static void NavigateToKegFleet()
        {
            try
            {
                // You can remove the switch to UI Thread if you are already in the UI Thread.
                Device.BeginInvokeOnMainThread(() =>
                {
                    Device.OpenUri(new Uri("https://www.slg.com/kegfleet/"));
                });
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        void Handle_PositionSelected(object sender, CarouselView.FormsPlugin.Abstractions.PositionSelectedEventArgs e)
        {
            btnNavigation.Text = e.NewValue == 3 ? "Got It." : "Next >";
        }

        public void NavigationCommand(object sender, EventArgs e)
        {
            if (myCarouselViewCtrl.Position == 3)
            {
                MessagingCenter.Send(new WhatsNewViewToModel
                {
                    IsBack = true
                }, "WhatsNewViewToModel");
            }
            else
                myCarouselViewCtrl.Position++;
        }
    }
}