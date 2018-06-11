using GalaSoft.MvvmLight.Ioc;
using KegID.Model;
using KegID.SQLiteClient;
using KegID.ViewModel;
using Microsoft.AppCenter.Crashes;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
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
            try
            {
                if (Application.Current.MainPage.Navigation.ModalStack.Count > 1)
                {
                    switch ((ViewTypeEnum)Enum.Parse(typeof(ViewTypeEnum), Application.Current.MainPage.Navigation.ModalStack.LastOrDefault().GetType().Name))
                    {
                        case ViewTypeEnum.ScanKegsView:
                        case ViewTypeEnum.FillScanView:
                            if (SimpleIoc.Default.GetInstance<ScanKegsViewModel>().IsFromScanned)
                            {
                                await OnAddMoreTagsClickedAsync(TagsTypeEnum.BestByDate);
                                await OnAddMoreTagsClickedAsync(TagsTypeEnum.ProductionDate);
                                await OnAddMoreTagsClickedAsync(TagsTypeEnum.AssetType);
                                await OnAddMoreTagsClickedAsync(TagsTypeEnum.Size);
                                await OnAddMoreTagsClickedAsync(TagsTypeEnum.Contents);
                                await OnAddMoreTagsClickedAsync(TagsTypeEnum.Batch);
                            }
                            else
                            {
                                await OnAddMoreTagsClickedAsync(TagsTypeEnum.BestByDate);
                                await OnAddMoreTagsClickedAsync(TagsTypeEnum.ProductionDate);
                                await OnAddMoreTagsClickedAsync(TagsTypeEnum.AssetType);
                                await OnAddMoreTagsClickedAsync(TagsTypeEnum.Size);
                            }
                            break;
                        case ViewTypeEnum.AddBatchView:
                            OnAddTagsClicked(null, null);
                            break;
                        case ViewTypeEnum.EditKegView:
                            await OnAddMoreTagsClickedAsync(TagsTypeEnum.Volume);
                            await OnAddMoreTagsClickedAsync(TagsTypeEnum.Note);
                            break;
                        case ViewTypeEnum.MoveView:
                            await OnAddMoreTagsClickedAsync(TagsTypeEnum.BestByDate);
                            await OnAddMoreTagsClickedAsync(TagsTypeEnum.ProductionDate);
                            break;
                        case ViewTypeEnum.BulkUpdateScanView:
                            await OnAddMoreTagsClickedAsync(TagsTypeEnum.Volume);
                            break;
                    }
                }
                else if (Application.Current.MainPage.Navigation.ModalStack.LastOrDefault().GetType().Name == ViewTypeEnum.PalletizeView.ToString())
                {
                    await OnAddMoreTagsClickedAsync(TagsTypeEnum.Zone);
                    await OnAddMoreTagsClickedAsync(TagsTypeEnum.Area);
                    await OnAddMoreTagsClickedAsync(TagsTypeEnum.Slot);
                    await OnAddMoreTagsClickedAsync(TagsTypeEnum.SSCC);
                    await OnAddMoreTagsClickedAsync(TagsTypeEnum.Note);
                    await OnAddMoreTagsClickedAsync(TagsTypeEnum.Batch);
                    await OnAddMoreTagsClickedAsync(TagsTypeEnum.GTIN);
                    await OnAddMoreTagsClickedAsync(TagsTypeEnum.ProductionDate);
                    await OnAddMoreTagsClickedAsync(TagsTypeEnum.ExpiryDate);
                }
                else if (Application.Current.MainPage.Navigation.ModalStack.LastOrDefault().GetType().Name == ViewTypeEnum.MoveView.ToString())
                {
                    await OnAddMoreTagsClickedAsync(TagsTypeEnum.BestByDate);
                    await OnAddMoreTagsClickedAsync(TagsTypeEnum.ProductionDate);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        async Task OnAddMoreTagsClickedAsync(TagsTypeEnum title)
        {
            try
            {
                dynamic valueEntry = null;
                string customeTitle = string.Empty;

                grdTag.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });

                Label nameEntry = new Label()
                {
                    VerticalOptions = LayoutOptions.Center,
                };

                customeTitle = title.ToString();

                switch (title)
                {
                    case TagsTypeEnum.BestByDate:
                        customeTitle = "Best By Date";
                        break;
                    case TagsTypeEnum.ProductionDate:
                        customeTitle = "Production Date";
                        break;
                    case TagsTypeEnum.ExpiryDate:
                        customeTitle = "Expiry Date";
                        break;
                    case TagsTypeEnum.AssetType:
                        customeTitle = "Asset Type";
                        break;
                    case TagsTypeEnum.Size:
                        customeTitle = "Size";
                        break;
                    case TagsTypeEnum.Contents:
                        customeTitle = "Contents";
                        break;
                    case TagsTypeEnum.Batch:
                        customeTitle = "Batch";
                        break;
                }
                nameEntry.Text = customeTitle;
                nameEntry.TextColor = Color.Black;
                nameEntry.FontSize = 13;
                nameEntry.LineBreakMode = LineBreakMode.TailTruncation;

                switch (title)
                {
                    case TagsTypeEnum.BestByDate:
                    case TagsTypeEnum.ProductionDate:
                    case TagsTypeEnum.ExpiryDate:
                        valueEntry = new DatePicker()
                        {
                            VerticalOptions = LayoutOptions.Center,
                        };
                        break;
                    case TagsTypeEnum.AssetType:
                    case TagsTypeEnum.Size:
                    case TagsTypeEnum.Contents:
                    case TagsTypeEnum.Batch:
                        valueEntry = new Picker()
                        {
                            VerticalOptions = LayoutOptions.Center,
                            Title = "Select " + customeTitle
                        };
                        break;
                    case TagsTypeEnum.None:
                        break;
                    default:
                        valueEntry = new Entry()
                        {
                            VerticalOptions = LayoutOptions.Center,
                            AutomationId = "Value"
                        };
                        break;
                }

                switch (title)
                {
                    case TagsTypeEnum.BestByDate:
                    case TagsTypeEnum.ProductionDate:
                    case TagsTypeEnum.ExpiryDate:
                        break;
                    case TagsTypeEnum.AssetType:
                        var assetType = LoadAssetTypeAsync();
                        valueEntry.ItemsSource = assetType.Select(x => x.AssetType).ToList();
                        //valueEntry.ItemDisplayBinding = new Binding("AssetType");
                        break;

                    case TagsTypeEnum.Size:
                        var assetSize = LoadAssetSizeAsync();
                        valueEntry.ItemsSource = assetSize.Select(x => x.AssetSize).ToList();
                        //valueEntry.ItemDisplayBinding = new Binding("AssetSize");
                        break;

                    case TagsTypeEnum.Contents:
                        var result = await SimpleIoc.Default.GetInstance<ScanKegsViewModel>().LoadBrandAsync();
                        valueEntry.ItemsSource = result.ToList();
                        valueEntry.ItemDisplayBinding = new Binding("BrandName");
                        break;

                    case TagsTypeEnum.Batch:
                        var Batchresult = await SimpleIoc.Default.GetInstance<BatchViewModel>().LoadBatchAsync();
                        valueEntry.ItemsSource = Batchresult.ToList();
                        valueEntry.ItemDisplayBinding = new Binding("BrandName");
                        break;

                    default:
                        break;
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
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private IList<AssetSizeModel> LoadAssetSizeAsync()
        {
            var vRealmDb = Realm.GetInstance();
            return vRealmDb.All<AssetSizeModel>().ToList();//await SQLiteServiceClient.Db.Table<AssetSizeModel>().ToListAsync();
        }

        private IList<AssetTypeModel> LoadAssetTypeAsync()
        {
            var vRealmDb = Realm.GetInstance();
            return vRealmDb.All<AssetTypeModel>().ToList();//await SQLiteServiceClient.Db.Table<AssetTypeModel>().ToListAsync();
        }

        void OnAddTagsClicked(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
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
                 Crashes.TrackError(ex);
            }
        }

        async void SaveTagsClickedAsync(object sender, EventArgs e)
        {
            Tag tag = null;
            List<Tag> tags = new List<Tag>();
            string tagsStr = string.Empty;
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
                            if (((Picker)child).SelectedItem != null)
                                if (((Picker)child).SelectedItem.GetType() == typeof(BatchModel))
                                    tag.Value = ((BatchModel)((Picker)child).SelectedItem).BrandName;
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
                    tagsStr = tagsStr + item.Property + item.Value + ";";
                }

                switch ((ViewTypeEnum)Enum.Parse(typeof(ViewTypeEnum), Application.Current.MainPage.Navigation.ModalStack[Application.Current.MainPage.Navigation.ModalStack.Count - 2].GetType().Name))
                {
                    case ViewTypeEnum.ScanKegsView:
                        SimpleIoc.Default.GetInstance<ScanKegsViewModel>().AssignAddTagsValue(tags, tagsStr);
                        break;
                    case ViewTypeEnum.MoveView:
                        SimpleIoc.Default.GetInstance<MoveViewModel>().AssignAddTagsValue(tags, tagsStr);
                        break;
                    case ViewTypeEnum.PalletizeView:
                        SimpleIoc.Default.GetInstance<PalletizeViewModel>().AddInfoTitle = tagsStr;
                        break;
                    case ViewTypeEnum.FillScanView:
                        SimpleIoc.Default.GetInstance<FillScanViewModel>().AssignAddTagsValue(tags, tagsStr);
                        break;
                    case ViewTypeEnum.AddBatchView:
                        SimpleIoc.Default.GetInstance<AddBatchViewModel>().AssignAddTagsValue(tags, tagsStr);
                        break;
                    case ViewTypeEnum.EditKegView:
                        SimpleIoc.Default.GetInstance<EditKegViewModel>().AssignAddTagsValue(tags, tagsStr);
                        break;
                    case ViewTypeEnum.BulkUpdateScanView:
                        SimpleIoc.Default.GetInstance<BulkUpdateScanViewModel>().AssignAddTagsValue(tags, tagsStr);
                        break;
                }

                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                 Crashes.TrackError(ex);
            }
            finally
            {
                tags = null;
                tag = null;
                tagsStr = string.Empty;
            }
        }
    }
}