using KegID.Model;
using Prism.Navigation;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MaintainDetailView : ContentPage, IInitialize
    {
        public MaintainDetailView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        public void GenerateDynamicMaintenancePerformed(IList<MaintenanceTypeModel> list)
        {
            var items = list;
            for (int i = 0; i < items.Count; i++)
            {
                Label PerformedLabel = new Label()
                {
                    VerticalOptions = LayoutOptions.Center,
                    Text = items[i].Name,
                    Style = (Style)Application.Current.Resources["LabelTitleStyle"],
                    TextColor = Color.Black,
                    Margin = new Thickness(10, 0, 0, 0)
                };

                BoxView boxView = new BoxView()
                {
                    BackgroundColor = (Color)Application.Current.Resources["bannerBGColor"],
                    HeightRequest = 1,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                };

                maintenancePerformedStack.Children.Add(PerformedLabel);
                if (items.Count - 1 > i)
                {
                    maintenancePerformedStack.Children.Add(boxView);
                }
            }
        }

        public void Initialize(INavigationParameters parameters)
        {
            GenerateDynamicMaintenancePerformed(parameters.GetValue<IList<MaintenanceTypeModel>>("SelectedMaintainenace"));
        }

        protected override bool OnBackButtonPressed()
        {
            (BindingContext as INavigationAware)?.OnNavigatedTo(new NavigationParameters
                    {
                        { "HomeCommandCommandRecieverAsync", "HomeCommandCommandRecieverAsync" }
                    });
            return true;
        }
    }
}