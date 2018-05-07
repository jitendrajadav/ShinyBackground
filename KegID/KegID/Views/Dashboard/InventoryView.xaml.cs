
using GalaSoft.MvvmLight.Ioc;
using KegID.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InventoryView : TabbedPage
    {
        public InventoryView()
        {
            InitializeComponent();
            CurrentPage = Children[SimpleIoc.Default.GetInstance<InventoryViewModel>().CurrentPage];
        }
    }
}