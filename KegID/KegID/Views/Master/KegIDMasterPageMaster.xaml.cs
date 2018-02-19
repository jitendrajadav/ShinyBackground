using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class KegIDMasterPageMaster : ContentPage
    {
        public ListView ListView;
        public KegIDMasterPageMaster()
        {
            InitializeComponent();
            ListView = MenuItemsListView;
        }
    }
}