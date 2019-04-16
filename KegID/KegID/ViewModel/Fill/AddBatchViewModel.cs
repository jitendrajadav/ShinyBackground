using KegID.Common;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KegID.ViewModel
{
    public class AddBatchViewModel : BaseViewModel
    {
        #region Properties

        //private readonly INavigationService _navigationService;
        private readonly IUuidManager _uuidManager;

        #region BrandButtonTitle

        /// <summary>
        /// The <see cref="BrandButtonTitle" /> property's name.
        /// </summary>
        public const string BrandButtonTitlePropertyName = "BrandButtonTitle";

        private string _BrandButtonTitle = "Brand";

        /// <summary>
        /// Sets and gets the BrandButtonTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string BrandButtonTitle
        {
            get
            {
                return _BrandButtonTitle;
            }

            set
            {
                if (_BrandButtonTitle == value)
                {
                    return;
                }

                _BrandButtonTitle = value;
                RaisePropertyChanged(BrandButtonTitlePropertyName);
            }
        }

        #endregion

        #region BatchCode

        /// <summary>
        /// The <see cref="BatchCode" /> property's name.
        /// </summary>
        public const string BatchCodePropertyName = "BatchCode";

        private string _BatchCode = default;

        /// <summary>
        /// Sets and gets the BatchCode property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string BatchCode
        {
            get
            {
                return _BatchCode;
            }

            set
            {
                if (_BatchCode == value)
                {
                    return;
                }

                _BatchCode = value;
                RaisePropertyChanged(BatchCodePropertyName);
            }
        }

        #endregion

        #region BrewDate

        /// <summary>
        /// The <see cref="BrewDate" /> property's name.
        /// </summary>
        public const string BrewDatePropertyName = "BrewDate";

        private DateTimeOffset _BrewDate = DateTime.Now;

        /// <summary>
        /// Sets and gets the BrewDate property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DateTimeOffset BrewDate
        {
            get
            {
                return _BrewDate;
            }

            set
            {
                if (_BrewDate == value)
                {
                    return;
                }

                _BrewDate = value;
                RaisePropertyChanged(BrewDatePropertyName);
            }
        }

        #endregion

        #region VolumeDigit

        /// <summary>
        /// The <see cref="VolumeDigit" /> property's name.
        /// </summary>
        public const string VolumeDigitPropertyName = "VolumeDigit";

        private long _VolumeDigit = default;

        /// <summary>
        /// Sets and gets the VolumeDigit property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public long VolumeDigit
        {
            get
            {
                return _VolumeDigit;
            }

            set
            {
                if (_VolumeDigit == value)
                {
                    return;
                }

                _VolumeDigit = value;
                RaisePropertyChanged(VolumeDigitPropertyName);
            }
        }

        #endregion

        #region VolumeChar

        /// <summary>
        /// The <see cref="VolumeChar" /> property's name.
        /// </summary>
        public const string VolumeCharPropertyName = "VolumeChar";

        private string _VolumeChar = "bbl";

        /// <summary>
        /// Sets and gets the VolumeChar property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string VolumeChar
        {
            get
            {
                return _VolumeChar;
            }

            set
            {
                if (_VolumeChar == value)
                {
                    return;
                }

                _VolumeChar = value;
                RaisePropertyChanged(VolumeCharPropertyName);
            }
        }

        #endregion

        #region PackageDate

        /// <summary>
        /// The <see cref="PackageDate" /> property's name.
        /// </summary>
        public const string PackageDatePropertyName = "PackageDate";

        private DateTimeOffset _PackageDate = DateTime.Today;

        /// <summary>
        /// Sets and gets the PackageDate property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DateTimeOffset PackageDate
        {
            get
            {
                return _PackageDate;
            }

            set
            {
                if (_PackageDate == value)
                {
                    return;
                }

                _PackageDate = value;
                RaisePropertyChanged(PackageDatePropertyName);
            }
        }

        #endregion

        #region BestByDate

        /// <summary>
        /// The <see cref="BestByDate" /> property's name.
        /// </summary>
        public const string BestByDatePropertyName = "BestByDate";

        private DateTimeOffset _BestByDate = DateTime.Now;

        /// <summary>
        /// Sets and gets the BestByDate property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DateTimeOffset BestByDate
        {
            get
            {
                return _BestByDate;
            }

            set
            {
                if (_BestByDate == value)
                {
                    return;
                }

                _BestByDate = value;
                RaisePropertyChanged(BestByDatePropertyName);
            }
        }

        #endregion

        #region AlcoholContent

        /// <summary>
        /// The <see cref="AlcoholContent" /> property's name.
        /// </summary>
        public const string AlcoholContentPropertyName = "AlcoholContent";

        private long _AlcoholContent = default;

        /// <summary>
        /// Sets and gets the AlcoholContent property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public long AlcoholContent
        {
            get
            {
                return _AlcoholContent;
            }

            set
            {
                if (_AlcoholContent == value)
                {
                    return;
                }

                _AlcoholContent = value;
                RaisePropertyChanged(AlcoholContentPropertyName);
            }
        }

        #endregion

        #region TagsStr

        /// <summary>
        /// The <see cref="TagsStr" /> property's name.
        /// </summary>
        public const string TagsStrPropertyName = "TagsStr";

        private string _TagsStr = default;

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

        #region BrandModel

        /// <summary>
        /// The <see cref="BrandModel" /> property's name.
        /// </summary>
        public const string BrandModelPropertyName = "BrandModel";

        private BrandModel _BrandModel = null;

        /// <summary>
        /// Sets and gets the BrandModel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public BrandModel BrandModel
        {
            get
            {
                return _BrandModel;
            }

            set
            {
                if (_BrandModel == value)
                {
                    return;
                }

                _BrandModel = value;
                BrandButtonTitle = _BrandModel.BrandName;
                RaisePropertyChanged(BrandModelPropertyName);
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

        #region NewBatchModel

        /// <summary>
        /// The <see cref="NewBatchModel" /> property's name.
        /// </summary>
        public const string NewBatchModelPropertyName = "NewBatchModel";

        private NewBatch _NewBatchModel = new NewBatch();

        /// <summary>
        /// Sets and gets the NewBatchModel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public NewBatch NewBatchModel
        {
            get
            {
                return _NewBatchModel;
            }

            set
            {
                if (_NewBatchModel == value)
                {
                    return;
                }

                _NewBatchModel = value;
                RaisePropertyChanged(NewBatchModelPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand AddTagsCommand { get;}
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand DoneCommand { get;}
        public DelegateCommand BrandCommand { get; }
        public DelegateCommand VolumeCharCommand { get; }

        #endregion

        #region Constructor

        public AddBatchViewModel(INavigationService navigationService, IUuidManager uuidManager) : base(navigationService)
        {
            //_navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
            _uuidManager = uuidManager;

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
            try
            {
                //NewBatchModel.Tags = Tags;
                NewBatchModel.Abv = AlcoholContent;
                NewBatchModel.BatchCode = BatchCode;
                NewBatchModel.BatchId = _uuidManager.GetUuId();
                NewBatchModel.BestBeforeDate = BestByDate;
                NewBatchModel.BrandName = BrandButtonTitle;
                NewBatchModel.BrewDate = BrewDate;
                NewBatchModel.BrewedVolume = VolumeDigit;
                NewBatchModel.BrewedVolumeUom = VolumeChar;
                NewBatchModel.CompanyId = AppSettings.CompanyId;
                NewBatchModel.CompletedDate = DateTime.Today;
                NewBatchModel.IsCompleted = true;
                NewBatchModel.PackageDate = PackageDate;
                NewBatchModel.PackagedVolume = 12;
                NewBatchModel.PackagedVolumeUom = "";
                NewBatchModel.RecipeId = AppSettings.CompanyId;
                NewBatchModel.SourceKey = "";

                await _navigationService.GoBackAsync(new NavigationParameters { { "NewBatchModel", NewBatchModel } }, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
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
