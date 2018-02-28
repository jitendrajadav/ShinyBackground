using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KegID.Common;
using KegID.DependencyServices;
using KegID.Views;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class PartnerInfoViewModel : ViewModelBase
    {
        #region Properties

        string translatedNumber;

        #region Title

        /// <summary>
        /// The <see cref="Title" /> property's name.
        /// </summary>
        public const string TitlePropertyName = "Title";

        private string _Title = default(string);

        /// <summary>
        /// Sets and gets the Title property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Title
        {
            get
            {
                return _Title;
            }

            set
            {
                if (_Title == value)
                {
                    return;
                }

                _Title = value;
                RaisePropertyChanged(TitlePropertyName);
            }
        }

        #endregion

        #region Kegs

        /// <summary>
        /// The <see cref="Kegs" /> property's name.
        /// </summary>
        public const string KegsPropertyName = "Kegs";

        private string _Kegs = "0";

        /// <summary>
        /// Sets and gets the Kegs property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Kegs
        {
            get
            {
                return _Kegs;
            }

            set
            {
                if (_Kegs == value)
                {
                    return;
                }

                _Kegs = value;
                RaisePropertyChanged(KegsPropertyName);
            }
        }

        #endregion

        #region ShipTo

        /// <summary>
        /// The <see cref="ShipTo" /> property's name.
        /// </summary>
        public const string ShipToPropertyName = "ShipTo";

        private string _ShipTo = "Ship To";

        /// <summary>
        /// Sets and gets the ShipTo property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ShipTo
        {
            get
            {
                return _ShipTo;
            }

            set
            {
                if (_ShipTo == value)
                {
                    return;
                }

                _ShipTo = value;
                RaisePropertyChanged(ShipToPropertyName);
            }
        }

        #endregion

        #region BillTo

        /// <summary>
        /// The <see cref="BillTo" /> property's name.
        /// </summary>
        public const string BillToPropertyName = "BillTo";

        private string _BillTo = "Bill To";

        /// <summary>
        /// Sets and gets the BillTo property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string BillTo
        {
            get
            {
                return _BillTo;
            }

            set
            {
                if (_BillTo == value)
                {
                    return;
                }

                _BillTo = value;
                RaisePropertyChanged(BillToPropertyName);
            }
        }

        #endregion

        #region Contact

        /// <summary>
        /// The <see cref="Contact" /> property's name.
        /// </summary>
        public const string ContactPropertyName = "Contact";

        private string _Contact = "Contact";

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

        #region Phone

        /// <summary>
        /// The <see cref="Phone" /> property's name.
        /// </summary>
        public const string PhonePropertyName = "Phone";

        private string _Phone = "Phone";

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

        #region ContactEmail

        /// <summary>
        /// The <see cref="ContactEmail" /> property's name.
        /// </summary>
        public const string ContactEmailPropertyName = "ContactEmail";

        private string _ContactEmail = "Contact";

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

        private string _Ref = "Ref #";

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

        private string _Acct = "Acct #";

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

        private string _Route = "Route";

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

        private string _Notes = "Notes";

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

        #region Type

        /// <summary>
        /// The <see cref="Type" /> property's name.
        /// </summary>
        public const string TypePropertyName = "Type";

        private string _Type = "Type";

        /// <summary>
        /// Sets and gets the Type property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Type
        {
            get
            {
                return _Type;
            }

            set
            {
                if (_Type == value)
                {
                    return;
                }

                _Type = value;
                RaisePropertyChanged(TypePropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public RelayCommand PartnersCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand KegsCommand { get; }
        public RelayCommand ShipToCommand { get; }
        #endregion

        #region Constructor

        public PartnerInfoViewModel()
        {
            PartnersCommand = new RelayCommand(PartnersCommandRecieverAsync);
            EditCommand = new RelayCommand(EditCommandRecieverAsync);
            KegsCommand = new RelayCommand(KegsCommandReciever);
            ShipToCommand = new RelayCommand(ShipToCommandRecieverAsync);
        }

        private async void ShipToCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new PartnerInfoMapView());
        }

        #endregion

        #region Methods

        private void KegsCommandReciever()
        {
            
        }

        void OnTranslate(object sender, EventArgs e)
        {
            translatedNumber = PhoneTranslator.ToNumber(Phone);
            if (!string.IsNullOrWhiteSpace(translatedNumber))
            {
                //callButton.IsEnabled = true;
                //callButton.Text = "Call " + translatedNumber;
            }
            else
            {
                //callButton.IsEnabled = false;
                //callButton.Text = "Call";
            }
        }
        async void OnCall(object sender, EventArgs e)
        {
            if (await Application.Current.MainPage.DisplayAlert(
                "Dial a Number",
                "Would you like to call " + translatedNumber + "?",
                "Cancel",
                "Call"))
            {
                // TODO: dial the phone
                var dialer = DependencyService.Get<IDialer>();
                if (dialer != null)
                    await dialer.DialAsync(translatedNumber);
            }
        }

        private async void EditCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new AddPartnerView());
        }

        private async void PartnersCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        #endregion
    }
}
