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
                        OnAddMoreTagsClicked("Asset Type");
                        OnAddMoreTagsClicked("Size");
                        OnAddMoreTagsClicked("Contents");
                        OnAddMoreTagsClicked("Batch");
                    }
                    else
                    {
                        OnAddMoreTagsClicked("Asset Type");
                        OnAddMoreTagsClicked("Size");
                    }
                }
            }
        }

        void OnAddMoreTagsClicked(string title)
        {
            grdTag.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });

            Label nameEntry = new Label()
            {
                VerticalOptions = LayoutOptions.Center,
                Text = title
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
                        else if (child.GetType() == typeof(Entry))
                        {
                            if (!string.IsNullOrWhiteSpace(((Entry)child).Text))
                                strValue = strValue + ((Entry)child).Text + " ";
                        }
                    }

                    if (strValue.Split(' ').Count() > 2)
                        sb.Append(strValue);

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