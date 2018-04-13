using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class VolumeViewModel : BaseViewModel
    {
        #region Properties

        #region VolumeCollection

        /// <summary>
        /// The <see cref="VolumeCollection" /> property's name.
        /// </summary>
        public const string VolumeCollectionPropertyName = "VolumeCollection";

        private IList<string> _VolumeCollection = null;

        /// <summary>
        /// Sets and gets the VolumeCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<string> VolumeCollection
        {
            get
            {
                return _VolumeCollection;
            }

            set
            {
                if (_VolumeCollection == value)
                {
                    return;
                }

                _VolumeCollection = value;
                RaisePropertyChanged(VolumeCollectionPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public RelayCommand<string> ItemTappedCommand { get; }
        
        #endregion

        #region Constructor

        public VolumeViewModel()
        {
            ItemTappedCommand = new RelayCommand<string>((model)=>ItemTappedCommandRecieverAsync(model));
            VolumeCollection = new List<string>() { "bbl", "hl", "gal" };
        }

        #endregion

        #region Methods

        private async void ItemTappedCommandRecieverAsync(string model)
        {
            SimpleIoc.Default.GetInstance<AddBatchViewModel>().VolumeChar = model;
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        #endregion
    }
}
