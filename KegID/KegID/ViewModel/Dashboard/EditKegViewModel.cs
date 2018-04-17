using GalaSoft.MvvmLight.Command;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using KegID.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class EditKegViewModel : BaseViewModel
    {
        #region Properties
        public IDashboardService DashboardService { get; set; }
        public string KegId { get; set; }
        public string Barcode { get; set; }
        public string TypeName { get; set; }
        public string SizeName { get; set; }

        #region Owner

        /// <summary>
        /// The <see cref="Owner" /> property's name.
        /// </summary>
        public const string OwnerPropertyName = "Owner";

        private string _Owner = "Barcode Brewing";

        /// <summary>
        /// Sets and gets the Owner property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Owner
        {
            get
            {
                return _Owner;
            }

            set
            {
                if (_Owner == value)
                {
                    return;
                }

                _Owner = value;
                RaisePropertyChanged(OwnerPropertyName);
            }
        }

        #endregion

        #region Size

        /// <summary>
        /// The <see cref="Size" /> property's name.
        /// </summary>
        public const string SizePropertyName = "Size";

        private string _Size = string.Empty;

        /// <summary>
        /// Sets and gets the Size property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Size
        {
            get
            {
                return _Size;
            }

            set
            {
                if (_Size == value)
                {
                    return;
                }

                _Size = value;
                RaisePropertyChanged(SizePropertyName);
            }
        }

        #endregion

        #region PartnerModel

        /// <summary>
        /// The <see cref="PartnerModel" /> property's name.
        /// </summary>
        public const string PartnerModelPropertyName = "PartnerModel";

        private PartnerModel _PartnerModel = new PartnerModel();

        /// <summary>
        /// Sets and gets the PartnerModel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public PartnerModel PartnerModel
        {
            get
            {
                return _PartnerModel;
            }

            set
            {
                if (_PartnerModel == value)
                {
                    return;
                }

                _PartnerModel = value;
                RaisePropertyChanged(PartnerModelPropertyName);
                Owner = PartnerModel.FullName;
            }
        }

        #endregion

        #region SelectedItemType

        /// <summary>
        /// The <see cref="SelectedItemType" /> property's name.
        /// </summary>
        public const string SelectedItemTypePropertyName = "SelectedItemType";

        private string _SelectedItemType = string.Empty;

        /// <summary>
        /// Sets and gets the SelectedItemType property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string SelectedItemType
        {
            get
            {
                return _SelectedItemType;
            }

            set
            {
                if (_SelectedItemType == value)
                {
                    return;
                }

                _SelectedItemType = value;
                RaisePropertyChanged(SelectedItemTypePropertyName);
            }
        }

        #endregion

        #region TagsStr

        /// <summary>
        /// The <see cref="TagsStr" /> property's name.
        /// </summary>
        public const string TagsStrPropertyName = "TagsStr";

        private string _TagsStr = "Add info";

        /// <summary>
        /// Sets and gets the TagsStr property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string TagsStr
        {
            get
            {
                return _TagsStr;
            }

            set
            {
                if (_TagsStr == value)
                {
                    return;
                }

                _TagsStr = value;
                RaisePropertyChanged(TagsStrPropertyName);
            }
        }

        #endregion

        #region Tags

        /// <summary>
        /// The <see cref="Tags" /> property's name.
        /// </summary>
        public const string TagsPropertyName = "Tags";

        private List<Tag> _Tags = null;

        /// <summary>
        /// Sets and gets the Tags property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public List<Tag> Tags
        {
            get
            {
                return _Tags;
            }

            set
            {
                if (_Tags == value)
                {
                    return;
                }

                _Tags = value;
                RaisePropertyChanged(TagsPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public RelayCommand CancelCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand PartnerCommand { get; }
        public RelayCommand SizeCommand { get;}
        public RelayCommand AddTagsCommand { get;}

        #endregion

        #region Contructor

        public EditKegViewModel(IDashboardService _dashboardService)
        {
            DashboardService = _dashboardService;
            CancelCommand = new RelayCommand(CancelCommandRecieverAsync);
            SaveCommand = new RelayCommand(SaveCommandRecieverAsync);
            PartnerCommand = new RelayCommand(PartnerCommandRecieverAsync);
            SizeCommand = new RelayCommand(SizeCommandRecieverAsync);
            AddTagsCommand = new RelayCommand(AddTagsCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void CancelCommandRecieverAsync()
        {
            var message = Resources["dialog_cancel_message"];
            bool accept = await Application.Current.MainPage.DisplayAlert("Cancel?", Resources["dialog_cancel_message"], "Stay here","Leave");
            if (!accept)
            {
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }

        private async void SaveCommandRecieverAsync()
        {
            var vlaue1 = SelectedItemType;
            var vlaue5 = TagsStr;

            var model = new KegRequestModel
            {
                KegId = KegId,
                Barcode = Barcode,
                OwnerId = PartnerModel.PartnerId,
                AltBarcode = Barcode,
                Notes = "",
                ReferenceKey = "",
                ProfileId = "",
                AssetType = "",
                AssetSize = "",
                AssetVolume = "",
                AssetDescription = "",
                OwnerSkuId = "",
                FixedContents = "",
                Tags = Tags,
                MaintenanceAlertIds = new List<string>(),
                LessorId = "",
                PurchaseDate = DateTime.Now,
                PurchasePrice = 0,
                PurchaseOrder = "",
                ManufacturerName = "",
                ManufacturerId = "",
                ManufactureLocation = "",
                ManufactureDate = DateTimeOffset.Now,
                Material = "",
                Markings = "",
                Colors = ""
            };

          var Result = await DashboardService.PostKegAsync(model, AppSettings.User.SessionId, Configuration.NewKeg);
        }

        private async void PartnerCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new PartnersView());
        }

        private async void SizeCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new SizeView());
        }

        private async void AddTagsCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new AddTagsView());
        }

        internal void AssingInitialValue(string _kegId,string _barcode, string _owner, string _typeName, string _sizeName)
        {
            KegId = _kegId;
            Barcode = _barcode;
            Owner = _owner;
            SelectedItemType = _typeName;
            Size = _sizeName;
        }

        internal void AssignAddTagsValue(List<Tag> _tags, string _tagsStr)
        {
            Tags = _tags;
            TagsStr = _tagsStr;
        }

        #endregion
    }
}
