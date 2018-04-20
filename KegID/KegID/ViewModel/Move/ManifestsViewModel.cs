using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using KegID.SQLiteClient;
using KegID.Views;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class ManifestsViewModel : BaseViewModel
    {
        #region Properties

        public IMoveService _moveService { get; set; }

        #region ManifestCollection

        /// <summary>
        /// The <see cref="ManifestCollection" /> property's name.
        /// </summary>
        public const string ManifestCollectionPropertyName = "ManifestCollection";

        private ObservableCollection<ManifestModel> _ManifestCollection = new ObservableCollection<ManifestModel>();

        /// <summary>
        /// Sets and gets the ManifestCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<ManifestModel> ManifestCollection
        {
            get
            {
                return _ManifestCollection;
            }

            set
            {
                if (_ManifestCollection == value)
                {
                    return;
                }

                _ManifestCollection = value;
                RaisePropertyChanged(ManifestCollectionPropertyName);
            }
        }

        #endregion

        #region QueuedTextColor

        /// <summary>
        /// The <see cref="QueuedTextColor" /> property's name.
        /// </summary>
        public const string QueuedTextColorPropertyName = "QueuedTextColor";

        private string _QueuedTextColor = "#4E6388";

        /// <summary>
        /// Sets and gets the QueuedTextColor property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string QueuedTextColor
        {
            get
            {
                return _QueuedTextColor;
            }

            set
            {
                if (_QueuedTextColor == value)
                {
                    return;
                }

                _QueuedTextColor = value;
                RaisePropertyChanged(QueuedTextColorPropertyName);
            }
        }

        #endregion

        #region QueuedBackgroundColor

        /// <summary>
        /// The <see cref="QueuedBackgroundColor" /> property's name.
        /// </summary>
        public const string QueuedBackgroundColorPropertyName = "QueuedBackgroundColor";

        private string _QueuedBackgroundColor = "Transparent";

        /// <summary>
        /// Sets and gets the QueuedBackgroundColor property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string QueuedBackgroundColor
        {
            get
            {
                return _QueuedBackgroundColor;
            }

            set
            {
                if (_QueuedBackgroundColor == value)
                {
                    return;
                }

                _QueuedBackgroundColor = value;
                RaisePropertyChanged(QueuedBackgroundColorPropertyName);
            }
        }

        #endregion

        #region DraftTextColor

        /// <summary>
        /// The <see cref="DraftTextColor" /> property's name.
        /// </summary>
        public const string DraftTextColorPropertyName = "DraftTextColor";

        private string _DraftTextColor = "White";

        /// <summary>
        /// Sets and gets the DraftTextColor property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string DraftTextColor
        {
            get
            {
                return _DraftTextColor;
            }

            set
            {
                if (_DraftTextColor == value)
                {
                    return;
                }

                _DraftTextColor = value;
                RaisePropertyChanged(DraftTextColorPropertyName);
            }
        }

        #endregion

        #region DraftBackgroundColor

        /// <summary>
        /// The <see cref="DraftBackgroundColor" /> property's name.
        /// </summary>
        public const string DraftBackgroundColorPropertyName = "DraftBackgroundColor";

        private string _DraftBackgroundColor = "#4E6388";

        /// <summary>
        /// Sets and gets the DraftBackgroundColor property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string DraftBackgroundColor
        {
            get
            {
                return _DraftBackgroundColor;
            }

            set
            {
                if (_DraftBackgroundColor == value)
                {
                    return;
                }

                _DraftBackgroundColor = value;
                RaisePropertyChanged(DraftBackgroundColorPropertyName);
            }
        }

        #endregion

        #region RecentTextColor

        /// <summary>
        /// The <see cref="RecentTextColor" /> property's name.
        /// </summary>
        public const string RecentTextColorPropertyName = "RecentTextColor";

        private string _RecentTextColor = "#4E6388";

        /// <summary>
        /// Sets and gets the RecentTextColor property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string RecentTextColor
        {
            get
            {
                return _RecentTextColor;
            }

            set
            {
                if (_RecentTextColor == value)
                {
                    return;
                }

                _RecentTextColor = value;
                RaisePropertyChanged(RecentTextColorPropertyName);
            }
        }

        #endregion

        #region RecentBackgroundColor

        /// <summary>
        /// The <see cref="RecentBackgroundColor" /> property's name.
        /// </summary>
        public const string RecentBackgroundColorPropertyName = "RecentBackgroundColor";

        private string _RecentBackgroundColor = "Transparent";

        /// <summary>
        /// Sets and gets the RecentBackgroundColor property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string RecentBackgroundColor
        {
            get
            {
                return _RecentBackgroundColor;
            }

            set
            {
                if (_RecentBackgroundColor == value)
                {
                    return;
                }

                _RecentBackgroundColor = value;
                RaisePropertyChanged(RecentBackgroundColorPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public RelayCommand HomeCommand { get; }
        public RelayCommand ActionSearchCommand { get; }
        public RelayCommand QueuedCommand { get; }
        public RelayCommand DraftCommand { get; }
        public RelayCommand RecentCommand { get; }
        public RelayCommand<ManifestModel> ItemTappedCommand { get; }

        #endregion

        #region Constructor

        public ManifestsViewModel(IMoveService moveService)
        {
            _moveService = moveService;

            HomeCommand = new RelayCommand(HomeCommandRecieverAsync);
            ActionSearchCommand = new RelayCommand(ActionSearchCommandRecieverAsync);
            QueuedCommand = new RelayCommand(QueuedCommandReciever);
            DraftCommand = new RelayCommand(DraftCommandReciever);
            RecentCommand = new RelayCommand(RecentCommandReciever);
            ItemTappedCommand = new RelayCommand<ManifestModel>((model) => ItemTappedCommandRecieverAsync(model));
        }

        #endregion

        #region Methods

        internal async Task LoadDraftManifestAsync()
        {
            try
            {
                ManifestCollection.Clear();
                Loader.StartLoading();
                var collection = await SQLiteServiceClient.Db.Table<DraftManifestModel>().ToListAsync();
                foreach (var item in collection)
                {
                    ManifestModel manifest = JsonConvert.DeserializeObject<ManifestModel>(item.DraftManifestJson);
                    manifest.ManifestItemsCount = manifest.ManifestItems.Count;
                    manifest.OwnerName = manifest.OwnerName;
                    ManifestCollection.Add(manifest);
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
            finally
            {
                Loader.StopLoading();
            }
        }

        private async void ItemTappedCommandRecieverAsync(ManifestModel model)
        {
            SimpleIoc.Default.GetInstance<MoveViewModel>().AssignInitialValue(model.ManifestId, model.ManifestItems.Count > 0 ? model.ManifestItems.FirstOrDefault().Barcode:string.Empty, model.ManifestItemsCount > 0 ? model.ManifestItemsCount.ToString() : string.Empty, model.OwnerName, model.ReceiverId, true);
            await Application.Current.MainPage.Navigation.PushModalAsync(new MoveView());
        }

        private async void HomeCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void ActionSearchCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PushModalAsync(new SearchManifestsView());

        private void QueuedCommandReciever()
        {
            QueuedTextColor = "White";
            QueuedBackgroundColor = "#4E6388";
            DraftTextColor = "#4E6388";
            DraftBackgroundColor = "Transparent";
            RecentTextColor= "#4E6388";
            RecentBackgroundColor = "Transparent";
        }

        private void DraftCommandReciever()
        {
            DraftTextColor = "White";
            DraftBackgroundColor = "#4E6388";
            QueuedTextColor = "#4E6388";
            QueuedBackgroundColor = "Transparent";
            RecentTextColor = "#4E6388";
            RecentBackgroundColor = "Transparent";
        }

        private void RecentCommandReciever()
        {
            RecentTextColor = "White";
            RecentBackgroundColor = "#4E6388";
            QueuedTextColor = "#4E6388";
            QueuedBackgroundColor = "Transparent";
            DraftTextColor = "#4E6388";
            DraftBackgroundColor = "Transparent";
        }

        #endregion

    }
}
