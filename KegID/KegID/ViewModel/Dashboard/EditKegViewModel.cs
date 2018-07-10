﻿using KegID.Common;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;

namespace KegID.ViewModel
{
    public class EditKegViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private readonly IDashboardService _dashboardService;
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

        public DelegateCommand CancelCommand { get; }
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand PartnerCommand { get; }
        public DelegateCommand SizeCommand { get;}
        public DelegateCommand AddTagsCommand { get;}

        #endregion

        #region Contructor

        public EditKegViewModel(IDashboardService dashboardService, INavigationService navigationService, IPageDialogService dialogService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
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
                    await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
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

                var Result = await _dashboardService.PostKegAsync(model, AppSettings.User.SessionId, Configuration.NewKeg);

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
                await _navigationService.NavigateAsync(new Uri("PartnersView", UriKind.Relative), useModalNavigation: true, animated: false);
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
                await _navigationService.NavigateAsync(new Uri("SizeView", UriKind.Relative), useModalNavigation: true, animated: false);
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
                var param = new NavigationParameters
                    {
                        {"viewTypeEnum",ViewTypeEnum.EditKegView }
                    };
                await _navigationService.NavigateAsync(new Uri("AddTagsView", UriKind.Relative), param, useModalNavigation: true, animated: false);
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

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("model"))
            {
                PartnerModel = parameters.GetValue<PartnerModel>("model");
            }
            if (parameters.ContainsKey("SizeModel"))
            {
                Size = parameters.GetValue<string>("SizeModel");
            }
        }

        #endregion
    }
}
