
using Prism.Navigation;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class KegStatusView : ContentPage
    {
        private Forms9Patch.SinglePicker addAlertPicker = new Forms9Patch.SinglePicker
        {
            BackgroundColor = Color.White,
        };

        private Forms9Patch.SinglePicker removeAlertPicker = new Forms9Patch.SinglePicker
        {
            BackgroundColor = Color.White,
        };

        public KegStatusView ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            addAlertPicker.PropertyChanged += OnSinglePickerPropertyChanged;
            removeAlertPicker.PropertyChanged += RemoveAlertPicker_PropertyChanged;
        }

        private void RemoveAlertPicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Forms9Patch.SinglePicker.SelectedItemProperty.PropertyName)
            {
                removeAlertPickerButton.HtmlText = (removeAlertPicker.SelectedItem as string) ?? "Remove alert";
                removeAlertPickerButton.TextColor = removeAlertPicker.SelectedItem == null ? Color.DarkGray : Color.FromHex("#007AFF");
                if (removeAlertPicker.SelectedItem as string != null)
                {
                    ((ViewModel.KegStatusViewModel)removeAlertPickerButton.BindingContext).RemoveSelecetedMaintenance = ((ViewModel.KegStatusViewModel)removeAlertPickerButton.BindingContext).RemoveMaintenanceCollection.Where(x => x.Name == removeAlertPicker.SelectedItem.ToString()).FirstOrDefault();
                }
            }
        }

        protected override bool OnBackButtonPressed()
        {
            (Application.Current.MainPage.Navigation.NavigationStack.Last()?.BindingContext as INavigationAware)?.OnNavigatedTo(new NavigationParameters
                    {
                        { "KegsCommandRecieverAsync", "KegsCommandRecieverAsync" }
                    });
            return true;
        }

        private void OnSinglePickerPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Forms9Patch.SinglePicker.SelectedItemProperty.PropertyName)
            {
                addAlertPickerButton.HtmlText = (addAlertPicker.SelectedItem as string) ?? "Add alert";
                addAlertPickerButton.TextColor = addAlertPicker.SelectedItem == null ? Color.DarkGray : Color.FromHex("#007AFF");
                if (addAlertPicker.SelectedItem as string != null)
                {
                    ((ViewModel.KegStatusViewModel)addAlertPickerButton.BindingContext).SelectedMaintenance = ((ViewModel.KegStatusViewModel)addAlertPickerButton.BindingContext).MaintenanceCollection.Where(x => x.Name == addAlertPicker.SelectedItem.ToString()).FirstOrDefault();
                }
            }
        }

        private void AddAlertPickerButton_Clicked(object sender, System.EventArgs e)
        {
            addAlertPicker.ItemsSource = ((ViewModel.KegStatusViewModel)((BindableObject)sender).BindingContext).MaintenanceCollection.Select(x=>x.Name).ToList();
          
            var bubbleContent = new StackLayout
            {
                WidthRequest = 200,
                HeightRequest = 100,
                Children =
                {
                    addAlertPicker
                },
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            var bubblePopup = new Forms9Patch.BubblePopup(addAlertPickerButton)
            {
                PointerDirection = Forms9Patch.PointerDirection.Vertical,
                Content = bubbleContent,
                BackgroundColor = Color.WhiteSmoke,
            };

            bubblePopup.IsVisible = true;
        }

        private void RemoveAlertPickerButton_Clicked(object sender, System.EventArgs e)
        {
            removeAlertPicker.ItemsSource = ((ViewModel.KegStatusViewModel)((BindableObject)sender).BindingContext).RemoveMaintenanceCollection.Select(x => x.Name).ToList();

            var bubbleContent = new StackLayout
            {
                WidthRequest = 200,
                HeightRequest = 100,
                Children =
                {
                    removeAlertPicker
                },
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            var bubblePopup = new Forms9Patch.BubblePopup(removeAlertPickerButton)
            {
                PointerDirection = Forms9Patch.PointerDirection.Vertical,
                Content = bubbleContent,
                BackgroundColor = Color.WhiteSmoke,
            };

            bubblePopup.IsVisible = true;
        }
    }
}