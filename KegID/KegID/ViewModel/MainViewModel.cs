using KegID.Services;

namespace KegID.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        #region Properties

        private readonly ISyncManager _syncManager;

        #endregion

        #region Commands

        #endregion

        #region Constructor

        public MainViewModel(ISyncManager syncManager)
        {
            _syncManager = syncManager;
            _syncManager.NotifyConnectivityChanged();
        }

        #endregion

        #region Methods


        #endregion
    }
}
