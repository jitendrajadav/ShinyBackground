﻿using GalaSoft.MvvmLight.Command;
using KegID.Model;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class ScanInfoViewModel  : BaseViewModel
    {
        #region Properties

        #region Barcode

        /// <summary>
        /// The <see cref="Barcode" /> property's name.
        /// </summary>
        public const string BarcodePropertyName = "Barcode";

        private string _Barcode = "Barcode ";

        /// <summary>
        /// Sets and gets the Barcode property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Barcode
        {
            get
            {
                return _Barcode;
            }

            set
            {
                if (_Barcode == value)
                {
                    return;
                }

                _Barcode = value;
                RaisePropertyChanged(BarcodePropertyName);
            }
        }

        #endregion

        #region Ownername

        /// <summary>
        /// The <see cref="Ownername" /> property's name.
        /// </summary>
        public const string OwnernamePropertyName = "Ownername";

        private string _Ownername = default(string);

        /// <summary>
        /// Sets and gets the Ownername property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Ownername
        {
            get
            {
                return _Ownername;
            }

            set
            {
                if (_Ownername == value)
                {
                    return;
                }

                _Ownername = value;
                RaisePropertyChanged(OwnernamePropertyName);
            }
        }
        #endregion

        #region Size

        /// <summary>
        /// The <see cref="Size" /> property's name.
        /// </summary>
        public const string SizePropertyName = "Size";

        private string _Size = default(string);

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

        #region Contents

        /// <summary>
        /// The <see cref="Contents" /> property's name.
        /// </summary>
        public const string ContentsPropertyName = "Contents";

        private string _Contents = default(string);

        /// <summary>
        /// Sets and gets the Contents property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Contents
        {
            get
            {
                return _Contents;
            }

            set
            {
                if (_Contents == value)
                {
                    return;
                }

                _Contents = value;
                RaisePropertyChanged(ContentsPropertyName);
            }
        }

        #endregion

        #region Batch

        /// <summary>
        /// The <see cref="Batch" /> property's name.
        /// </summary>
        public const string BatchPropertyName = "Batch";

        private string _Batch = default(string);

        /// <summary>
        /// Sets and gets the Batch property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Batch
        {
            get
            {
                return _Batch;
            }

            set
            {
                if (_Batch == value)
                {
                    return;
                }

                _Batch = value;
                RaisePropertyChanged(BatchPropertyName);
            }
        }

        #endregion

        #region Location

        /// <summary>
        /// The <see cref="Location" /> property's name.
        /// </summary>
        public const string LocationPropertyName = "Location";

        private string _Location = default(string);

        /// <summary>
        /// Sets and gets the Location property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Location
        {
            get
            {
                return _Location;
            }

            set
            {
                if (_Location == value)
                {
                    return;
                }

                _Location = value;
                RaisePropertyChanged(LocationPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public RelayCommand DoneCommand { get; }

        #endregion

        #region Constructor

        public ScanInfoViewModel()
        {
            DoneCommand = new RelayCommand(DoneCommandRecieverAsync);
        }

        #endregion

        #region Methods
        private async void DoneCommandRecieverAsync()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
            catch (System.Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal void AssignInitialValue(ValidateBarcodeModel _barcode)
        {
            //var value = await SQLiteServiceClient.Db.Table<ValidatePartnerModel>().Where(x => x.Barcode == _barcode).ToListAsync();

            try
            {
                Barcode = string.Format(" Barcode {0} ", _barcode.Barcode);
                //Ownername = _barcode.Ownername;
                //Size = _barcode.Tags[2].Value;
                //Contents = _barcode.Contents;
                //Batch = _barcode.Batch;
                //Location = _barcode.Location;
            }
            catch (System.Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        #endregion
    }
}
