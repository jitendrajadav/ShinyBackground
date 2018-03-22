using KegID.Model;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KegID.Controls
{
    /// <summary>
    /// Xamarin.Forms representation for a custom-renderer that uses 
    /// the native list control on each platform.
    /// </summary>
    public class NativeListView : ListView
    {
        public static readonly BindableProperty ItemsProperty =
            BindableProperty.Create("Items", typeof(IEnumerable<PartnerModel>), typeof(NativeListView), new List<PartnerModel>());

        public IEnumerable<PartnerModel> Items
        {
            get { return (IEnumerable<PartnerModel>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public new event EventHandler<SelectedItemChangedEventArgs> ItemSelected;

        public void NotifyItemSelected(object item)
        {
            ItemSelected?.Invoke(this, new SelectedItemChangedEventArgs(item));
        }
    }

}
