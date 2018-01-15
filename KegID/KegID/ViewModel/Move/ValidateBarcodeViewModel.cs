using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Response;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class ValidateBarcodeViewModel : ViewModelBase
    {
        #region Properties

        #region MultipleKegsTitle

        /// <summary>
        /// The <see cref="MultipleKegsTitle" /> property's name.
        /// </summary>
        public const string MultipleKegsTitlePropertyName = "MultipleKegsTitle";

        private string _MultipleKegsTitle = default(string);

        /// <summary>
        /// Sets and gets the MultipleKegsTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string MultipleKegsTitle
        {
            get
            {
                return _MultipleKegsTitle;
            }

            set
            {
                if (_MultipleKegsTitle == value)
                {
                    return;
                }

                _MultipleKegsTitle = value;
                RaisePropertyChanged(MultipleKegsTitlePropertyName);
            }
        }

        #endregion

        #region PartnerCollection

        /// <summary>
        /// The <see cref="PartnerCollection" /> property's name.
        /// </summary>
        public const string PartnerCollectionPropertyName = "PartnerCollection";

        private IList<PartnerTable> _PartnerCollection = null;

        /// <summary>
        /// Sets and gets the PartnerCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<PartnerTable> PartnerCollection
        {
            get
            {
                return _PartnerCollection;
            }

            set
            {
                if (_PartnerCollection == value)
                {
                    return;
                }

                _PartnerCollection = value;
                RaisePropertyChanged(PartnerCollectionPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands
        public RelayCommand CancelCommand { get; set; }

        public RelayCommand<PartnerTable> ItemTappedCommand { get; set; }
        #endregion

        #region Constructor
        public ValidateBarcodeViewModel()
        {
            CancelCommand = new RelayCommand(CancelCommandRecievierAsync);
            ItemTappedCommand = new RelayCommand<PartnerTable>((model) => ItemTappedCommandRecieverAsync(model));
        }

        #endregion

        #region Methods
        private async void CancelCommandRecievierAsync()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
        private async void ItemTappedCommandRecieverAsync(PartnerTable model)
        {
            SimpleIoc.Default.GetInstance<ScanKegsViewModel>().BarcodeCollection.Where(x => x.Id == model.Barcode).FirstOrDefault().Icon = "validationquestion.png";
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        #endregion
    }
}
