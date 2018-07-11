using KegID.Services;
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
            GenerateDynamicMaintenancePerformed();
        }

        public void GenerateDynamicMaintenancePerformed()
        {
            try
            {
                foreach (var item in ConstantManager.MaintainTypeCollection.Where(x=>x.IsToggled == true))
                {
                    Label PerformedLabel = new Label()
                    {
                        VerticalOptions = LayoutOptions.Center,
                        Text = item.Name,
                        Style = (Style)Application.Current.Resources["LabelTitleStyle"],
                        TextColor = Color.Black
                    };

                    maintenancePerformedStack.Children.Add(PerformedLabel);
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(message: ex.Message);
            }
        }
    }
}