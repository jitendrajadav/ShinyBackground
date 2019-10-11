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
        string translatedNumber;
        public string KegsHeld { get; set; }
        public string Contact { get; set; }
        public string ContactEmail { get; set; }
        public string Ref { get; set; }
        public string Acct { get; set; }
        public string Route { get; set; }
        public string Notes { get; set; }
        public PossessorLocation PartnerModel { get; set; }
        public PartnerInfoResponseModel PartnerInfoModel { get; set; }

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

        public PartnerInfoViewModel(IDashboardService dashboardService, INavigationService navigationService) : base(navigationService)
        {
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

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("PartnerModel"))
            {
                PartnerModel = parameters.GetValue<PossessorResponseModel>("PartnerModel").Location;
            }
            return base.InitializeAsync(parameters);
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
