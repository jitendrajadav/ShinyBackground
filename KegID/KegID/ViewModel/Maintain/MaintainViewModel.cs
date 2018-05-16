using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Model;
using KegID.Views;
using Microsoft.AppCenter.Crashes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class MaintainViewModel : BaseViewModel
    {
        #region Properties

        #region PartnerModel

        /// <summary>
        /// The <see cref="PartnerModel" /> property's name.
        /// </summary>
        public const string PartnerModelPropertyName = "PartnerModel";

        private PartnerModel _PartnerModel = new PartnerModel();

        /// <summary>
        /// Sets and gets the PartnerModel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public PartnerModel PartnerModel
        {
            get
            {
                return _PartnerModel;
            }

            set
            {
                if (_PartnerModel == value)
                {
                    return;
                }

                _PartnerModel = value;
                RaisePropertyChanged(PartnerModelPropertyName);
            }
        }

        #endregion

        #region Notes

        /// <summary>
        /// The <see cref="Notes" /> property's name.
        /// </summary>
        public const string NotesPropertyName = "Notes";

        private string _Notes = string.Empty;

        /// <summary>
        /// Sets and gets the Notes property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Notes
        {
            get
            {
                return _Notes;
            }

            set
            {
                if (_Notes == value)
                {
                    return;
                }

                _Notes = value;
                RaisePropertyChanged(NotesPropertyName);
            }
        }

        #endregion

        #region MaintainTypeCollection

        /// <summary>
        /// The <see cref="MaintainTypeCollection" /> property's name.
        /// </summary>
        public const string MaintainTypeCollectionPropertyName = "MaintainTypeCollection";

        private IList<MaintainTypeReponseModel> _maintainTypeCollection = null;

        /// <summary>
        /// Sets and gets the MaintainTypeCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<MaintainTypeReponseModel> MaintainTypeCollection
        {
            get
            {
                return _maintainTypeCollection;
            }

            set
            {
                if (_maintainTypeCollection == value)
                {
                    return;
                }

                _maintainTypeCollection = value;
                RaisePropertyChanged(MaintainTypeCollectionPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public RelayCommand HomeCommand { get; }
        public RelayCommand NextCommand { get; }
        public RelayCommand PartnerCommand { get; }
        public RelayCommand<MaintainTypeReponseModel> ItemTappedCommand { get; }

        #endregion

        #region Constructor

        public MaintainViewModel()
        {
            HomeCommand = new RelayCommand(HomeCommandRecieverAsync);
            PartnerCommand = new RelayCommand(PartnerCommandRecieverAsync);
            NextCommand = new RelayCommand(NextCommandRecieverAsync);
            PartnerModel.FullName = "Select a location";
            ItemTappedCommand = new RelayCommand<MaintainTypeReponseModel>((model) => ItemTappedCommandReciever(model));
        }

        #endregion

        #region Methods

        private void ItemTappedCommandReciever(MaintainTypeReponseModel model)
        {
            model.IsToggled = !model.IsToggled;
        }

        public async Task LoadMaintenanceTypeAsync()
        {
            try
            {
                var result = await SimpleIoc.Default.GetInstance<MaintainScanViewModel>().LoadMaintenanceTypeAsync();
                MaintainTypeCollection = result.Where(x => x.ActivationMethod == "ReverseOnly").OrderBy(x => x.Name).ToList();
            }
            catch (System.Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void HomeCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PopModalAsync();

        private async void PartnerCommandRecieverAsync()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushModalAsync(new PartnersView(), animated: false);
            }
            catch (System.Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
        private async void NextCommandRecieverAsync()
        {
            try
            {
                var flag = MaintainTypeCollection.Where(x => x.IsToggled == true);
                if (flag != null)
                    await Application.Current.MainPage.Navigation.PushModalAsync(new MaintainScanView(), animated: false);
                else
                    await Application.Current.MainPage.DisplayAlert("Error", "Please select at least one maintenance item to perform.", "Ok");

            }
            catch (System.Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        #endregion
    }
}
