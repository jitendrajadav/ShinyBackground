using KegID.LocalDb;
using KegID.Model;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Realms;
using System;
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
            try
            {
                if (model.Count > 0)
                    return model;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                return null;
            }
            return model;
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
            (Application.Current.MainPage.Navigation.NavigationStack.Last()?.BindingContext as INavigationAware)?.OnNavigatedTo(new NavigationParameters
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
                _singlePickerButton.TextColor = _singlePicker.SelectedItem == null ? Color.DarkGray : Color.Blue;
            }
        }

        private void _singlePickerButton_Clicked(object sender, System.EventArgs e)
        {
            var doneButton = new Forms9Patch.Button
            {
                BackgroundColor = Color.Blue,
                TextColor = Color.White,
                OutlineColor = Color.White,
                OutlineRadius = 4,
                OutlineWidth = 1,
                Text = "Done",
                HorizontalOptions = LayoutOptions.End,
            };
            var cancelButton = new Forms9Patch.Button
            {
                BackgroundColor = Color.Black,
                TextColor = Color.White,
                OutlineColor = Color.White,
                OutlineRadius = 4,
                OutlineWidth = 1,
                Text = "Cancel",
                HorizontalOptions = LayoutOptions.EndAndExpand,
            };
            var buttonBar = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    cancelButton, doneButton
                }
            };
            var bubbleContent = new StackLayout
            {
                WidthRequest = 300,
                HeightRequest = 300,
                Children =
                {
                    buttonBar,
                    _singlePicker
                }
            };
            var bubblePopup = new Forms9Patch.BubblePopup(_singlePickerButton)
            {
                PointerDirection = Forms9Patch.PointerDirection.Vertical,
                Content = bubbleContent,
                BackgroundColor = Color.Black,
            };

            var selectedItemAtStart = _singlePicker.SelectedItem;
            doneButton.Clicked += async (s, args) => await bubblePopup.CancelAsync();
            cancelButton.Clicked += async (s, args) =>
            {
                _singlePicker.SelectedItem = selectedItemAtStart;
                await bubblePopup.CancelAsync();
            };

            bubblePopup.IsVisible = true;
        }
    }
}