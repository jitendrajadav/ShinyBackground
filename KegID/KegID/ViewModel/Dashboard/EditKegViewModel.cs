using KegID.Common;
using KegID.Model;
using KegID.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KegID.ViewModel
{
    public class EditKegViewModel : BaseViewModel
    {
        #region Properties

        private readonly IPageDialogService _dialogService;
        public string KegId { get; set; }
        public string Barcode { get; set; }
        public string TypeName { get; set; }
        public string SizeName { get; set; }

        public string Owner { get; set; }
        public string Size { get; set; }
        public PartnerModel PartnerModel { get; set; } = new PartnerModel();
        public void OnPartnerModelChanged()
        {
            Owner = PartnerModel?.FullName;
        }
        public string SelectedItemType { get; set; }
        public string TagsStr { get; set; } = "Add info";
        public string AltBarcode { get; set; }
        public List<Tag> Tags { get; set; }

        #endregion

        #region Commands

        public DelegateCommand CancelCommand { get; }
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand PartnerCommand { get; }
        public DelegateCommand SizeCommand { get; }
        public DelegateCommand AddTagsCommand { get; }

        #endregion

        #region Contructor

        public EditKegViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
            CancelCommand = new DelegateCommand(CancelCommandRecieverAsync);
            SaveCommand = new DelegateCommand(async () => await RunSafe(SaveCommandRecieverAsync()));
            PartnerCommand = new DelegateCommand(PartnerCommandRecieverAsync);
            SizeCommand = new DelegateCommand(SizeCommandRecieverAsync);
            AddTagsCommand = new DelegateCommand(AddTagsCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void CancelCommandRecieverAsync()
        {
            bool accept = await _dialogService.DisplayAlertAsync("Cancel?", Resources["dialog_cancel_message"], "Stay here", "Leave");
            if (!accept)
                await NavigationService.GoBackAsync(animated: false);
        }

        private async Task SaveCommandRecieverAsync()
        {
            var model = new KegRequestModel
            {
                KegId = KegId,
                Barcode = Barcode,
                OwnerId = PartnerModel.PartnerId,
                AltBarcode = AltBarcode,
                Notes = "",
                ReferenceKey = "",
                ProfileId = "",
                AssetType = SelectedItemType,
                AssetSize = Size,
                AssetVolume = "",
                AssetDescription = "",
                OwnerSkuId = "",
                FixedContents = "",
                Tags = Tags,
                MaintenanceAlertIds = new List<string>(),
                LessorId = "",
                PurchaseDate = DateTimeOffset.Now,
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

            var Result = await ApiManager.PostKegStatus(model, KegId, Settings.SessionId);
            await NavigationService.GoBackAsync(new NavigationParameters
                    {
                        { "TagsStr", TagsStr },
                        { "Owner", Owner },
                        { "Size",  Size},
                        { "Type",SelectedItemType},
                        { "AltBarcode",AltBarcode}
                    }, animated: false);
        }

        private async void PartnerCommandRecieverAsync()
        {
            await NavigationService.NavigateAsync("PartnersView", animated: false);
        }

        private async void SizeCommandRecieverAsync()
        {
            await NavigationService.NavigateAsync("SizeView", animated: false);
        }

        private async void AddTagsCommandRecieverAsync()
        {
            await NavigationService.NavigateAsync("AddTagsView", new NavigationParameters
                    {
                        {"viewTypeEnum",ViewTypeEnum.EditKegView }
                    }, animated: false);
        }

        internal void AssingInitialValue(string _kegId, string _barcode, string _owner, string _typeName, string _sizeName)
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

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("AssignInitialValue"))
            {
                AssingInitialValue(parameters.GetValue<string>("KegId"), parameters.GetValue<string>("AssignInitialValue"), parameters.GetValue<string>("Owner"), parameters.GetValue<string>("TypeName"), parameters.GetValue<string>("SizeName"));
            }
            return base.InitializeAsync(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            switch (parameters.Keys.FirstOrDefault())
            {
                case "model":
                    PartnerModel = parameters.GetValue<PartnerModel>("model");
                    break;
                case "SizeModel":
                    Size = parameters.GetValue<string>("SizeModel");
                    break;
                case "AddTags":
                    AssignAddTagsValue(ConstantManager.Tags, ConstantManager.TagsStr);
                    break;
                case "CancelCommandRecieverAsync":
                    CancelCommandRecieverAsync();
                    break;
            }
        }

        #endregion
    }
}
