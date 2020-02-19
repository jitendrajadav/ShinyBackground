using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using KegID.Common;
using KegID.LocalDb;
using KegID.Model;
using KegID.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Realms;

namespace KegID.ViewModel
{
    public class AddPartnerViewModel : BaseViewModel
    {
        #region Properties

        private readonly IPageDialogService _dialogService;
        private readonly IUuidManager _uuidManager;

        public bool IsInternalOn { get; set; }
        public bool IsSharedOn { get; set; }
        public Address ShipAddress { get; set; } = new Address();
        public void OnShipAddressChanged()
        {
            string ship = UpdateAddress(ShipAddress);
            ShippingAddress = !string.IsNullOrEmpty(ship) ? ship : "Edit address";
        }

        public Address BillAddress { get; set; } = new Address();
        public void OnBillAddressChanged()
        {
            string bill = UpdateAddress(BillAddress);
            BillingAddress = !string.IsNullOrEmpty(bill) ? bill : "Edit address";
        }

        public string AccountNumber { get; set; }
        public string ContactEmail { get; set; }
        public string ContactName { get; set; }
        public string Fax { get; set; }
        public bool IsNotify { get; set; }
        public string LocationCode { get; set; }
        public string LocationStatus { get; set; }
        public string Notes { get; set; }
        public string PartnerName { get; set; }
        public string Phone { get; set; }
        public string PrivateKey { get; set; }
        public string ReferenceKey { get; set; }
        public string RouteName { get; set; }
        public string SmsAddress { get; set; }
        public string PTTimeZone { get; set; }
        public string Website { get; set; }
        public string ShippingAddress { get; set; } = "Edit address";
        public string BillingAddress { get; set; } = "Edit address";
        public IList<PartnerTypeModel> PartnerTypeCollectioin { get; set; }
        public PartnerTypeModel SelectedPartnerType { get; set; }
        public PartnerInfoResponseModel PartnerInfoModel { get; set; }

        #endregion

        #region Commands

        public DelegateCommand CalcelCommand { get; }
        public DelegateCommand SubmitCommand { get; }
        public DelegateCommand ShippingAddressCommand { get; }
        public DelegateCommand BillingAddressCommand { get; }

        #endregion

        #region Constructor

        public AddPartnerViewModel(INavigationService navigationService, IPageDialogService dialogService, IUuidManager uuidManager) : base(navigationService)
        {
            _dialogService = dialogService;
            _uuidManager = uuidManager;

            CalcelCommand = new DelegateCommand(CalcelCommandRecieverAsync);
            SubmitCommand = new DelegateCommand(async () => await RunSafe(SubmitCommandRecieverAsync()));
            ShippingAddressCommand = new DelegateCommand(ShippingAddressCommandRecieverAsync);
            BillingAddressCommand = new DelegateCommand(BillingAddressCommandRecieverAsync);

            LoadPartnerAsync(null);
        }

        #endregion

        #region Methods

        private string UpdateAddress(Address address)
        {
            string addressLable = string.Empty;
            if (!string.IsNullOrEmpty(address.Line1))
                addressLable = address.Line1 + " ; ";
            if (!string.IsNullOrEmpty(address.Line2))
                addressLable += address.Line2 + " ; ";
            if (!string.IsNullOrEmpty(address.Line3))
                addressLable += address.Line3 + " ; ";
            if (!string.IsNullOrEmpty(address.City))
                addressLable += address.City + " ; ";
            if (!string.IsNullOrEmpty(address.State))
                addressLable += address.State + " ; ";
            if (!string.IsNullOrEmpty(address.PostalCode))
                addressLable += address.PostalCode + " ; ";
            if (!string.IsNullOrEmpty(address.Country))
                addressLable += address.Country + " ; ";
            return addressLable;
        }


        private async void BillingAddressCommandRecieverAsync()
        {
            await NavigationService.NavigateAsync("EditAddressView", new NavigationParameters
                    {
                        { "AddressTitle", "Billing Address" }
                    }, animated: false);
        }

        private async void ShippingAddressCommandRecieverAsync()
        {
            await NavigationService.NavigateAsync("EditAddressView", new NavigationParameters
                    {
                        { "AddressTitle", "Shipping Address" }
                    }, animated: false);
        }

        private async void CalcelCommandRecieverAsync()
        {
            bool accept = await _dialogService.DisplayAlertAsync("Cancel?", "Are you sure you want to cancel?", "Stay here", "Leave");
            if (!accept)
            {
                await NavigationService.GoBackAsync(animated: false);
            }
        }

        private async Task SubmitCommandRecieverAsync()
        {
            NewPartnerRequestModel newPartnerRequestModel = new NewPartnerRequestModel
            {
                AccountNumber = AccountNumber,
                BillAddress = BillAddress,
                ContactEmail = ContactEmail,
                ContactName = ContactName,
                Fax = Fax,
                IsInternal = IsInternalOn,
                IsNotify = IsNotify,
                IsShared = IsSharedOn,
                LocationCode = LocationCode,
                LocationStatus = LocationStatus,
                Notes = Notes,
                FirstName = PartnerName,
                LastName = PartnerName,
                ParentPartnerId = IsInternalOn ? Settings.CompanyId : _uuidManager.GetUuId(),
                PartnerId = _uuidManager.GetUuId(),
                PartnerName = PartnerName,
                PartnerTypeCode = SelectedPartnerType.Code,
                Phone = Phone,
                PrivateKey = PrivateKey,
                ReferenceKey = ReferenceKey,
                RouteName = RouteName,
                ShipAddress = ShipAddress,
                SmsAddress = SmsAddress,
                TimeZone = "+05:30",//PTTimeZone;
                Website = ""//Website;
            };

            UserDialogs.Instance.ShowLoading("Loading");
            var respose = await ApiManager.PostNewPartner(newPartnerRequestModel, Settings.SessionId);
            if (respose.IsSuccessStatusCode)
            {
                PartnerModel partnerModel = new PartnerModel
                {
                    Address = BillingAddress,
                    Address1 = ShippingAddress,
                    City = newPartnerRequestModel.BillAddress != null ? newPartnerRequestModel.BillAddress.City : string.Empty,
                    ParentPartnerId = newPartnerRequestModel.ParentPartnerId,
                    ParentPartnerName = newPartnerRequestModel.PartnerName,
                    PartnerId = newPartnerRequestModel.PartnerId,
                    PartnershipIsActive = newPartnerRequestModel.IsInternal,
                    IsInternal = newPartnerRequestModel.IsInternal,
                    IsShared = newPartnerRequestModel.IsShared,
                    Lat = newPartnerRequestModel.BillAddress != null ? newPartnerRequestModel.BillAddress.Latitude : default(double),
                    LocationCode = newPartnerRequestModel.LocationCode,
                    LocationStatus = newPartnerRequestModel.LocationStatus,
                    Lon = newPartnerRequestModel.BillAddress != null ? newPartnerRequestModel.BillAddress.Longitude : default(double),
                    MasterCompanyId = newPartnerRequestModel.ParentPartnerId,
                    PartnerTypeCode = newPartnerRequestModel.PartnerTypeCode,
                    PartnerTypeName = newPartnerRequestModel.PartnerName,
                    PhoneNumber = newPartnerRequestModel.PartnerName,
                    PostalCode = newPartnerRequestModel.BillAddress != null ? newPartnerRequestModel.BillAddress.PostalCode : string.Empty,
                    SourceKey = newPartnerRequestModel.RouteName,
                    State = newPartnerRequestModel.BillAddress != null ? newPartnerRequestModel.BillAddress.State : string.Empty,
                    FullName = newPartnerRequestModel.PartnerName,
                };
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                await RealmDb.WriteAsync((realmDb) =>
                 {
                     realmDb.Add(partnerModel);
                 });
                await NavigationService.GoBackAsync(new NavigationParameters
                        {
                            { "partnerModel", partnerModel }
                        }, animated: false);

            }
            UserDialogs.Instance.HideLoading();
        }

        public void LoadPartnerAsync(PartnerInfoResponseModel partnerInfoModel)
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            PartnerTypeCollectioin = RealmDb.All<PartnerTypeModel>().ToList();

            if (partnerInfoModel != null)
                AssingValueAddPartner(partnerInfoModel);
        }

        private void AssingValueAddPartner(PartnerInfoResponseModel partnerInfoModel)
        {
            PartnerInfoModel = partnerInfoModel;
            PartnerName = PartnerInfoModel.FullName;
            ShipAddress = PartnerInfoModel.ShipAddress;
            BillAddress = PartnerInfoModel.BillAddress;
            ContactName = PartnerInfoModel.ContactName;
            Phone = PartnerInfoModel.Phone;
            ContactEmail = PartnerInfoModel.ContactEmail;
            AccountNumber = PartnerInfoModel.AccountNumber;
            ReferenceKey = PartnerInfoModel.ReferenceKey;
            Notes = PartnerInfoModel.Notes;
            SelectedPartnerType = PartnerTypeCollectioin.Where(x => x.Name == PartnerInfoModel.PartnerTypeName).FirstOrDefault();
        }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("PartnerInfoModel"))
                AssignValueAddress(parameters);

            return base.InitializeAsync(parameters);
        }

        private void AssignValueAddress(INavigationParameters parameters)
        {
            var value = parameters.GetValue<PartnerInfoResponseModel>("PartnerInfoModel");
            ShipAddress = value.ShipAddress;
            BillAddress = value.BillAddress;
            PartnerName = value.FullName;
            SelectedPartnerType = PartnerTypeCollectioin.Where(x => x.Code == value.PartnerTypeCode).FirstOrDefault();
            IsInternalOn = value.IsInternal;
            IsSharedOn = value.IsShared;
            ContactName = value.ContactName;
            Phone = value.Phone;
            ContactEmail = value.ContactEmail;
            AccountNumber = value.AccountNumber;
            ReferenceKey = value.ReferenceKey;
            Notes = value.Notes;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            switch (parameters.Keys.FirstOrDefault())
            {
                case "EditAddress":
                    if (parameters.GetValue<bool>("IsShipping"))
                        ShipAddress = parameters.GetValue<Address>("EditAddress");
                    else
                        BillAddress = parameters.GetValue<Address>("EditAddress");
                    break;
                case "CalcelCommandRecieverAsync":
                    CalcelCommandRecieverAsync();
                    break;
            }
        }

        #endregion
    }
}
