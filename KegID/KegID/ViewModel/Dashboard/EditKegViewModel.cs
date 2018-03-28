﻿using GalaSoft.MvvmLight.Command;
using KegID.Model;
using KegID.Views;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class EditKegViewModel : BaseViewModel
    {
        #region Properties

        #region KegStatuModel

        /// <summary>
        /// The <see cref="KegStatusModel" /> property's name.
        /// </summary>
        public const string KegStatusModelPropertyName = "KegStatusModel";

        private KegPossessionResponseModel _kegStatusModel = null;

        /// <summary>
        /// Sets and gets the KegStatusModel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public KegPossessionResponseModel KegStatusModel
        {
            get
            {
                return _kegStatusModel;
            }

            set
            {
                if (_kegStatusModel == value)
                {
                    return;
                }

                _kegStatusModel = value;
                RaisePropertyChanged(KegStatusModelPropertyName);
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
                KegStatusModel.PossessorName = PartnerModel.FullName;
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

        public EditKegViewModel()
        {
            CancelCommand = new RelayCommand(CancelCommandRecieverAsync);
            SaveCommand = new RelayCommand(SaveCommandReciever);
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

        private void SaveCommandReciever()
        {
            var vlaue1 = SelectedItemType;
            var vlaue5 = TagsStr;
            var value7 = Tags;
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

        #endregion
    }
}