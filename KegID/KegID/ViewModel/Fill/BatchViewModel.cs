using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using KegID.View;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;

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
                if (model.BrandName.Contains("Add Batch"))
                {
                    await Application.Current.MainPage.Navigation.PushModalAsync(new AddBatchView());
                }
                else
                {
                    SimpleIoc.Default.GetInstance<FillViewModel>().BatchButtonTitle = model.BrandName + "-" + model.BatchCode;
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                }
            }
        }

        public async Task<IList<BatchModel>> LoadBatchAsync()
        {
            try
            {
                Loader.StartLoading();
                var value = await _fillService.GetBatchListAsync(Configuration.SessionId);
                if (value.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    value.BatchModel.Add(new BatchModel { BrandName = "Add Batch" });
                    BatchCollection = value.BatchModel.OrderBy(x=>x.BrandName).ToList();
                }
            }
            catch (System.Exception)
            {

            }
            finally
            {
                Loader.StopLoading();
            }
            return BatchCollection;
        }
        #endregion
    }
}
