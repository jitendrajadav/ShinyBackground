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
    public class AddBatchViewModel : BaseViewModel
    {
        #region Properties

        private readonly IUuidManager _uuidManager;
        private readonly IPageDialogService _dialogService;
        public string BrandButtonTitle { get; set; } = "Brand";
        public string BatchCode { get; set; } = "BatchCode";
        public DateTimeOffset BrewDate { get; set; } = DateTime.Now;
        public string VolumeDigit { get; set; }
        public string VolumeChar { get; set; } = "bbl";
        public DateTimeOffset PackageDate { get; set; } = DateTime.Today;
        public DateTime? BestByDate { get; set; }
        public string AlcoholContent { get; set; }
        public string TagsStr { get; set; }
        public BrandModel BrandModel { get; set; }
        public void OnBrandModelChanged()
        {
            BrandButtonTitle = BrandModel.BrandName;
        }
        public List<Tag> Tags { get; set; }
        public NewBatch NewBatchModel { get; set; } = new NewBatch();

        #endregion

        #region Commands

        public DelegateCommand AddTagsCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand DoneCommand { get; }
        public DelegateCommand BrandCommand { get; }
        public DelegateCommand VolumeCharCommand { get; }

        #endregion

        #region Constructor

        public AddBatchViewModel(INavigationService navigationService, IUuidManager uuidManager, IPageDialogService dialogService) : base(navigationService)
        {
            _uuidManager = uuidManager;
            _dialogService = dialogService;

            AddTagsCommand = new DelegateCommand(AddTagsCommandRecieverAsync);
            CancelCommand = new DelegateCommand(CancelCommandRecieverAsync);
            DoneCommand = new DelegateCommand(DoneCommandRecieverAsync);
            BrandCommand = new DelegateCommand(BrandCommandRecieverAsync);
            VolumeCharCommand = new DelegateCommand(VolumeCharCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void VolumeCharCommandRecieverAsync()
        {
            await _navigationService.NavigateAsync("VolumeView", animated: false);
        }

        private async void BrandCommandRecieverAsync()
        {
            await _navigationService.NavigateAsync("BrandView", animated: false);
        }

        private async void DoneCommandRecieverAsync()
        {
            if (!string.Equals(BrandButtonTitle, "brand", StringComparison.OrdinalIgnoreCase))
            {
                var abv = AlcoholContent ?? "";
                NewBatchModel.Abv = abv;
                NewBatchModel.BatchCode = BatchCode;
                NewBatchModel.BatchId = _uuidManager.GetUuId();
                NewBatchModel.BestBeforeDate = BestByDate ?? DateTime.Now;
                NewBatchModel.BrandName = BrandButtonTitle;
                NewBatchModel.BrewDate = BrewDate;
                NewBatchModel.BrewedVolume = VolumeDigit;
                NewBatchModel.BrewedVolumeUom = VolumeChar;
                NewBatchModel.CompanyId = Settings.CompanyId;
                NewBatchModel.CompletedDate = DateTimeOffset.Now;
                NewBatchModel.IsCompleted = true;
                NewBatchModel.PackageDate = PackageDate;
                NewBatchModel.RecipeId = Settings.CompanyId;
                NewBatchModel.SourceKey = "";

                await _navigationService.GoBackAsync(new NavigationParameters { { "NewBatchModel", NewBatchModel } }, animated: false);
            }
            else
            {
                await _dialogService.DisplayAlertAsync("Error", "Brand is required.", "Ok");
            }
        }

        private async void CancelCommandRecieverAsync()
        {
            await _navigationService.GoBackAsync(animated: false);
        }

        private async void AddTagsCommandRecieverAsync()
        {
            await _navigationService.NavigateAsync("AddTagsView", new NavigationParameters
                    {
                        {"viewTypeEnum",ViewTypeEnum.AddBatchView }
                    }, animated: false);
        }

        internal void AssignAddTagsValue(List<Tag> _tags, string _tagsStr)
        {
            Tags = _tags;
            TagsStr = _tagsStr;
        }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            if (parameters.Keys.Contains("VolumeModel"))
            {
                VolumeChar = parameters.GetValue<string>("VolumeModel");
            }
            return base.InitializeAsync(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            switch (parameters.Keys.FirstOrDefault())
            {
                case "BrandModel":
                    BrandModel = parameters.GetValue<BrandModel>("BrandModel");
                    break;
                case "CancelCommandRecieverAsync":
                    CancelCommandRecieverAsync();
                    break;
            }
        }

        #endregion
    }
}
