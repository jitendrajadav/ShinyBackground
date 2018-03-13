using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using KegID.Views;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using KegID.SQLiteClient;
using System.Diagnostics;
using System;

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
        public RelayCommand<BatchModel> ItemTappedCommand { get;}
        public RelayCommand AddBatchCommand { get; }
        #endregion

        #region Constructor

        public BatchViewModel(IFillService fillService)
        {
            _fillService = fillService;
            ItemTappedCommand = new RelayCommand<BatchModel>((model) => ItemTappedCommandRecieverAsync(model));
            AddBatchCommand = new RelayCommand(AddBatchCommandRecieverAsync);
            LoadBatch();
        }

        #endregion

        #region Methods

        private async void LoadBatch()
        {
            await LoadBatchAsync();
        }

        private async void AddBatchCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new AddBatchView());
        }

        private async void ItemTappedCommandRecieverAsync(BatchModel model)
        {
            if (model != null)
            {
                SimpleIoc.Default.GetInstance<FillViewModel>().NewBatchModel = model;
                SimpleIoc.Default.GetInstance<FillViewModel>().BatchButtonTitle = model.BrandName + "-" + model.BatchCode;
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }

        public async Task<IList<BatchModel>> LoadBatchAsync()
        {
            try
            {
                BatchCollection = await SQLiteServiceClient.Db.Table<BatchModel>().ToListAsync();
                if (BatchCollection.Count==0)
                {
                    Loader.StartLoading();

                    var value = await _fillService.GetBatchListAsync(Configuration.SessionId);
                    if (value.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        BatchCollection = value.BatchModel.Where(p=>p.BrandName!= string.Empty).OrderBy(x => x.BrandName).ToList();
                        await SQLiteServiceClient.Db.InsertAllAsync(BatchCollection);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
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
