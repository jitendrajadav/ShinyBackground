using KegID.Model;
using Xamarin.Forms;

namespace KegID.Common
{
    public class ManuDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Menu { get; set; }
        public DataTemplate Dashboard { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return ((Dashboard)item)?.Atriskegs != null ? Dashboard : Menu;
        }
    }
}
