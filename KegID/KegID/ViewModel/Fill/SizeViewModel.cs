using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class SizeViewModel : BaseViewModel
    {
        #region Properties

        #region SizeCollection

        /// <summary>
        /// The <see cref="SizeCollection" /> property's name.
        /// </summary>
        public const string SizeCollectionPropertyName = "SizeCollection";

        private IList<string> _SizeCollection = null;

        /// <summary>
        /// Sets and gets the SizeCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<string> SizeCollection
        {
            get
            {
                return _SizeCollection;
            }

            set
            {
                if (_SizeCollection == value)
                {
                    return;
                }

                _SizeCollection = value;
                RaisePropertyChanged(SizeCollectionPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public RelayCommand<string> ItemTappedCommand { get;}

        #endregion

        #region Constructor

        public SizeViewModel()
        {
            SizeCollection = new List<string>() { "1/2 bbl", "1/4 bbl", "1/6 bbl", "30 L", "40 L", "50 L" };
            ItemTappedCommand = new RelayCommand<string>((model) => ItemTappedCommandRecieverAsync(model));
        }

        #endregion

        #region Methods
        private async void ItemTappedCommandRecieverAsync(string model)
        {
            SimpleIoc.Default.GetInstance<FillViewModel>().SizeButtonTitle = model;
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        #endregion
    }
}
