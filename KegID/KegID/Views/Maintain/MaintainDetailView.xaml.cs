using KegID.Services;
using Prism.Navigation;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MaintainDetailView : ContentPage
    {
        public MaintainDetailView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            //GenerateDynamicMaintenancePerformed();
        }

        //public void GenerateDynamicMaintenancePerformed()
        //{
        //    var items = ConstantManager.MaintainTypeCollection.Where(x => x.IsToggled).ToList();
        //    try
        //    {
        //        for (int i = 0; i < items.Count; i++)
        //        {
        //            Label PerformedLabel = new Label()
        //            {
        //                VerticalOptions = LayoutOptions.Center,
        //                Text = items[i].Name,
        //                Style = (Style)Application.Current.Resources["LabelTitleStyle"],
        //                TextColor = Color.Black,
        //                Margin = new Thickness(10, 0, 0, 0)
        //            };

        //            BoxView boxView = new BoxView()
        //            {
        //                BackgroundColor = (Color)Application.Current.Resources["bannerBGColor"],
        //                HeightRequest = 1,
        //                VerticalOptions = LayoutOptions.FillAndExpand,
        //                HorizontalOptions = LayoutOptions.FillAndExpand,
        //            };

        //            maintenancePerformedStack.Children.Add(PerformedLabel);
        //            if (items.Count - 1 > i)
        //            {
        //                maintenancePerformedStack.Children.Add(boxView);
        //            }
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        Debug.WriteLine(message: ex.Message);
        //    }
        //    finally
        //    {
        //        items = null;
        //    }
        //}

        protected override bool OnBackButtonPressed()
        {
            (Application.Current.MainPage.Navigation.NavigationStack.Last()?.BindingContext as INavigationAware)?.OnNavigatedTo(new NavigationParameters
                    {
                        { "HomeCommandCommandRecieverAsync", "HomeCommandCommandRecieverAsync" }
                    });
            return true;
        }
    }
}