using GalaSoft.MvvmLight.Ioc;
using KegID.Model;
using KegID.ViewModel;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddTagsView : ContentPage
    {
        public AddTagsView()
        {
            InitializeComponent();
            LoadAddTagsAsync();
        }

        private async void LoadAddTagsAsync()
        {
            var value = Application.Current.MainPage.Navigation.ModalStack.Count;
            if (Application.Current.MainPage.Navigation.ModalStack.Count > 2)
            {
                if (Application.Current.MainPage.Navigation.ModalStack[2].GetType() == typeof(ScanKegsView))
                {
                    if (SimpleIoc.Default.GetInstance<ScanKegsViewModel>().IsFromScanned)
                    {
                        await OnAddMoreTagsClickedAsync("Asset Type");
                        await OnAddMoreTagsClickedAsync("Size");
                        await OnAddMoreTagsClickedAsync("Contents");
                        await OnAddMoreTagsClickedAsync("Batch");
                    }
                    else
                    {
                        await OnAddMoreTagsClickedAsync("Asset Type");
                        await OnAddMoreTagsClickedAsync("Size");
                    }
                }
            }
        }

        async Task OnAddMoreTagsClickedAsync(string title)
        {
            dynamic valueEntry;

            grdTag.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });

            Label nameEntry = new Label()
            {
                VerticalOptions = LayoutOptions.Center,
                Text = title
            };
            
            if (!string.IsNullOrEmpty(title))
            {
                valueEntry = new Picker()
                {
                    VerticalOptions = LayoutOptions.Center,
                };

                switch (title)
                {
                    case "Asset Type":
                        valueEntry.Items.Add("Keg");
                        valueEntry.Items.Add("Tap Handle");
                        break;

                    case "Size":
                        valueEntry.Items.Add("1/2 bbl");
                        valueEntry.Items.Add("1/4 bbl");
                        valueEntry.Items.Add("1/6 bbl");
                        valueEntry.Items.Add("30 L");
                        valueEntry.Items.Add("40 L");
                        valueEntry.Items.Add("50 L");
                        break;

                    case "Contents":
                        var result = await SimpleIoc.Default.GetInstance<ScanKegsViewModel>().LoadBrandAsync();
                        valueEntry.ItemsSource = result.ToList();
                        valueEntry.ItemDisplayBinding = new Binding("BrandName"); ;
                        break;

                    case "Batch":
                        var Batchresult = await SimpleIoc.Default.GetInstance<BatchViewModel>().LoadBatchAsync();
                        valueEntry.ItemsSource = Batchresult.ToList();
                        valueEntry.ItemDisplayBinding = new Binding("BrandName"); ;
                        break;

                    default:
                        break;
                }
            }
            else
            {
                valueEntry = new Entry()
                {
                    VerticalOptions = LayoutOptions.Center,
                }; 
            }

            Button removeButton = new Button()
            {
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.Center,
                Text = "x",
                TextColor = (Color)Application.Current.Resources["selectTextColor"] 
            };
            removeButton.Clicked += OnRemoveTagsClicked;

            Grid.SetColumn(nameEntry, 0);
            Grid.SetColumn(valueEntry, 1);
            Grid.SetColumn(removeButton, 2);
            grdTag.Children.Add(nameEntry, 0, grdTag.RowDefinitions.Count - 1);
            grdTag.Children.Add(valueEntry, 1, grdTag.RowDefinitions.Count - 1);
            grdTag.Children.Add(removeButton, 2, grdTag.RowDefinitions.Count - 1);
        }

        void OnAddTagsClicked(object sender, EventArgs e)
        {
            grdTag.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });

            Entry propertyEntry = new Entry()
            {
                VerticalOptions = LayoutOptions.Center,
                 AutomationId = "Property"
            };

            Entry valueEntry = new Entry()
            {
                VerticalOptions = LayoutOptions.Center,
                AutomationId = "Value"
            };

            Button removeButton = new Button()
            {
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.Center,
                Text = "x",
                TextColor = (Color)Application.Current.Resources["selectTextColor"]
            };
            removeButton.Clicked += OnRemoveTagsClicked;

            Grid.SetColumn(propertyEntry, 0);
            Grid.SetColumn(valueEntry, 1);
            Grid.SetColumn(removeButton, 2);
            grdTag.Children.Add(propertyEntry, 0, grdTag.RowDefinitions.Count - 1);
            grdTag.Children.Add(valueEntry, 1, grdTag.RowDefinitions.Count - 1);
            grdTag.Children.Add(removeButton, 2, grdTag.RowDefinitions.Count - 1);
        }

        void OnRemoveTagsClicked(object sender, EventArgs e)
        {
            try
            {
                var button = (Button)sender;
                int row = Grid.GetRow(button);
                var children = grdTag.Children.ToList();
                foreach (var child in children.Where(child => Grid.GetRow(child) == row))
                {
                    grdTag.Children.Remove(child);
                }
                foreach (var child in children.Where(child => Grid.GetRow(child) > row))
                {
                    Grid.SetRow(child, Grid.GetRow(child) - 1);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        async void SaveTagsClickedAsync(object sender, EventArgs e)
        {
            Tag tag = null;
            string tagsStr = string.Empty;
            string tagsStrMove = string.Empty;
            try
            {
                var children = grdTag.Children.ToList();
                for (int i = 0; i < grdTag.RowDefinitions.Count; i++)
                {
                    tag = new Tag();
                    foreach (var child in children.Where(child => Grid.GetRow(child) == i))
                    {
                        if (child.GetType() == typeof(Label))
                            tag.Property = ((Label)child).Text;
                        else if (child.GetType() == typeof(DatePicker))
                            tag.Value = ((DatePicker)child).Date.ToShortDateString();
                        else if (child.GetType() == typeof(Picker))
                        {
                            if (((Picker)child).SelectedItem!= null)
                                tag.Value = ((Picker)child).SelectedItem.ToString() ;
                        }
                        else if (child.GetType() == typeof(Entry))
                        {
                            if (!string.IsNullOrWhiteSpace(((Entry)child).Text))
                            {
                                if (((Entry)child).AutomationId == "Property")
                                    tag.Property = ((Entry)child).Text;
                                else if(((Entry)child).AutomationId == "Value")
                                    tag.Value = ((Entry)child).Text;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(tag.Value) && !string.IsNullOrEmpty(tag.Property))
                    {
                        if (Application.Current.MainPage.Navigation.ModalStack[2].GetType() == typeof(ScanKegsView))
                        {
                            SimpleIoc.Default.GetInstance<ScanKegsViewModel>().Tags.Add(tag);
                            SimpleIoc.Default.GetInstance<ScanKegsViewModel>().IsFromScanned = false;
                        }
                        else
                            SimpleIoc.Default.GetInstance<MoveViewModel>().Tags.Add(tag);
                    }
                }
                
                foreach (var item in SimpleIoc.Default.GetInstance<ScanKegsViewModel>().Tags)
                {
                    tagsStr = tagsStr + item.Property + item.Value + ";";
                }
                foreach (var item in SimpleIoc.Default.GetInstance<MoveViewModel>().Tags)
                {
                    tagsStrMove = tagsStrMove + item.Property + item.Value + ";";
                }
                SimpleIoc.Default.GetInstance<ScanKegsViewModel>().TagsStr = tagsStr;
                SimpleIoc.Default.GetInstance<MoveViewModel>().TagsStr = tagsStrMove;
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                tag = null;
                tagsStr = string.Empty;
                tagsStrMove = string.Empty;
            }
        }
    }
}