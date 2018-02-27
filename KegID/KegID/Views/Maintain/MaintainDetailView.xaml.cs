
using GalaSoft.MvvmLight.Ioc;
using KegID.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MaintainDetailView : ContentPage
	{
		public MaintainDetailView()
		{
			InitializeComponent ();
            GenerateDynamicMaintenancePerformed();

        }

        public void GenerateDynamicMaintenancePerformed()
        {
            //maintenancePerformed;

            foreach (var item in SimpleIoc.Default.GetInstance<MaintainViewModel>().MaintenancePerformedCollection)
            {
                Label PerformedLabel = new Label()
                {
                    VerticalOptions = LayoutOptions.Center,
                    Text = item
                };

                maintenancePerformedStack.Children.Add(PerformedLabel);
            }

        }
    }
}