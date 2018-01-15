
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;

namespace KegID.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WhatIsNewView : ContentPage
    {
        public WhatIsNewView()
        {
            InitializeComponent();
        }

        void Handle_PositionSelected(object sender, CarouselView.FormsPlugin.Abstractions.PositionSelectedEventArgs e)
        {
            if (e.NewValue == 2)
                btnNavigation.Text = "Got It.";
            else 
                btnNavigation.Text = "Next >";
        }

        public async void NavigationCommand(object sender, EventArgs e)
        {
            if (whatsNew.Position == 2)
                await Application.Current.MainPage.Navigation.PopModalAsync();
            else
                whatsNew.Position++;
        }
    }
}