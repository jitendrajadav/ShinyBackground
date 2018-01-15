using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.View
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