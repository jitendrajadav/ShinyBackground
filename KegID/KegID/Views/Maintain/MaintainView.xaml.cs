
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MaintainView : ContentPage
	{
		public MaintainView ()
		{
            try
            {
                InitializeComponent();
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
		}
	}
}