using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Model;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class BrandViewModel : BaseViewModel
    {
        #region Properties

        #region BrandCollection

        /// <summary>
        /// The <see cref="BrandCollection" /> property's name.
        /// </summary>
        public const string BrandCollectionPropertyName = "BrandCollection";

        private IList<BrandModel> _BrandCollection = null;

        /// <summary>
        /// Sets and gets the BrandCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<BrandModel> BrandCollection
        {
            get
            {
                return _BrandCollection;
            }

            set
            {
                if (_BrandCollection == value)
                {
                    return;
                }

                _BrandCollection = value;
                RaisePropertyChanged(BrandCollectionPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public RelayCommand<BrandModel> ItemTappedCommand { get; }

        #endregion

        #region Constructor
        public BrandViewModel()
        {
            ItemTappedCommand = new RelayCommand<BrandModel>((model)=>ItemTappedCommandRecieverAsync(model));
            LoadBrand();
        }

        private async void LoadBrand()
        {
            BrandCollection = await SimpleIoc.Default.GetInstance<ScanKegsViewModel>().LoadBrandAsync();
        }

        #endregion

        #region Methods
        private async void ItemTappedCommandRecieverAsync(BrandModel model)
        {
            SimpleIoc.Default.GetInstance<AddBatchViewModel>().BrandModel = model;
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        #endregion

    }
}
