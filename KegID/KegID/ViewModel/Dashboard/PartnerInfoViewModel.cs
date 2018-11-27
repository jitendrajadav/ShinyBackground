using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KegID.Common;
using KegID.DependencyServices;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class PartnerInfoViewModel : BaseViewModel
    {
        #region Properties

        private readonly IDashboardService _dashboardService;
        private readonly INavigationService _navigationService;

        string translatedNumber;

        #region KegsHeld

        /// <summary>
        /// The <see cref="KegsHeld" /> property's name.
        /// </summary>
        public const string KegsHeldPropertyName = "KegsHeld";

        private string _KegsHeld = string.Empty;

        /// <summary>
        /// Sets and gets the KegsHeld property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string KegsHeld
        {
            get
            {
                return _KegsHeld;
            }

            set
            {
                if (_KegsHeld == value)
                {
                    return;
                }

                _KegsHeld = value;
                RaisePropertyChanged(KegsHeldPropertyName);
            }
        }

        #endregion

        #region Contact

        /// <summary>
        /// The <see cref="Contact" /> property's name.
        /// </summary>
        public const string ContactPropertyName = "Contact";

        private string _Contact = string.Empty;

        /// <summary>
        /// Sets and gets the Contact property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Contact
        {
            get
            {
                return _Contact;
            }

            set
            {
                if (_Contact == value)
                {
                    return;
                }

                _Contact = value;
                RaisePropertyChanged(ContactPropertyName);
            }
        }

        #endregion

        #region ContactEmail

        /// <summary>
        /// The <see cref="ContactEmail" /> property's name.
        /// </summary>
        public const string ContactEmailPropertyName = "ContactEmail";

        private string _ContactEmail = string.Empty;

        /// <summary>
        /// Sets and gets the ContactEmail property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ContactEmail
        {
            get
            {
                return _ContactEmail;
            }

            set
            {
                if (_ContactEmail == value)
                {
                    return;
                }

                _ContactEmail = value;
                RaisePropertyChanged(ContactEmailPropertyName);
            }
        }

        #endregion

        #region Ref

        /// <summary>
        /// The <see cref="Ref" /> property's name.
        /// </summary>
        public const string RefPropertyName = "Ref";

        private string _Ref = string.Empty;

        /// <summary>
        /// Sets and gets the Ref property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Ref
        {
            get
            {
                return _Ref;
            }

            set
            {
                if (_Ref == value)
                {
                    return;
                }

                _Ref = value;
                RaisePropertyChanged(RefPropertyName);
            }
        }

        #endregion

        #region Acct

        /// <summary>
        /// The <see cref="Acct" /> property's name.
        /// </summary>
        public const string AcctPropertyName = "Acct";

        private string _Acct = string.Empty;

        /// <summary>
        /// Sets and gets the Acct property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Acct
        {
            get
            {
                return _Acct;
            }

            set
            {
                if (_Acct == value)
                {
                    return;
                }

                _Acct = value;
                RaisePropertyChanged(AcctPropertyName);
            }
        }

        #endregion

        #region Route

        /// <summary>
        /// The <see cref="Route" /> property's name.
        /// </summary>
        public const string RoutePropertyName = "Route";

        private string _Route = string.Empty;

        /// <summary>
        /// Sets and gets the Route property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Route
        {
            get
            {
                return _Route;
            }

            set
            {
                if (_Route == value)
                {
                    return;
                }

                _Route = value;
                RaisePropertyChanged(RoutePropertyName);
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

        #region PartnerModel

        /// <summary>
        /// The <see cref="PartnerModel" /> property's name.
        /// </summary>
        public const string PartnerModelPropertyName = "PartnerModel";

        private PossessorLocation _PartnerModel = null;

        /// <summary>
        /// Sets and gets the PartnerModel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public PossessorLocation PartnerModel
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

        #region PartnerInfoModel

        /// <summary>
        /// The <see cref="PartnerInfoModel" /> property's name.
        /// </summary>
        public const string PartnerInfoModelPropertyName = "PartnerInfoModel";

        private PartnerInfoResponseModel _PartnerInfoResponseModel = null;

        /// <summary>
        /// Sets and gets the PartnerInfoModel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public PartnerInfoResponseModel PartnerInfoModel
        {
            get
            {
                return _PartnerInfoResponseModel;
            }

            set
            {
                if (_PartnerInfoResponseModel == value)
                {
                    return;
                }

                _PartnerInfoResponseModel = value;
                RaisePropertyChanged(PartnerInfoModelPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand PartnersCommand { get; }
        public DelegateCommand EditCommand { get; }
        public DelegateCommand KegsCommand { get; }
        public DelegateCommand ShipToCommand { get; }
        public DelegateCommand PhoneNumberCommand { get; }
        public DelegateCommand ContactEmailCommand { get;}
        public DelegateCommand SendKegsCommand { get;}
        
        #endregion

        #region Constructor

        public PartnerInfoViewModel(IDashboardService dashboardService, INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            _dashboardService = dashboardService;
            PartnersCommand = new DelegateCommand(PartnersCommandRecieverAsync);
            EditCommand = new DelegateCommand(EditCommandRecieverAsync);
            KegsCommand = new DelegateCommand(KegsCommandRecieverAsync);
            ShipToCommand = new DelegateCommand(ShipToCommandRecieverAsync);
            PhoneNumberCommand = new DelegateCommand(PhoneNumberCommandReciever);
            ContactEmailCommand = new DelegateCommand(ContactEmailCommandRecieverAsync);
            SendKegsCommand = new DelegateCommand(SendKegsCommandRecieverAsync);

            LoadPartnerInfoAsync();
        }


        #endregion

        #region Methods

        private async void ContactEmailCommandRecieverAsync()
        {
            try
            {
                await SendEmail("KegID","Mail Sending",new List<string> { "test@kegid.com" });
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public async Task SendEmail(string subject, string body, List<string> recipients)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients,
                    //Cc = ccRecipients,
                    //Bcc = bccRecipients
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Sms is not supported on this device
                Crashes.TrackError(fbsEx);
            }
            catch (Exception ex)
            {
                // Some other exception occured
                Crashes.TrackError(ex);
            }
        }

        private async void LoadPartnerInfoAsync()
        {
            try
            {
                Loader.StartLoading();

                PartnerInfoModel = await _dashboardService.GetPartnerInfoAsync(AppSettings.SessionId, ConstantManager.DBPartnerId);
                KegsHeld = PartnerInfoModel.KegsHeld.ToString();
                Notes = PartnerInfoModel.Notes;
                Ref = PartnerInfoModel.ReferenceKey;
                ContactEmail = PartnerInfoModel.ContactEmail;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                Loader.StopLoading();
            }
        }

        private async void SendKegsCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync("MoveView", new NavigationParameters
                    {
                        { "PartnerModel", PartnerModel }
                    }, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void ShipToCommandRecieverAsync()
        {
            try
            {
                ConstantManager.Position = new LocationInfo
                {
                    Lat = PartnerInfoModel.Lat,
                    Lon = PartnerInfoModel.Lon,
                    Label = PartnerInfoModel.BillAddress.City,
                    Address = PartnerInfoModel.BillAddress.Line1
                };
                await _navigationService.NavigateAsync("PartnerInfoMapView", new NavigationParameters
                    {
                        { "PartnerInfoModel", PartnerInfoModel }
                    }, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void KegsCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync("KegsView", animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void PhoneNumberCommandReciever()
        {
            OnTranslate(null,null);
        }

        void OnTranslate(object sender, EventArgs e)
        {
            try
            {
                translatedNumber = PhoneTranslator.ToNumber(PartnerModel.PhoneNumber);
                if (!string.IsNullOrWhiteSpace(translatedNumber))
                {
                    //callButton.IsEnabled = true;
                    //callButton.Text = "Call " + translatedNumber;
                    //OnCall(null, null);
                    PlacePhoneCallAsync();
                }
                else
                {
                    //callButton.IsEnabled = false;
                    //callButton.Text = "Call";
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void PlacePhoneCallAsync()
        {
            try
            {
                if (!await Application.Current.MainPage.DisplayAlert(
                           "Dial a Number",
                           "Would you like to call " + translatedNumber + "?",
                           "Cancel",
                           "Call"))
                {
                    // TODO: dial the phone
                    var dialer = DependencyService.Get<IDialer>();
                    if (dialer != null)
                        await dialer.DialAsync(translatedNumber);

                    // Not working with -
                    //PhoneDialer.Open(translatedNumber);
                }
            }
            catch (ArgumentNullException anEx)
            {
                Crashes.TrackError(anEx);
                // Number was null or white space
            }
            catch (FeatureNotSupportedException ex)
            {
                Crashes.TrackError(ex);
                // Phone Dialer is not supported on this device.
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                // Other error has occurred.
            }
        }

        async void OnCall(object sender, EventArgs e)
        {
            try
            {
                if (!await Application.Current.MainPage.DisplayAlert(
                        "Dial a Number",
                        "Would you like to call " + translatedNumber + "?",
                        "Cancel",
                        "Call"))
                {
                    //// TODO: dial the phone
                    //var dialer = DependencyService.Get<IDialer>();
                    //if (dialer != null)
                    //    await dialer.DialAsync(translatedNumber);

                    // Make Phone Call
                    //var phoneCallTask = CrossMessaging.Current.PhoneDialer;
                    //if (phoneCallTask.CanMakePhoneCall)
                    //    phoneCallTask.MakePhoneCall(translatedNumber);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void EditCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync("AddPartnerView", new NavigationParameters
                    {
                        { "PartnerInfoModel", PartnerInfoModel }
                    }, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void PartnersCommandRecieverAsync()
        {
            try
            {
                await _navigationService.GoBackAsync(animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("PartnerModel"))
            {
                PartnerModel = parameters.GetValue<PossessorResponseModel>("PartnerModel").Location;
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("PartnersCommandRecieverAsync"))
            {
                PartnersCommandRecieverAsync();
            }
        }

        #endregion
    }
}
