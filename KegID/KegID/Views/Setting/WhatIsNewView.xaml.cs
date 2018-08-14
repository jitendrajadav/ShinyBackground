using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using KegID.Messages;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WhatIsNewView : ContentPage
    {
        public WhatIsNewView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        void Handle_PositionSelected(object sender, CarouselView.FormsPlugin.Abstractions.PositionSelectedEventArgs e)
        {
            if (e.NewValue == 3)
                btnNavigation.Text = "Got It.";
            else 
                btnNavigation.Text = "Next >";
        }

        public void NavigationCommand(object sender, EventArgs e)
        {
            if (whatsNew.Position == 3)
            {
                MessagingCenter.Send(new WhatsNewViewToModel
                {
                    IsBack = true
                }, "WhatsNewViewToModel");
            }
            else
                whatsNew.Position++;
        }
    }
}