using System;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.DependencyServices;
using KegID.Model;
using KegID.Services;
using KegID.Views;
using Plugin.Messaging;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class PartnerInfoViewModel : BaseViewModel
    {
        #region Properties
        public IDashboardService _dashboardService { get; set; }

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

        public RelayCommand PartnersCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand KegsCommand { get; }
        public RelayCommand ShipToCommand { get; }
        public RelayCommand PhoneNumberCommand { get; }
        public RelayCommand ContactEmailCommand { get;}
        public RelayCommand SendKegsCommand { get;}
        
        #endregion

        #region Constructor

        public PartnerInfoViewModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
            PartnersCommand = new RelayCommand(PartnersCommandRecieverAsync);
            EditCommand = new RelayCommand(EditCommandRecieverAsync);
            KegsCommand = new RelayCommand(KegsCommandRecieverAsync);
            ShipToCommand = new RelayCommand(ShipToCommandRecieverAsync);
            PhoneNumberCommand = new RelayCommand(PhoneNumberCommandReciever);
            ContactEmailCommand = new RelayCommand(ContactEmailCommandReciever);
            SendKegsCommand = new RelayCommand(SendKegsCommandRecieverAsync);

            LoadPartnerInfoAsync();
        }


        #endregion

        #region Methods

        private void ContactEmailCommandReciever()
        {

            // Send Sms
            //var smsMessenger = CrossMessaging.Current.SmsMessenger;
            //if (smsMessenger.CanSendSms)
            //    smsMessenger.SendSms("+27213894839493", "Well hello there from Xam.Messaging.Plugin");


            var emailMessenger = CrossMessaging.Current.EmailMessenger;
            if (emailMessenger.CanSendEmail)
            {
                // Send simple e-mail to single receiver without attachments, bcc, cc etc.
                emailMessenger.SendEmail("to.plugins@xamarin.com", "Xamarin Messaging Plugin", "Well hello there from Xam.Messaging.Plugin");

                // Alternatively use EmailBuilder fluent interface to construct more complex e-mail with multiple recipients, bcc, attachments etc. 
                var email = new EmailMessageBuilder()
                  .To("to.plugins@xamarin.com")
                  .Cc("cc.plugins@xamarin.com")
                  .Bcc(new[] { "bcc1.plugins@xamarin.com", "bcc2.plugins@xamarin.com" })
                  .Subject("Xamarin Messaging Plugin")
                  .Body("Well hello there from Xam.Messaging.Plugin")
                  .Build();

                emailMessenger.SendEmail(email);
            }

            // Construct HTML email (iOS and Android only)
            //var email = new EmailMessageBuilder()
            //  .To("to.plugins@xamarin.com")
            //  .Subject("Xamarin Messaging Plugin")
            //  .BodyAsHtml("Well hello there from <a>Xam.Messaging.Plugin</a>")
            //  .Build();
        }

        private async void LoadPartnerInfoAsync()
        {
            PartnerInfoModel = await _dashboardService.GetPartnerInfoAsync(AppSettings.User.SessionId, SimpleIoc.Default.GetInstance<DashboardPartnersViewModel>().PartnerId);
            KegsHeld = PartnerInfoModel.KegsHeld.ToString();
            Notes = PartnerInfoModel.Notes;
            Ref = PartnerInfoModel.ReferenceKey;
            ContactEmail = PartnerInfoModel.ContactEmail;
        }
        private async void SendKegsCommandRecieverAsync()
        {
            SimpleIoc.Default.GetInstance<MoveViewModel>().AssignInitialValue(string.Empty, string.Empty, string.Empty, PartnerModel.FullName, PartnerModel.PartnerId, true);
            await Application.Current.MainPage.Navigation.PushModalAsync(new MoveView(), animated: false);
        }

        private async void ShipToCommandRecieverAsync()
        {
            SimpleIoc.Default.GetInstance<PartnerInfoMapViewModel>().AssignInitialValue(PartnerInfoModel.Lat, PartnerInfoModel.Lon,PartnerInfoModel.BillAddress.City, PartnerInfoModel.BillAddress.Line1);
            await Application.Current.MainPage.Navigation.PushModalAsync(new PartnerInfoMapView(), animated: false);
        }

        private async void KegsCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new KegsView(), animated: false);
        }
        private void PhoneNumberCommandReciever()
        {
            OnTranslate(null,null);
        }

        void OnTranslate(object sender, EventArgs e)
        {
            translatedNumber = PhoneTranslator.ToNumber(PartnerModel.PhoneNumber);
            if (!string.IsNullOrWhiteSpace(translatedNumber))
            {
                //callButton.IsEnabled = true;
                //callButton.Text = "Call " + translatedNumber;
                OnCall(null,null);
            }
            else
            {
                //callButton.IsEnabled = false;
                //callButton.Text = "Call";
            }
        }
        async void OnCall(object sender, EventArgs e)
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
                var phoneCallTask = CrossMessaging.Current.PhoneDialer;
                if (phoneCallTask.CanMakePhoneCall)
                    phoneCallTask.MakePhoneCall(translatedNumber);
            }
        }

        private async void EditCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new AddPartnerView(), animated: false);
            SimpleIoc.Default.GetInstance<AddPartnerViewModel>().LoadPartnerAsync(PartnerInfoModel);
        }

        private async void PartnersCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        #endregion
    }
}
