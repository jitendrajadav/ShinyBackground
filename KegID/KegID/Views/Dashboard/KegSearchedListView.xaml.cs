﻿
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class KegSearchedListView : ContentPage
	{
		public KegSearchedListView ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override bool OnBackButtonPressed()
        {
            (BindingContext as INavigationAware)?.OnNavigatedTo(new NavigationParameters
                    {
                        { "KegSearchCommandRecieverAsync", "KegSearchCommandRecieverAsync" }
                    });
            return true;
        }
    }
}