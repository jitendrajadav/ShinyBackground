using GalaSoft.MvvmLight.Ioc;
using KegID.ViewModel;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
            if (Application.Current.MainPage.Navigation.ModalStack.Count >= 2)
            {
                if (Application.Current.MainPage.Navigation.ModalStack[1].GetType() == typeof(ScanKegsView))
                {
                    if (SimpleIoc.Default.GetInstance<ScanKegsViewModel>().IsFromScanned)
                    {
                        OnAddMoreTagsClickedAsync("Asset Type");
                        OnAddMoreTagsClickedAsync("Size");
                        OnAddMoreTagsClickedAsync("Contents");
                        OnAddMoreTagsClickedAsync("Batch");
                    }
                    else
                    {
                        OnAddMoreTagsClickedAsync("Asset Type");
                        OnAddMoreTagsClickedAsync("Size");
                    }
                }
            }
        }

        async void OnAddMoreTagsClickedAsync(string title)
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
                TextColor = Color.Blue
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

            Entry nameEntry = new Entry()
            {
                VerticalOptions = LayoutOptions.Center,
            };

            Entry valueEntry = new Entry()
            {
                VerticalOptions = LayoutOptions.Center,
            };

            Button removeButton = new Button()
            {
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.Center,
                Text = "x",
                TextColor = Color.Blue
            };
            removeButton.Clicked += OnRemoveTagsClicked;

            Grid.SetColumn(nameEntry, 0);
            Grid.SetColumn(valueEntry, 1);
            Grid.SetColumn(removeButton, 2);
            grdTag.Children.Add(nameEntry, 0, grdTag.RowDefinitions.Count - 1);
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
            StringBuilder sb = new StringBuilder();
            string strValue = string.Empty;
            try
            {
                var children = grdTag.Children.ToList();
                for (int i = 0; i < grdTag.RowDefinitions.Count; i++)
                {
                    foreach (var child in children.Where(child => Grid.GetRow(child) == i))
                    {
                        if (child.GetType() == typeof(Label))
                            strValue = strValue + ((Label)child).Text;
                        else if (child.GetType() == typeof(DatePicker))
                            strValue = strValue + ((DatePicker)child).Date + "; ";
                        else if (child.GetType() == typeof(Picker))
                        {
                            if (((Picker)child).SelectedItem!= null)
                                strValue = strValue + ((Picker)child).SelectedItem + "; ";
                        }
                        else if (child.GetType() == typeof(Entry))
                        {
                            if (!string.IsNullOrWhiteSpace(((Entry)child).Text))
                                strValue = strValue + ((Entry)child).Text + " ";
                        }
                    }

                    if (strValue.Contains(":"))
                    {
                        if (strValue.Split(' ').Count() > 3)
                            sb.Append(strValue);
                    }
                    else
                    {
                        if (strValue.Split(' ').Count() > 2)
                            sb.Append(strValue);
                    }
                    strValue = string.Empty;
                }

                if (Application.Current.MainPage.Navigation.ModalStack[1].GetType() == typeof(ScanKegsView))
                {
                    SimpleIoc.Default.GetInstance<ScanKegsViewModel>().Tags = sb.ToString();
                    SimpleIoc.Default.GetInstance<ScanKegsViewModel>().IsFromScanned = false;
                }
                else
                    SimpleIoc.Default.GetInstance<MoveViewModel>().Tags = sb.ToString();

                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                sb.Clear();
            }
        }
    }
}