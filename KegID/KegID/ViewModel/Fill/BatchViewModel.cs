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
using System;
using Microsoft.AppCenter.Crashes;
using Realms;
using KegID.LocalDb;

namespace KegID.ViewModel
{
    public class BatchViewModel : BaseViewModel
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
            try
            {
                await Application.Current.MainPage.Navigation.PushModalAsync(new AddBatchView(), animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void ItemTappedCommandRecieverAsync(BatchModel model)
        {
            try
            {
                if (model != null)
                {
                    SimpleIoc.Default.GetInstance<FillViewModel>().NewBatchModel = model;
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public async Task<IList<BatchModel>> LoadBatchAsync()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                BatchCollection = RealmDb.All<BatchModel>().ToList();//await SQLiteServiceClient.Db.Table<BatchModel>().ToListAsync();
                if (BatchCollection.Count==0)
                {
                    Loader.StartLoading();

                    var value = await _fillService.GetBatchListAsync(AppSettings.User.SessionId);
                    if (value.Response.StatusCode == System.Net.HttpStatusCode.OK.ToString())
                    {
                        BatchCollection = value.BatchModel.Where(p=>p.BrandName!= string.Empty).OrderBy(x => x.BrandName).ToList();
                        RealmDb.Write(() => 
                        {
                            foreach (var item in BatchCollection)
                            {
                                RealmDb.Add(item);
                            }
                        });
                        //await SQLiteServiceClient.Db.InsertAllAsync(BatchCollection);
                    }
                }
            }
            catch (Exception ex)
            {
                 Crashes.TrackError(ex);
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
