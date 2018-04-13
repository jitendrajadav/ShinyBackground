using KegID.Model;
using KegID.Views;
using System.Collections.ObjectModel;

namespace KegID.ViewModel
{
    public class KegIDMasterPageMasterViewModel : BaseViewModel
    {
        #region Properties

        #region MenuItems

        /// <summary>
        /// The <see cref="MenuItems" /> property's name.
        /// </summary>
        public const string MenuItemsPropertyName = "MenuItems";

        private ObservableCollection<KegIDMasterPageMenuItem> _menuItems = null;

        /// <summary>
        /// Sets and gets the MenuItems property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<KegIDMasterPageMenuItem> MenuItems
        {
            get
            {
                return _menuItems;
            }

            set
            {
                if (_menuItems == value)
                {
                    return;
                }

                _menuItems = value;
                RaisePropertyChanged(MenuItemsPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        #endregion

        #region Contstructor

        public KegIDMasterPageMasterViewModel()
        {
            MenuItems = new ObservableCollection<KegIDMasterPageMenuItem>(new[]
            {
                    new KegIDMasterPageMenuItem { Id = 0, Title = "Dashboard", MenuIcon="Assets/partners.png", TargetType = typeof(DashboardView)},
                    new KegIDMasterPageMenuItem { Id = 1, Title = "Move", MenuIcon="Assets/movekegs.png",TargetType = typeof(MoveView) },
                    new KegIDMasterPageMenuItem { Id = 2, Title = "Fill", MenuIcon="Assets/fillkegs.png",TargetType = typeof(FillView)},
                    new KegIDMasterPageMenuItem { Id = 3, Title = "Palletize", MenuIcon="Assets/pallet.png",TargetType = typeof(PalletizeView)},
                    new KegIDMasterPageMenuItem { Id = 4, Title = "Maintain", MenuIcon="Assets/repair.png",TargetType = typeof(MaintainView)},
                    new KegIDMasterPageMenuItem { Id = 5, Title = "Setting", MenuIcon="Assets/setting.png",TargetType = typeof(SettingView)},
                    new KegIDMasterPageMenuItem { Id = 6, Title = "Logout", MenuIcon="Assets/logout.png",TargetType = typeof(LoginView)},
            });
        }

        #endregion

        #region Methods

        #endregion
    }
}
