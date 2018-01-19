using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Response;
using KegID.Services;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class BatchViewModel : ViewModelBase
    {
        #region Properties
        public IFillService _fillService { get; set; }

        #region BatchCollection

        /// <summary>
        /// The <see cref="BatchCollection" /> property's name.
        /// </summary>
        public const string BatchCollectionPropertyName = "BatchCollection";

        private IList<BatchModel> _BatchCollection = null;

        /// <summary>
        /// Sets and gets the BatchCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<BatchModel> BatchCollection
        {
            get
            {
                return _BatchCollection;
            }

            set
            {
                if (_BatchCollection == value)
                {
                    return;
                }

                _BatchCollection = value;
                RaisePropertyChanged(BatchCollectionPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands
        public RelayCommand<BatchModel> ItemTappedCommand { get; set; }

        #endregion

        #region Constructor

        public BatchViewModel(IFillService fillService)
        {
            _fillService = fillService;
            ItemTappedCommand = new RelayCommand<BatchModel>((model) => ItemTappedCommandRecieverAsync(model));
            LoadBatchAsync();
        }

        #endregion

        #region Methods

        private async void ItemTappedCommandRecieverAsync(BatchModel model)
        {
            if (model != null)
            {
                SimpleIoc.Default.GetInstance<FillViewModel>().BatchButtonTitle = model.BrandName + "-" + model.BatchCode;
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }

        private async void LoadBatchAsync()
        {
            try
            {
                Loader.StartLoading();
                BatchCollection = await _fillService.GetBatchListAsync(Configuration.SessionId);
            }
            catch (System.Exception)
            {

            }
            finally
            {
                Loader.StopLoading();
            }
        }
        #endregion
    }
}
