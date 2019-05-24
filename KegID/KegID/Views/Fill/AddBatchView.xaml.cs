
using KegID.LocalDb;
using KegID.Model;
using Newtonsoft.Json;
using Prism.Navigation;

using Realms;

using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddBatchView : ContentPage
    {
        public AddBatchView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            LoadRequiredTag();
        }

        private void LoadRequiredTag()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            var preference = RealmDb.All<Preference>().Where(x => x.PreferenceName == "DefaultBatchTags").FirstOrDefault();
            var preferenceValue = JsonConvert.DeserializeObject<PreferenceTags>(preference.PreferenceValue);

            foreach (var tag in preferenceValue.Tags)
            {
                requiredTagGrd.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
                dynamic valueEntry = null;

                Label labelTitle = new Label()
                {
                    VerticalOptions = LayoutOptions.Center,
                    Text = tag.Name,
                    TextColor = Color.Black,
                    FontSize = 18,
                    LineBreakMode = LineBreakMode.TailTruncation,
                };

                switch (tag.Type)
                {
                    case "Freetext":
                        valueEntry = new Entry()
                        {
                            VerticalOptions = LayoutOptions.Center,
                            AutomationId = "Value",
                        };
                        if (tag.DefaultValue != null)
                            valueEntry.Text = tag.DefaultValue.ToString();
                        break;

                    case "SizeList":
                        valueEntry = new Picker()
                        {
                            VerticalOptions = LayoutOptions.Center,
                            Title = "Select PartnerList"
                        };
                        var assetSizes = RealmDb.All<AssetSizeModel>().ToList();
                        valueEntry.ItemsSource = assetSizes.ToList();
                        valueEntry.ItemDisplayBinding = new Binding("AssetSize");
                        break;

                    case "OwnerList":
                        valueEntry = new Picker()
                        {
                            VerticalOptions = LayoutOptions.Center,
                            Title = "Select PartnerList"
                        };
                        var owners = RealmDb.All<PartnerModel>().ToList();
                        valueEntry.ItemsSource = owners.ToList();
                        valueEntry.ItemDisplayBinding = new Binding("FullName");
                        break;

                    case "TypeList":
                        valueEntry = new Picker()
                        {
                            VerticalOptions = LayoutOptions.Center,
                            Title = "Select PartnerList"
                        };
                        var assetTypes = RealmDb.All<AssetTypeModel>().ToList();
                        valueEntry.ItemsSource = assetTypes.ToList();
                        valueEntry.ItemDisplayBinding = new Binding("AssetType");
                        break;

                    case "PartnerList":
                        valueEntry = new Picker()
                        {
                            VerticalOptions = LayoutOptions.Center,
                            Title = "Select PartnerList",
                        };
                        var partners = RealmDb.All<PartnerModel>().ToList();
                        valueEntry.ItemsSource = partners.ToList();
                        valueEntry.ItemDisplayBinding = new Binding("FullName");
                        break;

                    case "Date":
                        valueEntry = new DatePicker()
                        {
                            VerticalOptions = LayoutOptions.Center,
                        };
                        if (tag.DefaultValue != null)
                        {
                            valueEntry.Date = Convert.ToDateTime(tag.DefaultValue);
                        }
                        break;

                    case "Number":
                        valueEntry = new Entry()
                        {
                            VerticalOptions = LayoutOptions.Center,
                            AutomationId = "NumberValue",
                            Keyboard = Keyboard.Numeric,
                        };
                        if (tag.DefaultValue != null)
                            valueEntry.Text = tag.DefaultValue.ToString();
                        break;

                    case "TrueFalse":
                        valueEntry = new Switch()
                        {
                            VerticalOptions = LayoutOptions.Center,
                            AutomationId = "TrueFalseValue",

                        };
                        if (tag.DefaultValue != null)
                        {
                            valueEntry.IsToggled = Convert.ToBoolean(tag.DefaultValue);
                        }
                        break;

                    default:
                        break;
                }

                Grid.SetColumn(labelTitle, 0);
                Grid.SetColumn(valueEntry, 1);
                requiredTagGrd.Children.Add(labelTitle, 0, requiredTagGrd.RowDefinitions.Count - 1);
                requiredTagGrd.Children.Add(valueEntry, 1, requiredTagGrd.RowDefinitions.Count - 1);
                Grid.SetColumnSpan(valueEntry, 2);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            (Application.Current.MainPage.Navigation.NavigationStack.Last()?.BindingContext as INavigationAware)?.OnNavigatedTo(new NavigationParameters
                    {
                        { "CancelCommandRecievierAsync", "CancelCommandRecievierAsync" }
                    });
            return true;
        }

        private void LblDone_Clicked(object sender, EventArgs e)
        {
            bool tagsRequired = false;
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            var preference = RealmDb.All<Preference>().Where(x => x.PreferenceName == "DefaultBatchTags").FirstOrDefault();
            var preferenceValue = JsonConvert.DeserializeObject<PreferenceTags>(preference.PreferenceValue);

            var children = requiredTagGrd.Children.ToList();
            for (int i = 7; i < requiredTagGrd.RowDefinitions.Count; i++)
            {
                string name = string.Empty;
                foreach (var child in children.Where(child => Grid.GetRow(child) == i))
                {
                    if (child.GetType() == typeof(Label))
                        name = ((Label)child).Text;

                    var tag = preferenceValue.Tags.Where(x => x.Name == name).FirstOrDefault();

                    if (tag.Required)
                    {
                        if (child.GetType() == typeof(Entry))
                        {
                            if (string.IsNullOrEmpty(((Entry)child).Text))
                            {
                                tagsRequired = true;
                            }
                        }
                        else if (child.GetType() == typeof(Picker))
                        {
                            if (((Picker)child).SelectedItem == null)
                            {
                                tagsRequired = true;
                            }
                        }
                    }
                }
            }
            if (tagsRequired)
            {
                Application.Current.MainPage.DisplayAlert("Warning", "Required tag missing\n", "Ok");
            }
            else
                ((ViewModel.AddBatchViewModel)((BindableObject)sender).BindingContext).DoneCommand.Execute();
        }
    }
}