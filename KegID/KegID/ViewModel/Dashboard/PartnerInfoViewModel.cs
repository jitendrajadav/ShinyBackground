using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using KegID.Common;
using KegID.DependencyServices;
using KegID.Model;
using KegID.Services;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class PartnerInfoViewModel : BaseViewModel
    {
        #region Properties

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
        public DelegateCommand ContactEmailCommand { get; }
        public DelegateCommand SendKegsCommand { get; }

        #endregion

        #region Constructor

        public PartnerInfoViewModel(INavigationService navigationService) : base(navigationService)
        {
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
            await SendEmail("KegID", "Mail Sending", new List<string> { "test@kegid.com" });
        }

        public async Task SendEmail(string subject, string body, List<string> recipients)
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

        private async void LoadPartnerInfoAsync()
        {
            UserDialogs.Instance.ShowLoading("Loading");

            var response = await ApiManager.GetPartnerInfo(Settings.SessionId, ConstantManager.DBPartnerId);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = await Task.Run(() => JsonConvert.DeserializeObject<PartnerInfoResponseModel>(json, GetJsonSetting()));
                PartnerInfoModel = data;
                KegsHeld = PartnerInfoModel.KegsHeld.ToString();
                Notes = PartnerInfoModel.Notes;
                Ref = PartnerInfoModel.ReferenceKey;
                ContactEmail = PartnerInfoModel.ContactEmail;
            }

            UserDialogs.Instance.HideLoading();
        }

        private async void SendKegsCommandRecieverAsync()
        {
            await NavigationService.NavigateAsync("MoveView", new NavigationParameters
                    {
                        { "PartnerModel", PartnerModel }
                    }, animated: false);
        }

        private async void ShipToCommandRecieverAsync()
        {
            ConstantManager.Position = new LocationInfo
            {
                Lat = PartnerInfoModel.Lat,
                Lon = PartnerInfoModel.Lon,
                Label = PartnerInfoModel.BillAddress.City,
                Address = PartnerInfoModel.BillAddress.Line1
            };
            await NavigationService.NavigateAsync("PartnerInfoMapView", new NavigationParameters
                    {
                        { "PartnerInfoModel", PartnerInfoModel }
                    }, animated: false);
        }

        private async void KegsCommandRecieverAsync()
        {
            await NavigationService.NavigateAsync("KegsView", animated: false);
        }

        private void PhoneNumberCommandReciever()
        {
            OnTranslate(null, null);
        }

        void OnTranslate(object sender, EventArgs e)
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

        private async void PlacePhoneCallAsync()
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
                //var phoneCallTask = CrossMessaging.Current.PhoneDialer;
                //if (phoneCallTask.CanMakePhoneCall)
                //    phoneCallTask.MakePhoneCall(translatedNumber);
            }
        }

        private async void EditCommandRecieverAsync()
        {
            await NavigationService.NavigateAsync("AddPartnerView", new NavigationParameters
                    {
                        { "PartnerInfoModel", PartnerInfoModel }
                    }, animated: false);
        }

        private async void PartnersCommandRecieverAsync()
        {
            await NavigationService.GoBackAsync(animated: false);
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
