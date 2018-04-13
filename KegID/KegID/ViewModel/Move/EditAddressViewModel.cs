using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Model;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class EditAddressViewModel : BaseViewModel
    {
        #region Properties

        #region AddressTitle

        /// <summary>
        /// The <see cref="AddressTitle" /> property's name.
        /// </summary>
        public const string AddressTitlePropertyName = "AddressTitle";

        private string _AddressTitle = default(string);

        /// <summary>
        /// Sets and gets the AddressTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string AddressTitle
        {
            get
            {
                return _AddressTitle;
            }

            set
            {
                if (_AddressTitle == value)
                {
                    return;
                }

                _AddressTitle = value;
                RaisePropertyChanged(AddressTitlePropertyName);
            }
        }

        #endregion

        #region Line1

        /// <summary>
        /// The <see cref="Line1" /> property's name.
        /// </summary>
        public const string Line1PropertyName = "Line1";

        private string _Line1 = default(string);

        /// <summary>
        /// Sets and gets the Line1 property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Line1
        {
            get
            {
                return _Line1;
            }

            set
            {
                if (_Line1 == value)
                {
                    return;
                }

                _Line1 = value;
                RaisePropertyChanged(Line1PropertyName);
            }
        }

        #endregion

        #region Line2

        /// <summary>
        /// The <see cref="Line2" /> property's name.
        /// </summary>
        public const string Line2PropertyName = "Line2";

        private string _Line2 = default(string);

        /// <summary>
        /// Sets and gets the Line2 property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Line2
        {
            get
            {
                return _Line2;
            }

            set
            {
                if (_Line2 == value)
                {
                    return;
                }

                _Line2 = value;
                RaisePropertyChanged(Line2PropertyName);
            }
        }

        #endregion

        #region Line3

        /// <summary>
        /// The <see cref="Line3" /> property's name.
        /// </summary>
        public const string Line3PropertyName = "Line3";

        private string _Line3 = default(string);

        /// <summary>
        /// Sets and gets the Line3 property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Line3
        {
            get
            {
                return _Line3;
            }

            set
            {
                if (_Line3 == value)
                {
                    return;
                }

                _Line3 = value;
                RaisePropertyChanged(Line3PropertyName);
            }
        }

        #endregion

        #region City

        /// <summary>
        /// The <see cref="City" /> property's name.
        /// </summary>
        public const string CityPropertyName = "City";

        private string _City = default(string);

        /// <summary>
        /// Sets and gets the City property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string City
        {
            get
            {
                return _City;
            }

            set
            {
                if (_City == value)
                {
                    return;
                }

                _City = value;
                RaisePropertyChanged(CityPropertyName);
            }
        }

        #endregion

        #region State

        /// <summary>
        /// The <see cref="State" /> property's name.
        /// </summary>
        public const string StatePropertyName = "State";

        private string _State = default(string);

        /// <summary>
        /// Sets and gets the State property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string State
        {
            get
            {
                return _State;
            }

            set
            {
                if (_State == value)
                {
                    return;
                }

                _State = value;
                RaisePropertyChanged(StatePropertyName);
            }
        }

        #endregion

        #region PostalCode

        /// <summary>
        /// The <see cref="PostalCode" /> property's name.
        /// </summary>
        public const string PostalCodePropertyName = "PostalCode";

        private string _PostalCode = default(string);

        /// <summary>
        /// Sets and gets the PostalCode property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string PostalCode
        {
            get
            {
                return _PostalCode;
            }

            set
            {
                if (_PostalCode == value)
                {
                    return;
                }

                _PostalCode = value;
                RaisePropertyChanged(PostalCodePropertyName);
            }
        }

        #endregion

        #region Country

        /// <summary>
        /// The <see cref="Country" /> property's name.
        /// </summary>
        public const string CountryPropertyName = "Country";

        private string _Country = default(string);

        /// <summary>
        /// Sets and gets the Country property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Country
        {
            get
            {
                return _Country;
            }

            set
            {
                if (_Country == value)
                {
                    return;
                }

                _Country = value;
                RaisePropertyChanged(CountryPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public RelayCommand BackCommand { get; }
        public RelayCommand DoneCommand { get; }
       
        #endregion

        #region Constructor

        public EditAddressViewModel()
        {
            BackCommand = new RelayCommand(BackCommandRecieverAsync);
            DoneCommand = new RelayCommand(DoneCommandRecieverAsync);
        }

        #endregion

        #region Methods
        private async void DoneCommandRecieverAsync()
        {
            Address address = new Address()
            {
                Line1 = Line1,
                Line2 = Line2,
                Line3 = Line3,
                City = City,
                State = State,
                PostalCode = PostalCode,
                Country = Country
            };

            if (AddressTitle.Contains("Shipping"))
                SimpleIoc.Default.GetInstance<AddPartnerViewModel>().ShipAddress = address;
            else
                SimpleIoc.Default.GetInstance<AddPartnerViewModel>().BillAddress = address;

            await Application.Current.MainPage.Navigation.PopModalAsync();

            CleanupData();
        }

        private void CleanupData()
        {
            Line1 = default(string);
            Line2 = default(string);
            Line3 = default(string);
            City = default(string); 
            State = default(string);
            PostalCode = default(string);
            Country = default(string);
        }

        private async void BackCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PopModalAsync();

        #endregion
    }
}
