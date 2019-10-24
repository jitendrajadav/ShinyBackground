using KegID.Common;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
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
        private readonly IDashboardService _dashboardService;
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
        public string TagsStr { get; set; }
        public List<Tag> Tags { get; set; }

        #endregion

        #region Commands

        public DelegateCommand CancelCommand { get; }
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand PartnerCommand { get; }
        public DelegateCommand SizeCommand { get;}
        public DelegateCommand AddTagsCommand { get;}

        #endregion

        #region Contructor

        public EditKegViewModel(IDashboardService dashboardService, INavigationService navigationService, IPageDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
            _dashboardService = dashboardService;
            CancelCommand = new DelegateCommand(CancelCommandRecieverAsync);
            SaveCommand = new DelegateCommand(SaveCommandRecieverAsync);
            PartnerCommand = new DelegateCommand(PartnerCommandRecieverAsync);
            SizeCommand = new DelegateCommand(SizeCommandRecieverAsync);
            AddTagsCommand = new DelegateCommand(AddTagsCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void CancelCommandRecieverAsync()
        {
            try
            {
                bool accept = await _dialogService.DisplayAlertAsync("Cancel?", Resources["dialog_cancel_message"], "Stay here", "Leave");
                if (!accept)
                    await _navigationService.GoBackAsync(animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void SaveCommandRecieverAsync()
        {
            try
            {
                var model = new KegRequestModel
                {
                    KegId = KegId,
                    Barcode = Barcode,
                    OwnerId = PartnerModel.PartnerId,
                    AltBarcode = Barcode,
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

                var Result = await _dashboardService.PostKegAsync(model, KegId, AppSettings.SessionId, Configuration.Keg);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void PartnerCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync("PartnersView", animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void SizeCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync("SizeView", animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void AddTagsCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync("AddTagsView", new NavigationParameters
                    {
                        {"viewTypeEnum",ViewTypeEnum.EditKegView }
                    }, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal void AssingInitialValue(string _kegId,string _barcode, string _owner, string _typeName, string _sizeName)
        {
            try
            {
                KegId = _kegId;
                Barcode = _barcode;
                Owner = _owner;
                SelectedItemType = _typeName;
                Size = _sizeName;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal void AssignAddTagsValue(List<Tag> _tags, string _tagsStr)
        {
            try
            {
                Tags = _tags;
                TagsStr = _tagsStr;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
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
