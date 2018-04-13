using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Services;
using KegID.SQLiteClient;
using KegID.Views;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;
using KegID.Model;
using System.Diagnostics;
using System;

namespace KegID.ViewModel
{
    public class PartnersViewModel : BaseViewModel
    {
        #region Properties
        public bool BrewerStockOn { get; set; }
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

        public RelayCommand InternalCommand { get; }
        public RelayCommand AlphabeticalCommand { get; }
        public RelayCommand<PartnerModel> ItemTappedCommand { get; }
        public RelayCommand SearchPartnerCommand { get; }
        public RelayCommand AddNewPartnerCommand { get; }
        public RelayCommand BackCommand { get; }
        public RelayCommand TextChangedCommand { get; }
        
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
        }
        
        #endregion

        #region Methods

        private void TextChangedCommandRecieverAsync()
        {
            try
            {
                var result = AllPartners.Where(x => x.FullName.ToLower().Contains(PartnerName.ToLower()));
                PartnerCollection = new ObservableCollection<PartnerModel>(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async void ItemTappedCommandRecieverAsync(PartnerModel model)
        {
            if (model != null)
            {
                switch ((ViewTypeEnum)Enum.Parse(typeof(ViewTypeEnum), Application.Current.MainPage.Navigation.ModalStack[Application.Current.MainPage.Navigation.ModalStack.Count - 2].GetType().Name))
                {
                    case ViewTypeEnum.SearchManifestsView:
                        SimpleIoc.Default.GetInstance<SearchManifestsViewModel>().AssignPartnerValue(model);
                        break;

                    case ViewTypeEnum.MoveView:
                        SimpleIoc.Default.GetInstance<MoveViewModel>().PartnerModel = model;
                        break;

                    case ViewTypeEnum.FillView:
                        SimpleIoc.Default.GetInstance<FillViewModel>().PartnerModel = model;
                        break;

                    case ViewTypeEnum.PalletizeView:
                        SimpleIoc.Default.GetInstance<PalletizeViewModel>().AssignPartnerValue(model);
                        break;

                    case ViewTypeEnum.MaintainView:
                        SimpleIoc.Default.GetInstance<MaintainViewModel>().PartnerModel = model;
                        break;

                    case ViewTypeEnum.EditKegView:
                        SimpleIoc.Default.GetInstance<EditKegViewModel>().PartnerModel = model;
                        break;

                    case ViewTypeEnum.SearchPalletView:
                        SimpleIoc.Default.GetInstance<SearchPalletViewModel>().PartnerModel = model;
                        break;

                    default:
                        break;
                }
                await Application.Current.MainPage.Navigation.PopModalAsync();
                Cleanup();
            }
        }

        public async void LoadPartnersAsync()
        {
            Loader.StartLoading();

            AllPartners = await SQLiteServiceClient.Db.Table<PartnerModel>().ToListAsync();

            try
            {
                if (AllPartners.Count > 0)
                {
                    if (BrewerStockOn)
                        PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners.Where(x => x.PartnerTypeName == "Brewer - Stock").ToList());
                    else
                        PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners);
                }
                else
                {
                    var value = await _moveService.GetPartnersListAsync(AppSettings.User.SessionId);
                    if (value.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        try
                        {
                            AllPartners = value.PartnerModel.Where(x => x.FullName != string.Empty).ToList();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                        if (BrewerStockOn)
                            PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners.Where(x => x.PartnerTypeName == "Brewer - Stock").ToList());
                        else
                            PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners);
                        await SQLiteServiceClient.Db.InsertAllAsync(AllPartners);
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
        }

        private void AlphabeticalCommandReciever()
        {
            if (BrewerStockOn)
                PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners.OrderBy(x => x.FullName).Where(x => x.PartnerTypeName == "Brewer - Stock").ToList());
            else
                PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners.OrderBy(x => x.FullName));

            AlphabeticalBackgroundColor = "#4E6388";
            AlphabeticalTextColor = "White";

            InternalBackgroundColor = "White";
            InternalTextColor = "#4E6388";
        }

        private void InternalCommandReciever()
        {
            if (BrewerStockOn)
                PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners.Where(x => x.PartnerTypeName == "Brewer - Stock").ToList());
            else
                PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners);

            InternalBackgroundColor = "#4E6388";
            InternalTextColor = "White";

            AlphabeticalBackgroundColor = "White";
            AlphabeticalTextColor = "#4E6388";
        }

        private async void BackCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
            Cleanup();
        }

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
            BrewerStockOn = false;
            PartnerCollection = null;
        }

        #endregion
    }
}
