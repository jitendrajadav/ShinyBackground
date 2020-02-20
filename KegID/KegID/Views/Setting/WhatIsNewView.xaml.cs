using KegID.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

        private void myCarouselViewCtrl_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            ((WhatIsNewViewModel)BindingContext).CurrentItemChanged.Execute((ImageClass)e.CurrentItem);
        }

        private void btnNavigation_Clicked(object sender, System.EventArgs e)
        {
            if (myCarouselViewCtrl.Position < ((System.Collections.Generic.List<ImageClass>)myCarouselViewCtrl.ItemsSource).Count - 1)
            {
                myCarouselViewCtrl.Position++;
            }
            ((WhatIsNewViewModel)BindingContext).NextCommand.Execute((ImageClass)myCarouselViewCtrl.CurrentItem);
        }
    }
}