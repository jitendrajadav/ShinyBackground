﻿
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InventoryView : TabbedPage
    {
		public InventoryView ()
		{
			InitializeComponent ();
		}
	}
}