using KegID.LocalDb;
using KegID.Model;
using KegID.Services;
using KegID.ViewModel;
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
    public partial class AddTagsView : ContentPage, IInitialize
    {
        BarcodeModel barcodeModel = null;
        public AddTagsView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override bool OnBackButtonPressed()
        {
            SaveTagsClickedAsync(null, null);
            return true;
        }

        private void LoadAddTagsAsync(ViewTypeEnum viewTypeEnum, BarcodeModel barcode = null)
        {
            barcodeModel = barcode;
            switch (viewTypeEnum)
            {
                case ViewTypeEnum.ScanKegsView:
                case ViewTypeEnum.FillScanView:
                    if (ConstantManager.IsFromScanned)
                    {
                        OnAddMoreTagsClickedAsync(TagsTypeEnum.BestByDate, barcode?.Tags?.ToList().Find(x => x.Property == "Best By Date")?.Value);
                        OnAddMoreTagsClickedAsync(TagsTypeEnum.ProductionDate, barcode?.Tags?.ToList().Find(x => x.Property == "Production Date")?.Value);
                        OnAddMoreTagsClickedAsync(TagsTypeEnum.AssetType, barcode?.Tags?.ToList().Find(x => x.Property == "Asset Type")?.Value);
                        OnAddMoreTagsClickedAsync(TagsTypeEnum.Size, barcode?.Tags?.ToList().Find(x => x.Property == "Size")?.Value);
                        OnAddMoreTagsClickedAsync(TagsTypeEnum.Contents, barcode?.Tags?.ToList().Find(x => x.Property == "Contents")?.Value);
                        OnAddMoreTagsClickedAsync(TagsTypeEnum.Batch, barcode?.Tags?.ToList().Find(x => x.Property == "Batch")?.Value);
                    }
                    else
                    {
                        OnAddMoreTagsClickedAsync(TagsTypeEnum.BestByDate, barcode?.Tags?.ToList().Find(x => x.Property == "Best By Date")?.Value);
                        OnAddMoreTagsClickedAsync(TagsTypeEnum.ProductionDate, barcode?.Tags?.ToList().Find(x => x.Property == "Production Date")?.Value);
                        OnAddMoreTagsClickedAsync(TagsTypeEnum.AssetType, barcode?.Tags?.ToList().Find(x => x.Property == "Asset Type")?.Value);
                        OnAddMoreTagsClickedAsync(TagsTypeEnum.Size, barcode?.Tags?.ToList().Find(x => x.Property == "Size")?.Value);
                    }
                    break;
                case ViewTypeEnum.AddBatchView:
                    OnAddTagsClicked(null, null);
                    break;
                case ViewTypeEnum.EditKegView:
                    OnAddMoreTagsClickedAsync(TagsTypeEnum.Volume);
                    OnAddMoreTagsClickedAsync(TagsTypeEnum.Note);
                    break;
                case ViewTypeEnum.MoveView:
                    OnAddMoreTagsClickedAsync(TagsTypeEnum.BestByDate);
                    OnAddMoreTagsClickedAsync(TagsTypeEnum.ProductionDate);
                    break;
                case ViewTypeEnum.BulkUpdateScanView:
                    OnAddMoreTagsClickedAsync(TagsTypeEnum.Volume);
                    break;
                case ViewTypeEnum.PalletizeView:
                    OnAddMoreTagsClickedAsync(TagsTypeEnum.Zone);
                    OnAddMoreTagsClickedAsync(TagsTypeEnum.Area);
                    OnAddMoreTagsClickedAsync(TagsTypeEnum.Slot);
                    OnAddMoreTagsClickedAsync(TagsTypeEnum.SSCC);
                    OnAddMoreTagsClickedAsync(TagsTypeEnum.Note);
                    OnAddMoreTagsClickedAsync(TagsTypeEnum.Batch);
                    OnAddMoreTagsClickedAsync(TagsTypeEnum.GTIN);
                    OnAddMoreTagsClickedAsync(TagsTypeEnum.ProductionDate);
                    OnAddMoreTagsClickedAsync(TagsTypeEnum.ExpiryDate);
                    break;
                case ViewTypeEnum.MaintainScanView:
                    OnAddMoreTagsClickedAsync(TagsTypeEnum.BestByDate, barcode?.Tags?.ToList().Find(x => x.Property == "Best By Date")?.Value);
                    OnAddMoreTagsClickedAsync(TagsTypeEnum.ProductionDate, barcode?.Tags?.ToList().Find(x => x.Property == "Production Date")?.Value);
                    OnAddMoreTagsClickedAsync(TagsTypeEnum.AssetType, barcode?.Tags?.ToList().Find(x => x.Property == "Asset Type")?.Value);
                    OnAddMoreTagsClickedAsync(TagsTypeEnum.Size, barcode?.Tags?.ToList().Find(x => x.Property == "Size")?.Value);
                    OnAddMoreTagsClickedAsync(TagsTypeEnum.Contents, barcode?.Tags?.ToList().Find(x => x.Property == "Contents")?.Value);
                    break;
            }

        }

        private void OnAddMoreTagsClickedAsync(TagsTypeEnum title, string selectedValue = "")
        {
            dynamic valueEntry = null;
            string customeTitle = string.Empty;

            grdTag.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });

            customeTitle = title.ToString();

            switch (title)
            {
                case TagsTypeEnum.BestByDate:
                    customeTitle = "Best By Date";
                    valueEntry = new DatePicker()
                    {
                        VerticalOptions = LayoutOptions.Center,
                    };
                    if (!string.IsNullOrEmpty(selectedValue))
                    {
                        valueEntry.Date = Convert.ToDateTime(selectedValue);
                    }
                    break;
                case TagsTypeEnum.ProductionDate:
                    customeTitle = "Production Date";
                    valueEntry = new DatePicker()
                    {
                        VerticalOptions = LayoutOptions.Center,
                    };
                    if (!string.IsNullOrEmpty(selectedValue))
                    {
                        valueEntry.Date = Convert.ToDateTime(selectedValue);
                    }
                    break;
                case TagsTypeEnum.ExpiryDate:
                    customeTitle = "Expiry Date";
                    valueEntry = new DatePicker()
                    {
                        VerticalOptions = LayoutOptions.Center,
                    };
                    if (!string.IsNullOrEmpty(selectedValue))
                    {
                        valueEntry.Date = Convert.ToDateTime(selectedValue);
                    }
                    break;
                case TagsTypeEnum.AssetType:
                    customeTitle = "Asset Type";
                    valueEntry = new Picker()
                    {
                        VerticalOptions = LayoutOptions.Center,
                        Title = "Select " + customeTitle
                    };
                    var assetType = LoadAssetTypeAsync();
                    valueEntry.ItemsSource = assetType.Select(x => x.AssetType).ToList();
                    if (!string.IsNullOrEmpty(selectedValue))
                    {
                        valueEntry.SelectedItem = selectedValue;
                    }
                    break;
                case TagsTypeEnum.Size:
                    customeTitle = "Size";
                    valueEntry = new Picker()
                    {
                        VerticalOptions = LayoutOptions.Center,
                        Title = "Select " + customeTitle
                    };
                    var assetSize = LoadAssetSizeAsync();
                    valueEntry.ItemsSource = assetSize.Select(x => x.AssetSize).ToList();
                    if (!string.IsNullOrEmpty(selectedValue))
                    {
                        valueEntry.SelectedItem = selectedValue;
                    }
                    break;
                case TagsTypeEnum.Contents:
                    customeTitle = "Contents";
                    valueEntry = new Picker()
                    {
                        VerticalOptions = LayoutOptions.Center,
                        Title = "Select " + customeTitle
                    };
                    var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                    var result = RealmDb.All<BrandModel>().ToList();
                    valueEntry.ItemsSource = result.ToList();
                    valueEntry.ItemDisplayBinding = new Binding("BrandName");
                    if (!string.IsNullOrEmpty(selectedValue))
                    {
                        valueEntry.SelectedItem = result.ToList().Find(x => x.BrandName == selectedValue);
                    }
                    break;
                case TagsTypeEnum.Batch:
                    if (!string.IsNullOrEmpty(selectedValue))
                    {
                        valueEntry = new Entry()
                        {
                            VerticalOptions = LayoutOptions.Center,
                            AutomationId = "Value",
                            Text = selectedValue
                        };
                    }
                    else
                    {
                        customeTitle = "Batch";
                        valueEntry = new Picker()
                        {
                            VerticalOptions = LayoutOptions.Center,
                            Title = "Select " + customeTitle
                        };
                        var RealmDb1 = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                        var Batchresult = RealmDb1.All<NewBatch>().ToList();
                        valueEntry.ItemsSource = Batchresult.ToList();
                        valueEntry.ItemDisplayBinding = new Binding("BrandName");
                    }
                    break;
                default:
                    valueEntry = new Entry()
                    {
                        VerticalOptions = LayoutOptions.Center,
                        AutomationId = "Value"
                    };
                    break;
            }

            Label nameEntry = new Label()
            {
                VerticalOptions = LayoutOptions.Center,
                Text = customeTitle,
                TextColor = Color.Black,
                FontSize = 13,
                LineBreakMode = LineBreakMode.TailTruncation,
            };

            Button removeButton = new Button()
            {
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                FontSize = 25,
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

        private IList<AssetSizeModel> LoadAssetSizeAsync()
        {
            Realm RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            return RealmDb.All<AssetSizeModel>().ToList();
        }

        private IList<AssetTypeModel> LoadAssetTypeAsync()
        {
            Realm RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            return RealmDb.All<AssetTypeModel>().ToList();
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
                HorizontalOptions = LayoutOptions.Start,
                FontSize = 25,
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

        private void OnRemoveTagsClicked(object sender, EventArgs e)
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

        private void SaveTagsClickedAsync(object sender, EventArgs e)
        {
            Tag tag = null;
            List<Tag> tags = new List<Tag>();
            string tagsStr = string.Empty;

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
                        if (((Picker)child).SelectedItem != null)
                            if (((Picker)child).SelectedItem.GetType() == typeof(NewBatch))
                                tag.Value = ((NewBatch)((Picker)child).SelectedItem).BrandName;
                            else if (((Picker)child).SelectedItem.GetType() == typeof(BrandModel))
                                tag.Value = ((BrandModel)((Picker)child).SelectedItem).BrandName;
                            else
                                tag.Value = ((Picker)child).SelectedItem.ToString();
                    }

                    else if (child.GetType() == typeof(Entry))
                    {
                        if (!string.IsNullOrWhiteSpace(((Entry)child).Text))
                        {
                            if (((Entry)child).AutomationId == "Property")
                                tag.Property = ((Entry)child).Text;
                            else if (((Entry)child).AutomationId == "Value")
                                tag.Value = ((Entry)child).Text;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(tag.Value) && !string.IsNullOrEmpty(tag.Property))
                    tags.Add(tag);
            }

            foreach (var item in tags)
            {
                tagsStr = tagsStr + item.Property + " : " + item.Value + " ; ";
            }
            ConstantManager.Tags = tags;
            ConstantManager.TagsStr = tagsStr;
            ((AddTagsViewModel)BindingContext).Barcode = barcodeModel?.Barcode ?? "";
            ((AddTagsViewModel)BindingContext).SaveCommand.Execute();
        }

        public void Initialize(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("viewTypeEnum"))
            {
                if (parameters.ContainsKey("AddTagsViewInitialValue"))
                {
                    LoadAddTagsAsync(parameters.GetValue<ViewTypeEnum>("viewTypeEnum"), parameters.GetValue<BarcodeModel>("AddTagsViewInitialValue"));
                }
                else
                {
                    LoadAddTagsAsync(parameters.GetValue<ViewTypeEnum>("viewTypeEnum"));
                }
            }
        }
    }
}