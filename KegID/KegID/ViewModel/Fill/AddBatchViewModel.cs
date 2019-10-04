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
        public List<Tag> Tags { get; set; }
        public NewBatch NewBatchModel { get; set; }
       
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
            try
            {
                await _navigationService.NavigateAsync("VolumeView", animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void BrandCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync("BrandView", animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void DoneCommandRecieverAsync()
        {
            if (BrandButtonTitle.ToLower() != "brand")
            {
                try
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
                    NewBatchModel.CompanyId = AppSettings.CompanyId;
                    NewBatchModel.CompletedDate = DateTimeOffset.Now;
                    NewBatchModel.IsCompleted = true;
                    NewBatchModel.PackageDate = PackageDate;
                    NewBatchModel.RecipeId = AppSettings.CompanyId;
                    NewBatchModel.SourceKey = "";

                    await _navigationService.GoBackAsync(new NavigationParameters { { "NewBatchModel", NewBatchModel } }, animated: false);
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
            else
            {
                await _dialogService.DisplayAlertAsync("Error", "Brand is required.", "Ok");
            }
        }

        private async void CancelCommandRecieverAsync()
        {
            try
            {
                await _navigationService.GoBackAsync(animated: false);
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
                        {"viewTypeEnum",ViewTypeEnum.AddBatchView }
                    }, animated: false);
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

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            switch (parameters.Keys.FirstOrDefault())
            {
                case "BrandModel":
                    BrandModel = parameters.GetValue<BrandModel>("BrandModel");
                    break;
                case "VolumeModel":
                    VolumeChar = parameters.GetValue<string>("VolumeModel");
                    break;
                default:
                    break;
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("CancelCommandRecieverAsync"))
            {
                CancelCommandRecieverAsync();
            }
        }

        #endregion
    }
}
