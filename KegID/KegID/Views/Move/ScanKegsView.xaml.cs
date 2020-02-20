using KegID.LocalDb;
using KegID.Model;
using Prism.Navigation;
using Realms;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ScanKegsView : ContentPage
	{
        Forms9Patch.SinglePicker _singlePicker = new Forms9Patch.SinglePicker
        {
            BackgroundColor = Color.White,
        };

        public List<string> LoadBrandAsync()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            var all = RealmDb.All<BrandModel>().ToList();
            List<string> model = all.Select(x => x.BrandName).ToList();

            if (model.Count > 0)
                return model;
            else
                return null;
        }

        public ScanKegsView ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            _singlePicker.ItemsSource = LoadBrandAsync();
            _singlePicker.PropertyChanged += OnSinglePickerPropertyChanged;
        }

        protected override bool OnBackButtonPressed()
        {
            (BindingContext as INavigationAware)?.OnNavigatedTo(new NavigationParameters
                    {
                        { "DoneCommandRecieverAsync", "DoneCommandRecieverAsync" }
                    });
            return true;
        }

        private void OnSinglePickerPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Forms9Patch.SinglePicker.SelectedItemProperty.PropertyName)
            {
                _singlePickerButton.HtmlText = (_singlePicker.SelectedItem as string) ?? "select content";
                _singlePickerButton.TextColor = _singlePicker.SelectedItem == null ? Color.DarkGray : Color.DarkGray;
            }
        }

        private void _singlePickerButton_Clicked(object sender, System.EventArgs e)
        {
            var bubbleContent = new StackLayout
            {
                WidthRequest = 300,
                HeightRequest = 300,
                Children =
                {
                    _singlePicker
                }
            };
            var bubblePopup = new Forms9Patch.BubblePopup(_singlePickerButton)
            {
                PointerDirection = Forms9Patch.PointerDirection.Vertical,
                Content = bubbleContent,
                BackgroundColor = Color.WhiteSmoke,
            };

            bubblePopup.IsVisible = true;
        }
    }
}