﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KegID.Response;
using KegID.SQLiteClient;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class ContentTagsViewModel : ViewModelBase
    {
        #region Properties

        #region ContentCollection

        /// <summary>
        /// The <see cref="ContentCollection" /> property's name.
        /// </summary>
        public const string ContentCollectionPropertyName = "ContentCollection";

        private IList<ManifestModel> _ContentCollection = null;

        /// <summary>
        /// Sets and gets the ContentCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<ManifestModel> ContentCollection
        {
            get
            {
                return _ContentCollection;
            }

            set
            {
                if (_ContentCollection == value)
                {
                    return;
                }

                _ContentCollection = value;
                RaisePropertyChanged(ContentCollectionPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public RelayCommand ManifestCommand { get; set; }

        #endregion

        #region Constructor

        public ContentTagsViewModel()
        {
            ManifestCommand = new RelayCommand(ManifestCommandRecieverAsync);
        }

        #endregion

        #region Methods
        public async void LoadContentAsync(string manifestId)
        {
            ContentCollection = await SQLiteServiceClient.Db.Table<ManifestModel>().Where(x=>x.ManifestId == manifestId).ToListAsync();
        }

        private async void ManifestCommandRecieverAsync()
        {
           await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        #endregion
    }
}
