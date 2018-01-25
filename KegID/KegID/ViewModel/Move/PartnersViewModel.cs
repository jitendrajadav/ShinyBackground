using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Response;
using KegID.Services;
using KegID.SQLiteClient;
using Xamarin.Forms;
using Xamarin.Forms.Extended;

namespace KegID.ViewModel
{
    public class PartnersViewModel : ViewModelBase
    {
        #region Properties

        public IMoveService _moveService { get; set; }

        private const int PageSize = 20;

        #region IsWorking

        /// <summary>
        /// The <see cref="IsWorking" /> property's name.
        /// </summary>
        public const string IsWorkingPropertyName = "IsWorking";

        private bool _IsWorking = false;

        /// <summary>
        /// Sets and gets the IsWorking property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsWorking
        {
            get
            {
                return _IsWorking;
            }

            set
            {
                if (_IsWorking == value)
                {
                    return;
                }

                _IsWorking = value;
                RaisePropertyChanged(IsWorkingPropertyName);
            }
        }

        #endregion

        #region InternalBackgroundColor

        /// <summary>
        /// The <see cref="InternalBackgroundColor" /> property's name.
        /// </summary>
        public const string InternalBackgroundColorPropertyName = "InternalBackgroundColor";

        private string _InternalBackgroundColor = "Transparent";

        /// <summary>
        /// Sets and gets the InternalBackgroundColor property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string InternalBackgroundColor
        {
            get
            {
                return _InternalBackgroundColor;
            }

            set
            {
                if (_InternalBackgroundColor == value)
                {
                    return;
                }

                _InternalBackgroundColor = value;
                RaisePropertyChanged(InternalBackgroundColorPropertyName);
            }
        }

        #endregion

        #region InternalTextColor

        /// <summary>
        /// The <see cref="InternalTextColor" /> property's name.
        /// </summary>
        public const string InternalTextColorPropertyName = "InternalTextColor";

        private string _InternalTextColor = "White";

        /// <summary>
        /// Sets and gets the InternalTextColor property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string InternalTextColor
        {
            get
            {
                return _InternalTextColor;
            }

            set
            {
                if (_InternalTextColor == value)
                {
                    return;
                }

                _InternalTextColor = value;
                RaisePropertyChanged(InternalTextColorPropertyName);
            }
        }

        #endregion

        #region AlphabeticalBackgroundColor

        /// <summary>
        /// The <see cref="AlphabeticalBackgroundColor" /> property's name.
        /// </summary>
        public const string AlphabeticalBackgroundColorPropertyName = "AlphabeticalBackgroundColor";

        private string _AlphabeticalBackgroundColor = "Transparent";

        /// <summary>
        /// Sets and gets the AlphabeticalBackgroundColor property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string AlphabeticalBackgroundColor
        {
            get
            {
                return _AlphabeticalBackgroundColor;
            }

            set
            {
                if (_AlphabeticalBackgroundColor == value)
                {
                    return;
                }

                _AlphabeticalBackgroundColor = value;
                RaisePropertyChanged(AlphabeticalBackgroundColorPropertyName);
            }
        }

        #endregion

        #region AlphabeticalTextColor

        /// <summary>
        /// The <see cref="AlphabeticalTextColor" /> property's name.
        /// </summary>
        public const string AlphabeticalTextColorPropertyName = "AlphabeticalTextColor";

        private string _AlphabeticalTextColor = "#4E6388";

        /// <summary>
        /// Sets and gets the AlphabeticalTextColor property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string AlphabeticalTextColor
        {
            get
            {
                return _AlphabeticalTextColor;
            }

            set
            {
                if (_AlphabeticalTextColor == value)
                {
                    return;
                }

                _AlphabeticalTextColor = value;
                RaisePropertyChanged(AlphabeticalTextColorPropertyName);
            }
        }

        #endregion

        #region PartnerCollection

        /// <summary>
        /// The <see cref="PartnerCollection" /> property's name.
        /// </summary>
        public const string PartnerCollectionPropertyName = "PartnerCollection";

        private InfiniteScrollCollection<PartnerModel> _PartnerCollection = null;

        /// <summary>
        /// Sets and gets the PartnerCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public InfiniteScrollCollection<PartnerModel> PartnerCollection
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

        public RelayCommand InternalCommand { get; set; }
        public RelayCommand AlphabeticalCommand { get; set; }

        public RelayCommand<PartnerModel> ItemTappedCommand { get; set; }

        #endregion

        #region Constructor
        public PartnersViewModel(IMoveService moveService)
        {
            _moveService = moveService;

            InternalCommand = new RelayCommand(InternalCommandReciever);
            AlphabeticalCommand = new RelayCommand(AlphabeticalCommandReciever);
            ItemTappedCommand = new RelayCommand<PartnerModel>((model) => ItemTappedCommandRecieverAsync(model));

            InternalBackgroundColor = "#4E6388";
            InternalTextColor = "White";

            //PartnerCollection = new InfiniteScrollCollection<PartnerModel>
            //{
            //    OnLoadMore = async () =>
            //    {
            //        // load the next page
            //        var page = PartnerCollection.Count / PageSize;
            //        //var items = await dataSource.GetItemsAsync(page + 1, PageSize);
            //        var items = await SQLiteServiceClient.Db.Table<PartnerModel>().ToListAsync();
            //        return items;
            //    }
            //};
            LoadPartnersAsync();
        }

        #endregion

        #region Methods

        private async void ItemTappedCommandRecieverAsync(PartnerModel model)
        {
            if (model != null)
            {
                SimpleIoc.Default.GetInstance<MoveViewModel>().Destination = model;

                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }

        private async void LoadPartnersAsync()
        {
            var model = await SQLiteServiceClient.Db.Table<PartnerModel>().ToListAsync();
            try
            {
                if (model.Count > 0)
                    PartnerCollection = new InfiniteScrollCollection<PartnerModel>(model);
                else
                {
                    Loader.StartLoading();
                    PartnerCollection = new InfiniteScrollCollection<PartnerModel>(await _moveService.GetPartnersListAsync(Configuration.SessionId));
                    await SQLiteServiceClient.Db.InsertAllAsync(PartnerCollection);
                }
            }
            catch (System.Exception)
            {
            }
            finally
            {
                Loader.StopLoading();
                model = null;
            }
        } 
        private void AlphabeticalCommandReciever()
        {
            AlphabeticalBackgroundColor = "#4E6388";
            AlphabeticalTextColor = "White";

            InternalBackgroundColor = "White";
            InternalTextColor = "#4E6388";
        }

        private void InternalCommandReciever()
        {
            InternalBackgroundColor = "#4E6388";
            InternalTextColor = "White";

            AlphabeticalBackgroundColor = "White";
            AlphabeticalTextColor = "#4E6388";
        }

        public override void Cleanup()
        {
            base.Cleanup();
            PartnerCollection = null;
        }

        #endregion
    }
}
