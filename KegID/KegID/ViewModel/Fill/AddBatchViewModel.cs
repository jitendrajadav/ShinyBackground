using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Model;
using KegID.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class AddBatchViewModel : BaseViewModel
    {
        #region Properties

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

        private string _BatchCode = default(string);

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

        private string _BrewDate = string.Empty;

        /// <summary>
        /// Sets and gets the BrewDate property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string BrewDate
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

        private string _VolumeDigit = default(string);

        /// <summary>
        /// Sets and gets the VolumeDigit property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string VolumeDigit
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

        private string _VolumeChar = default(string);

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

        private DateTime _PackageDate = DateTime.Today;

        /// <summary>
        /// Sets and gets the PackageDate property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DateTime PackageDate
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

        private string _BestByDate = string.Empty;

        /// <summary>
        /// Sets and gets the BestByDate property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string BestByDate
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

        private string _AlcoholContent = default(string);

        /// <summary>
        /// Sets and gets the AlcoholContent property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string AlcoholContent
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

        private string _TagsStr = default(string);

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

        private BatchModel _NewBatchModel = new BatchModel();

        /// <summary>
        /// Sets and gets the NewBatchModel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public BatchModel NewBatchModel
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

        public RelayCommand AddTagsCommand { get;}
        public RelayCommand CancelCommand { get; }
        public RelayCommand DoneCommand { get;}
        public RelayCommand BrandCommand { get; }
        public RelayCommand VolumeCharCommand { get; }

        #endregion

        #region Constructor

        public AddBatchViewModel()
        {
            AddTagsCommand = new RelayCommand(AddTagsCommandRecieverAsync);
            CancelCommand = new RelayCommand(CancelCommandRecieverAsync);
            DoneCommand = new RelayCommand(DoneCommandRecieverAsync);
            BrandCommand = new RelayCommand(BrandCommandRecieverAsync);
            VolumeCharCommand = new RelayCommand(VolumeCharCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void VolumeCharCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new VolumeView());
        }

        private async void BrandCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new BrandView());
        }

        private async void DoneCommandRecieverAsync()
        {
            NewBatchModel.Tags = Tags;
            NewBatchModel.Abv = Convert.ToInt64(AlcoholContent);
            NewBatchModel.BatchCode = BatchCode;
            NewBatchModel.BatchId = Uuid.GetUuId();
            NewBatchModel.BestBeforeDate = BestByDate;
            NewBatchModel.BrandName = BrandButtonTitle;
            NewBatchModel.BrewDate = BrewDate;
            NewBatchModel.BrewedVolume = Convert.ToInt64(VolumeDigit);
            NewBatchModel.BrewedVolumeUom = VolumeChar;
            NewBatchModel.CompanyId = AppSettings.User.CompanyId;
            NewBatchModel.CompletedDate = DateTime.Today.ToString();
            NewBatchModel.IsCompleted = true;
            NewBatchModel.PackageDate = PackageDate.ToString();
            NewBatchModel.PackagedVolume = 12;
            NewBatchModel.PackagedVolumeUom = "";
            NewBatchModel.RecipeId = AppSettings.User.CompanyId;
            NewBatchModel.SourceKey = "";

            //var result = await _fillService.PostBatchAsync(batchRequestModel, Configuration.SessionId, Configuration.NewBatch);
            SimpleIoc.Default.GetInstance<FillViewModel>().NewBatchModel.BrandName = BrandButtonTitle + BatchCode;
            await Application.Current.MainPage.Navigation.PopModalAsync();
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void CancelCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void AddTagsCommandRecieverAsync()
        {
           await Application.Current.MainPage.Navigation.PushModalAsync(new AddTagsView());
        }

        internal void AssignAddTagsValue(List<Tag> _tags, string _tagsStr)
        {
            Tags = _tags;
            TagsStr = _tagsStr;
        }

        #endregion
    }
}
