using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Services;
using KegID.SQLiteClient;
using KegID.View;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;
using KegID.Model;
using System.Diagnostics;
using System;

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

        private ObservableCollection<PartnerModel> _PartnerCollection = null;

        /// <summary>
        /// Sets and gets the PartnerCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<PartnerModel> PartnerCollection
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

        #region PartnerName

        /// <summary>
        /// The <see cref="PartnerName" /> property's name.
        /// </summary>
        public const string PartnerNamePropertyName = "PartnerName";

        private string _PartnerName = default(string);

        /// <summary>
        /// Sets and gets the PartnerName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string PartnerName
        {
            get
            {
                return _PartnerName;
            }

            set
            {
                if (_PartnerName == value)
                {
                    return;
                }

                _PartnerName = value;
                RaisePropertyChanged(PartnerNamePropertyName);
            }
        }

        #endregion

        public IList<PartnerModel> AllPartners { get; set; }

        #endregion

        #region Commands

        public RelayCommand InternalCommand { get; set; }
        public RelayCommand AlphabeticalCommand { get; set; }
        public RelayCommand<PartnerModel> ItemTappedCommand { get; set; }
        public RelayCommand SearchPartnerCommand { get; set; }
        public RelayCommand AddNewPartnerCommand { get; set; }
        public RelayCommand BackCommand { get; set; }
        public RelayCommand TextChangedCommand { get; set; }

        #endregion

        #region Constructor
        public PartnersViewModel(IMoveService moveService)
        {
            _moveService = moveService;

            InternalCommand = new RelayCommand(InternalCommandReciever);
            AlphabeticalCommand = new RelayCommand(AlphabeticalCommandReciever);
            ItemTappedCommand = new RelayCommand<PartnerModel>((model) => ItemTappedCommandRecieverAsync(model));
            SearchPartnerCommand = new RelayCommand(SearchPartnerCommandRecieverAsync);
            AddNewPartnerCommand = new RelayCommand(AddNewPartnerCommandRecieverAsync);
            BackCommand = new RelayCommand(BackCommandRecieverAsync);
            TextChangedCommand = new RelayCommand(TextChangedCommandRecieverAsync);

            InternalBackgroundColor = "#4E6388";
            InternalTextColor = "White";

            #region Old Code
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
            #endregion

            LoadPartnersAsync();
        }

        private void TextChangedCommandRecieverAsync()
        {
            try
            {
                var result = AllPartners.Where(x => x.FullName.ToLower().Contains(PartnerName.ToLower()));
                    PartnerCollection = new ObservableCollection<PartnerModel>(result);
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
         }

        #endregion

        #region Methods

        private async void ItemTappedCommandRecieverAsync(PartnerModel model)
        {
            if (model != null)
            {
                switch ((ViewTypeEnum)Enum.Parse(typeof(ViewTypeEnum), Application.Current.MainPage.Navigation.ModalStack[Application.Current.MainPage.Navigation.ModalStack.Count - 2].GetType().Name))
                {
                    case ViewTypeEnum.SearchManifestsView:
                        if (SimpleIoc.Default.GetInstance<SearchManifestsViewModel>().IsManifestDestination)
                        {
                            SimpleIoc.Default.GetInstance<SearchManifestsViewModel>().IsManifestDestination = false;
                            SimpleIoc.Default.GetInstance<SearchManifestsViewModel>().ManifestDestination = model.FullName;
                        }
                        else
                            SimpleIoc.Default.GetInstance<SearchManifestsViewModel>().ManifestSender = model.FullName;
                        break;
                        case ViewTypeEnum.MoveView:
                        SimpleIoc.Default.GetInstance<MoveViewModel>().Destination = model;
                        break;
                    case ViewTypeEnum.FillView:
                        SimpleIoc.Default.GetInstance<FillViewModel>().DestinationTitle = model.FullName;
                        break;
                    case ViewTypeEnum.PalletizeView:
                        if (SimpleIoc.Default.GetInstance<PalletizeViewModel>().TargetLocationPartner)
                        {
                            SimpleIoc.Default.GetInstance<PalletizeViewModel>().TargetLocationPartner = false;
                            SimpleIoc.Default.GetInstance<PalletizeViewModel>().TargetLocationTitle = model.FullName;
                        }
                        else
                            SimpleIoc.Default.GetInstance<PalletizeViewModel>().SelectLocationTitle = model.FullName;
                        break;
                    default:
                        break;
                }
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }

        private async void LoadPartnersAsync()
        {
            AllPartners = await SQLiteServiceClient.Db.Table<PartnerModel>().ToListAsync();
            try
            {
                if (AllPartners.Count > 0)
                    PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners);
                else
                {
                    Loader.StartLoading();
                    var value = await _moveService.GetPartnersListAsync(Configuration.SessionId);
                    if (value.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        AllPartners = value.PartnerModel.Where(x=>x.FullName != string.Empty).ToList();
                        PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners);
                        await SQLiteServiceClient.Db.InsertAllAsync(AllPartners);
                    }
                }
            }
            catch (System.Exception)
            {
            }
            finally
            {
                Loader.StopLoading();
            }
        } 
        private void AlphabeticalCommandReciever()
        {
            PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners.OrderBy(x => x.FullName));

            AlphabeticalBackgroundColor = "#4E6388";
            AlphabeticalTextColor = "White";

            InternalBackgroundColor = "White";
            InternalTextColor = "#4E6388";
        }

        private void InternalCommandReciever()
        {
            PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners);

            InternalBackgroundColor = "#4E6388";
            InternalTextColor = "White";

            AlphabeticalBackgroundColor = "White";
            AlphabeticalTextColor = "#4E6388";
        }

        private async void BackCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PopModalAsync();

        private async void AddNewPartnerCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new AddPartnerView());
        }

        private async void SearchPartnerCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new SearchPartnersView());
        }

        public override void Cleanup()
        {
            base.Cleanup();
            PartnerCollection = null;
        }

        #endregion
    }
}
