﻿
using Prism.Navigation;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddBatchView : ContentPage
	{
		public AddBatchView ()
		{
			InitializeComponent ();
		}

        protected override bool OnBackButtonPressed()
        {
            (Application.Current.MainPage.Navigation.ModalStack.Last()?.BindingContext as INavigationAware)?.OnNavigatedTo(new NavigationParameters
                    {
                        { "CancelCommandRecievierAsync", "CancelCommandRecievierAsync" }
                    });
            return true;
        }
    }
}