using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KegID.Services;
using KegID.View;
using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class MoveViewModel : ViewModelBase
    {
        #region Properties
        public IMoveService _moveService { get; set; }

        #region MenifestRefId

        /// <summary>
        /// The <see cref="MenifestRefId" /> property's name.
        /// </summary>
        public const string MenifestRefIdPropertyName = "MenifestRefId";

        private string _MenifestRefId = default(string);

        /// <summary>
        /// Sets and gets the MenifestRefId property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string MenifestRefId
        {
            get
            {
                return _MenifestRefId;
            }

            set
            {
                if (_MenifestRefId == value)
                {
                    return;
                }

                _MenifestRefId = value;
                RaisePropertyChanged(MenifestRefIdPropertyName);
            }
        }

        #endregion

        #region DestinationButtonTitle

        /// <summary>
        /// The <see cref="DestinationButtonTitle" /> property's name.
        /// </summary>
        public const string DestinationButtonTitlePropertyName = "DestinationButtonTitle";

        private string _DestinationButtonTitle = "Select a location";

        /// <summary>
        /// Sets and gets the DestinationButtonTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string DestinationButtonTitle
        {
            get
            {
                return _DestinationButtonTitle;
            }

            set
            {
                if (_DestinationButtonTitle == value)
                {
                    return;
                }

                _DestinationButtonTitle = value;
                RaisePropertyChanged(DestinationButtonTitlePropertyName);
            }
        }

        #endregion

        #region Tags

        /// <summary>
        /// The <see cref="Tags" /> property's name.
        /// </summary>
        public const string TagsPropertyName = "Tags";

        private string _tags = "Add info";

        /// <summary>
        /// Sets and gets the Tags property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Tags
        {
            get
            {
                return _tags;
            }

            set
            {
                if (_tags == value)
                {
                    return;
                }

                _tags = value;
                RaisePropertyChanged(TagsPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public RelayCommand ScannedCommand { get; set; }

        public RelayCommand SelectLocationCommand { get; set; }

        public RelayCommand MoreInfoCommand { get; set; }

        public RelayCommand ScanKegsCommad { get; set; }

        public RelayCommand SaveDraftCommand { get; set; }

        #endregion

        #region Constructor
        public MoveViewModel(IMoveService moveService)
        {
            _moveService = moveService;

            ScannedCommand = new RelayCommand(ScannedCommandRecieverAsync);
            SelectLocationCommand = new RelayCommand(SelectLocationCommandRecieverAsync);
            MoreInfoCommand = new RelayCommand(MoreInfoCommandRecieverAsync);
            ScanKegsCommad = new RelayCommand(ScanKegsCommadRecieverAsync);
            SaveDraftCommand = new RelayCommand(SaveDraftCommandRecieverAsync);
        }

        private async void SaveDraftCommandRecieverAsync()
        {
            var manifest = await _moveService.GetManifestListAsync(Configuration.SessionId);
        }

        #endregion

        #region Methods

        private async void ScannedCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new ScannerView());
        }

        public void GetUuId()
        {
            MenifestRefId = "Menifest #: " + Regex.Match(Guid.NewGuid().ToString(), @"(.{8})\s*$").Value.ToUpper();
        }

        private async void SelectLocationCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new PartnersView());
        }

        private async void MoreInfoCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new AddTagsView());
        }
        private async void ScanKegsCommadRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new ScanKegsView());
        }
        
        #endregion
    }
}
