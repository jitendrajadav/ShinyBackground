using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Model;
using KegID.View;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Properties

        #region Title

        /// <summary>
        /// The <see cref="Title" /> property's name.
        /// </summary>
        public const string TitlePropertyName = "Title";

        private string _title = default(string);

        /// <summary>
        /// Sets and gets the Title property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                if (_title == value)
                {
                    return;
                }

                _title = value;
                RaisePropertyChanged(TitlePropertyName);
            }
        }

        #endregion

        #region CemeraIcon

        /// <summary>
        /// The <see cref="CemeraIcon" /> property's name.
        /// </summary>
        public const string CemeraIconPropertyName = "CemeraIcon";

        private string _cemeraIcon = default(string);

        /// <summary>
        /// Sets and gets the CemeraIcon property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string CemeraIcon
        {
            get
            {
                return _cemeraIcon;
            }

            set
            {
                if (_cemeraIcon == value)
                {
                    return;
                }

                _cemeraIcon = value;
                RaisePropertyChanged(CemeraIconPropertyName);
            }
        }

        #endregion

        #region BarcodeCollection

        /// <summary>
        /// The <see cref="BarcodeCollection" /> property's name.
        /// </summary>
        public const string BarcodeCollectionPropertyName = "BarcodeCollection";

        private ObservableCollection<Barcode> _barcodeCollection = new ObservableCollection<Barcode>();

        /// <summary>
        /// Sets and gets the BarcodeCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<Barcode> BarcodeCollection
        {
            get
            {
                return _barcodeCollection;
            }

            set
            {
                if (_barcodeCollection == value)
                {
                    return;
                }

                _barcodeCollection = value;
                RaisePropertyChanged(BarcodeCollectionPropertyName);
            }
        }

        #endregion

        #region LandedPageCollection

        /// <summary>
        /// The <see cref="LandedPageCollection" /> property's name.
        /// </summary>
        public const string LandedPageCollectionPropertyName = "LandedPageCollection";

        private ObservableCollection<Xamarin.Forms.View> _LandedPageCollection = null;

        /// <summary>
        /// Sets and gets the LandedPageCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<Xamarin.Forms.View> LandedPageCollection
        {
            get
            {
                return _LandedPageCollection;
            }

            set
            {
                if (_LandedPageCollection == value)
                {
                    return;
                }

                _LandedPageCollection = value;
                RaisePropertyChanged(LandedPageCollectionPropertyName);
            }
        }

        #endregion

        #region BgImage

        /// <summary>
        /// The <see cref="BgImage" /> property's name.
        /// </summary>
        public const string BgImagePropertyName = "BgImage";

        private string _BgImage = "Assets/kegbg.png";

        /// <summary>
        /// Sets and gets the BgImage property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string BgImage
        {
            get
            {
                return _BgImage;
            }

            set
            {
                if (_BgImage == value)
                {
                    return;
                }

                _BgImage = value;
                RaisePropertyChanged(BgImagePropertyName);
            }
        }

        #endregion


        #endregion

        #region Commands

        public RelayCommand GotoDetailPage { get; set; }

        public RelayCommand<Barcode> ItemSelectedCommand { get; set; }

        public RelayCommand<Barcode> ItemTappedCommand { get; set; }

        public RelayCommand MaintainCommand { get; set; }
        public RelayCommand PalletizeCommand { get; set; }
        public RelayCommand FillCommand { get; set; }
        public RelayCommand MoveCommand { get; set; }

        #endregion

        public MainViewModel()
        {
            //Title = "Parth!!!";
            CemeraIcon = GetIconByPlatform.GetIcon("camera.png");

            GotoDetailPage = new RelayCommand(GotoDetailPageCommandReciever);
            ItemSelectedCommand = new RelayCommand<Barcode>((model) => ItemSelectedCommandReciever(model));
            ItemTappedCommand = new RelayCommand<Barcode>((model) => ItemTappedCommandReciever(model));

            BgImage = GetIconByPlatform.GetIcon("kegbg.png");
            MaintainCommand = new RelayCommand(MaintainCommandRecieverAsync);
            PalletizeCommand = new RelayCommand(PalletizeCommandRecieverAsync);
            FillCommand = new RelayCommand(FillCommandRecieverAsync);
            MoveCommand = new RelayCommand(MoveCommandRecieverAsync);
        }

        #region Methods
        private async void MoveCommandRecieverAsync()
        {
            SimpleIoc.Default.GetInstance<MoveViewModel>().GetUuId();
            await Application.Current.MainPage.Navigation.PushModalAsync(new MoveView());
        }

        private async void FillCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PushModalAsync(new FillView());

        private async void PalletizeCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PushModalAsync(new PalletizeView());

        private async void MaintainCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PushModalAsync(new MaintainView());

        private void ItemTappedCommandReciever(Barcode model)
        {

        }

        private void ItemSelectedCommandReciever(Barcode model)
        {

        }

        private void GotoDetailPageCommandReciever()
        {
            //Application.Current.MainPage.Navigation.PushModalAsync(new DetailView());
        } 
        #endregion

    }

}
