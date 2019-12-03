
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
                removeAlertPickerButton.TextColor = removeAlertPicker.SelectedItem == null ? Color.DarkGray : Color.FromHex("#007AFF");
                if (removeAlertPicker.SelectedItem is string)
                {
                    ((ViewModel.KegStatusViewModel)removeAlertPickerButton.BindingContext).RemoveSelecetedMaintenance = ((ViewModel.KegStatusViewModel)removeAlertPickerButton.BindingContext).RemoveMaintenanceCollection.Where(x => x.Name == removeAlertPicker.SelectedItem.ToString()).FirstOrDefault();
                }
            }
        }

        protected override bool OnBackButtonPressed()
        {
            (BindingContext as INavigationAware)?.OnNavigatedTo(new NavigationParameters
                    {
                        { "KegsCommandRecieverAsync", "KegsCommandRecieverAsync" }
                    });
            return true;
        }

        private void OnSinglePickerPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Forms9Patch.SinglePicker.SelectedItemProperty.PropertyName)
            {
                addAlertPickerButton.TextColor = addAlertPicker.SelectedItem == null ? Color.DarkGray : Color.FromHex("#007AFF");
                if (addAlertPicker.SelectedItem is string)
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

            var addBubblePopup = new Forms9Patch.BubblePopup(addAlertPickerButton)
            {
                PointerDirection = Forms9Patch.PointerDirection.Vertical,
                Content = bubbleContent,
                BackgroundColor = Color.WhiteSmoke,
            };

            addBubblePopup.IsVisible = true;
            addBubblePopup.Popped += AddBubblePopup_Popped;
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

            var removeBubblePopup = new Forms9Patch.BubblePopup(removeAlertPickerButton)
            {
                PointerDirection = Forms9Patch.PointerDirection.Vertical,
                Content = bubbleContent,
                BackgroundColor = Color.WhiteSmoke,
            };

            removeBubblePopup.IsVisible = true;
            removeBubblePopup.Popped += RemoveBubblePopup_Popped;
        }

        private void RemoveBubblePopup_Popped(object sender, Forms9Patch.PopupPoppedEventArgs e)
        {
            ((ViewModel.KegStatusViewModel)removeAlertPickerButton.BindingContext).RemoveAlertCommand.Execute();
        }

        private void AddBubblePopup_Popped(object sender, Forms9Patch.PopupPoppedEventArgs e)
        {
            ((ViewModel.KegStatusViewModel)addAlertPickerButton.BindingContext).AddAlertCommand.Execute();
        }
    }
}