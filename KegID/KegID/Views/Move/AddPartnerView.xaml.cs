﻿
using Prism.Navigation;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddPartnerView : ContentPage
	{
		public AddPartnerView ()
		{
			InitializeComponent ();
		}

        protected override bool OnBackButtonPressed()
        {
            (Application.Current.MainPage.Navigation.ModalStack.Last()?.BindingContext as INavigationAware)?.OnNavigatedTo(new NavigationParameters
                    {
                        { "CalcelCommandRecieverAsync", "CalcelCommandRecieverAsync" }
                    });
            return true;
        }
    }
}