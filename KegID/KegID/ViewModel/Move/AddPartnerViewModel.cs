using System;
using System.Collections.Generic;
using System.Linq;
using KegID.Common;
using KegID.LocalDb;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Realms;

namespace KegID.ViewModel
{
    public class AddPartnerViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private readonly IMoveService _moveService;
        private readonly IUuidManager _uuidManager;

        #region IsInternalOn

        /// <summary>
        /// The <see cref="IsInternalOn" /> property's name.
        /// </summary>
        public const string IsInternalOnPropertyName = "IsInternalOn";

        private bool _IsInternalOn = false;

        /// <summary>
        /// Sets and gets the IsInternalOn property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsInternalOn
        {
            get
            {
                return _IsInternalOn;
            }

            set
            {
                if (_IsInternalOn == value)
                {
                    return;
                }

                _IsInternalOn = value;
                RaisePropertyChanged(IsInternalOnPropertyName);
            }
        }

        #endregion

        #region IsSharedOn

        /// <summary>
        /// The <see cref="IsSharedOn" /> property's name.
        /// </summary>
        public const string IsSharedOnPropertyName = "IsSharedOn";

        private bool _IsSharedOn = false;

        /// <summary>
        /// Sets and gets the IsSharedOn property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsSharedOn
        {
            get
            {
                return _IsSharedOn;
            }

            set
            {
                if (_IsSharedOn == value)
                {
                    return;
                }

                _IsSharedOn = value;
                RaisePropertyChanged(IsSharedOnPropertyName);
            }
        }

        #endregion

        #region ShipAddress

        /// <summary>
        /// The <see cref="ShipAddress" /> property's name.
        /// </summary>
        public const string ShipAddressPropertyName = "ShipAddress";

        private Address _ShipAddress = new Address();

        /// <summary>
        /// Sets and gets the ShipAddress property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Address ShipAddress
        {
            get
            {
                return _ShipAddress;
            }

            set
            {
                if (_ShipAddress == value)
                {
                    return;
                }

                _ShipAddress = value;
                string ship = UpdateAddress(_ShipAddress);
                ShippingAddress = !string.IsNullOrEmpty(ship) ? ship : "Edit address";
                RaisePropertyChanged(ShipAddressPropertyName);
            }
        }


        #endregion

        #region BillAddress

        /// <summary>
        /// The <see cref="BillAddress" /> property's name.
        /// </summary>
        public const string BillAddressPropertyName = "BillAddress";

        private Address _BillAddress = new Address();

        /// <summary>
        /// Sets and gets the BillAddress property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Address BillAddress
        {
            get
            {
                return _BillAddress;
            }

            set
            {
                if (_BillAddress == value)
                {
                    return;
                }

                _BillAddress = value;
                string bill = UpdateAddress(_BillAddress); 
                BillingAddress = !string.IsNullOrEmpty(bill) ? bill : "Edit address";
                RaisePropertyChanged(BillAddressPropertyName);
            }
        }

        #endregion

        #region AccountNumber

        /// <summary>
        /// The <see cref="AccountNumber" /> property's name.
        /// </summary>
        public const string AccountNumberPropertyName = "AccountNumber";

        private string _AccountNumber = string.Empty;

        /// <summary>
        /// Sets and gets the AccountNumber property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string AccountNumber
        {
            get
            {
                return _AccountNumber;
            }

            set
            {
                if (_AccountNumber == value)
                {
                    return;
                }

                _AccountNumber = value;
                RaisePropertyChanged(AccountNumberPropertyName);
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

        #region ContactName

        /// <summary>
        /// The <see cref="ContactName" /> property's name.
        /// </summary>
        public const string ContactNamePropertyName = "ContactName";

        private string _ContactName = string.Empty;

        /// <summary>
        /// Sets and gets the ContactName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ContactName
        {
            get
            {
                return _ContactName;
            }

            set
            {
                if (_ContactName == value)
                {
                    return;
                }

                _ContactName = value;
                RaisePropertyChanged(ContactNamePropertyName);
            }
        }

        #endregion
        
        #region Fax

        /// <summary>
        /// The <see cref="Fax" /> property's name.
        /// </summary>
        public const string FaxPropertyName = "Fax";

        private string _Fax = string.Empty;

        /// <summary>
        /// Sets and gets the Fax property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Fax
        {
            get
            {
                return _Fax;
            }

            set
            {
                if (_Fax == value)
                {
                    return;
                }

                _Fax = value;
                RaisePropertyChanged(FaxPropertyName);
            }
        }

        #endregion

        #region IsNotify

        /// <summary>
        /// The <see cref="IsNotify" /> property's name.
        /// </summary>
        public const string IsNotifyPropertyName = "IsNotify";

        private bool _IsNotify = false;

        /// <summary>
        /// Sets and gets the IsNotify property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsNotify
        {
            get
            {
                return _IsNotify;
            }

            set
            {
                if (_IsNotify == value)
                {
                    return;
                }

                _IsNotify = value;
                RaisePropertyChanged(IsNotifyPropertyName);
            }
        }

        #endregion

        #region LocationCode

        /// <summary>
        /// The <see cref="LocationCode" /> property's name.
        /// </summary>
        public const string LocationCodePropertyName = "LocationCode";

        private string _LocationCode = string.Empty;

        /// <summary>
        /// Sets and gets the LocationCode property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string LocationCode
        {
            get
            {
                return _LocationCode;
            }

            set
            {
                if (_LocationCode == value)
                {
                    return;
                }

                _LocationCode = value;
                RaisePropertyChanged(LocationCodePropertyName);
            }
        }

        #endregion

        #region LocationStatus

        /// <summary>
        /// The <see cref="LocationStatus" /> property's name.
        /// </summary>
        public const string LocationStatusPropertyName = "LocationStatus";

        private string _LocationStatus = string.Empty;

        /// <summary>
        /// Sets and gets the LocationStatus property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string LocationStatus
        {
            get
            {
                return _LocationStatus;
            }

            set
            {
                if (_LocationStatus == value)
                {
                    return;
                }

                _LocationStatus = value;
                RaisePropertyChanged(LocationStatusPropertyName);
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

        #region PartnerName

        /// <summary>
        /// The <see cref="PartnerName" /> property's name.
        /// </summary>
        public const string PartnerNamePropertyName = "PartnerName";

        private string _PartnerName = string.Empty;

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

        #region Phone

        /// <summary>
        /// The <see cref="Phone" /> property's name.
        /// </summary>
        public const string PhonePropertyName = "Phone";

        private string _Phone = string.Empty;

        /// <summary>
        /// Sets and gets the Phone property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Phone
        {
            get
            {
                return _Phone;
            }

            set
            {
                if (_Phone == value)
                {
                    return;
                }

                _Phone = value;
                RaisePropertyChanged(PhonePropertyName);
            }
        }

        #endregion

        #region PrivateKey

        /// <summary>
        /// The <see cref="PrivateKey" /> property's name.
        /// </summary>
        public const string PrivateKeyPropertyName = "PrivateKey";

        private string _PrivateKey = string.Empty;

        /// <summary>
        /// Sets and gets the PrivateKey property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string PrivateKey
        {
            get
            {
                return _PrivateKey;
            }

            set
            {
                if (_PrivateKey == value)
                {
                    return;
                }

                _PrivateKey = value;
                RaisePropertyChanged(PrivateKeyPropertyName);
            }
        }

        #endregion

        #region ReferenceKey

        /// <summary>
        /// The <see cref="ReferenceKey" /> property's name.
        /// </summary>
        public const string ReferenceKeyPropertyName = "ReferenceKey";

        private string _ReferenceKey = string.Empty;

        /// <summary>
        /// Sets and gets the ReferenceKey property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ReferenceKey
        {
            get
            {
                return _ReferenceKey;
            }

            set
            {
                if (_ReferenceKey == value)
                {
                    return;
                }

                _ReferenceKey = value;
                RaisePropertyChanged(ReferenceKeyPropertyName);
            }
        }

        #endregion

        #region RouteName

        /// <summary>
        /// The <see cref="RouteName" /> property's name.
        /// </summary>
        public const string RouteNamePropertyName = "RouteName";

        private string _RouteName = string.Empty;

        /// <summary>
        /// Sets and gets the RouteName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string RouteName
        {
            get
            {
                return _RouteName;
            }

            set
            {
                if (_RouteName == value)
                {
                    return;
                }

                _RouteName = value;
                RaisePropertyChanged(RouteNamePropertyName);
            }
        }

        #endregion

        #region SmsAddress

        /// <summary>
        /// The <see cref="SmsAddress" /> property's name.
        /// </summary>
        public const string SmsAddressPropertyName = "SmsAddress";

        private string _SmsAddress = string.Empty;

        /// <summary>
        /// Sets and gets the SmsAddress property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string SmsAddress
        {
            get
            {
                return _SmsAddress;
            }

            set
            {
                if (_SmsAddress == value)
                {
                    return;
                }

                _SmsAddress = value;
                RaisePropertyChanged(SmsAddressPropertyName);
            }
        }

        #endregion

        #region PTTimeZone

        /// <summary>
        /// The <see cref="PTTimeZone" /> property's name.
        /// </summary>
        public const string PTTimeZonePropertyName = "PTTimeZone";

        private string _PTTimeZone = string.Empty;

        /// <summary>
        /// Sets and gets the PTTimeZone property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string PTTimeZone
        {
            get
            {
                return _PTTimeZone;
            }

            set
            {
                if (_PTTimeZone == value)
                {
                    return;
                }

                _PTTimeZone = value;
                RaisePropertyChanged(PTTimeZonePropertyName);
            }
        }

        #endregion

        #region Website

        /// <summary>
        /// The <see cref="Website" /> property's name.
        /// </summary>
        public const string WebsitePropertyName = "Website";

        private string _Website = string.Empty;

        /// <summary>
        /// Sets and gets the Website property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Website
        {
            get
            {
                return _Website;
            }

            set
            {
                if (_Website == value)
                {
                    return;
                }

                _Website = value;
                RaisePropertyChanged(WebsitePropertyName);
            }
        }

        #endregion

        #region ShippingAddress

        /// <summary>
        /// The <see cref="ShippingAddress" /> property's name.
        /// </summary>
        public const string ShippingAddressPropertyName = "ShippingAddress";

        private string _ShippingAddress = "Edit address";

        /// <summary>
        /// Sets and gets the ShippingAddress property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ShippingAddress
        {
            get
            {
                return _ShippingAddress;
            }

            set
            {
                if (_ShippingAddress == value)
                {
                    return;
                }

                _ShippingAddress = value;
                RaisePropertyChanged(ShippingAddressPropertyName);
            }
        }

        #endregion

        #region BillingAddress

        /// <summary>
        /// The <see cref="BillingAddress" /> property's name.
        /// </summary>
        public const string BillingAddressPropertyName = "BillingAddress";

        private string _BillingAddress = "Edit address";

        /// <summary>
        /// Sets and gets the BillingAddress property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string BillingAddress
        {
            get
            {
                return _BillingAddress;
            }

            set
            {
                if (_BillingAddress == value)
                {
                    return;
                }

                _BillingAddress = value;
                RaisePropertyChanged(BillingAddressPropertyName);
            }
        }

        #endregion

        #region PartnerTypeCollectioin

        /// <summary>
        /// The <see cref="PartnerTypeCollectioin" /> property's name.
        /// </summary>
        public const string PartnerTypeCollectioinPropertyName = "PartnerTypeCollectioin";

        private IList<PartnerTypeModel> _PartnerTypeCollectioin = null;

        /// <summary>
        /// Sets and gets the PartnerTypeCollectioin property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<PartnerTypeModel> PartnerTypeCollectioin
        {
            get
            {
                return _PartnerTypeCollectioin;
            }

            set
            {
                if (_PartnerTypeCollectioin == value)
                {
                    return;
                }

                _PartnerTypeCollectioin = value;
                RaisePropertyChanged(PartnerTypeCollectioinPropertyName);
            }
        }

        #endregion

        #region SelectedPartnerType

        /// <summary>
        /// The <see cref="SelectedPartnerType" /> property's name.
        /// </summary>
        public const string SelectedPartnerTypePropertyName = "SelectedPartnerType";

        private PartnerTypeModel _SelectedPartnerType = null;

        /// <summary>
        /// Sets and gets the SelectedPartnerType property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public PartnerTypeModel SelectedPartnerType
        {
            get
            {
                return _SelectedPartnerType;
            }

            set
            {
                if (_SelectedPartnerType == value)
                {
                    return;
                }

                _SelectedPartnerType = value;
                RaisePropertyChanged(SelectedPartnerTypePropertyName);
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

        public DelegateCommand CalcelCommand { get; }
        public DelegateCommand SubmitCommand { get;}
        public DelegateCommand ShippingAddressCommand { get; }
        public DelegateCommand BillingAddressCommand { get;}

        #endregion

        #region Constructor

        public AddPartnerViewModel(IMoveService moveService, INavigationService navigationService, IPageDialogService dialogService, IUuidManager uuidManager)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
            _dialogService = dialogService;
            _moveService = moveService;
            _uuidManager = uuidManager;

            CalcelCommand = new DelegateCommand(CalcelCommandRecieverAsync);
            SubmitCommand = new DelegateCommand(SubmitCommandRecieverAsync);
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
            try
            {
                await _navigationService.NavigateAsync("EditAddressView", new NavigationParameters
                    {
                        { "AddressTitle", "Billing Address" }
                    }, animated: false);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void ShippingAddressCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync("EditAddressView", new NavigationParameters
                    {
                        { "AddressTitle", "Shipping Address" }
                    }, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void CalcelCommandRecieverAsync()
        {
            try
            {
                bool accept = await _dialogService.DisplayAlertAsync("Cancel?", "Are you sure you want to cancel?", "Stay here", "Leave");
                if (!accept)
                {
                    await _navigationService.GoBackAsync(animated: false);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void SubmitCommandRecieverAsync()
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
                ParentPartnerId = IsInternalOn ? AppSettings.CompanyId : _uuidManager.GetUuId(),
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

            try
            {
                Loader.StartLoading();
                var result = await _moveService.PostNewPartnerAsync(newPartnerRequestModel, AppSettings.SessionId, RequestType: Configuration.NewPartner);

                if (result != null)
                {
                    try
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
                        await _navigationService.GoBackAsync(new NavigationParameters
                        {
                            { "partnerModel", partnerModel }
                        }, animated: false);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }
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

        public void LoadPartnerAsync(PartnerInfoResponseModel partnerInfoModel)
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            PartnerTypeCollectioin = RealmDb.All<PartnerTypeModel>().ToList();

            if (partnerInfoModel != null)
                AssingValueAddPartner(partnerInfoModel);
        }

        private void AssingValueAddPartner(PartnerInfoResponseModel partnerInfoModel)
        {
            try
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
            catch (Exception ex)
            {
                 Crashes.TrackError(ex);
            }
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            switch (parameters.Keys.FirstOrDefault())
            {
                case "EditAddress":
                    if (parameters.GetValue<bool>("IsShipping"))
                        ShipAddress = parameters.GetValue<Address>("EditAddress");
                    else
                        BillAddress = parameters.GetValue<Address>("EditAddress");
                    break;
                case "PartnerInfoModel":
                    AssignValueAddress(parameters);
                    break;
                default:
                    break;
            }

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
            if (parameters.ContainsKey("CalcelCommandRecieverAsync"))
            {
                CalcelCommandRecieverAsync();
            }
        }

        #endregion
    }
}
