﻿using Microsoft.AppCenter.Crashes;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageTablet : ContentPage
    {
        public MainPageTablet()
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
    }
}