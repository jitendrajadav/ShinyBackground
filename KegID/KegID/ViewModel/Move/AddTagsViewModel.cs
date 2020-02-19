using KegID.Services;
using Prism.Commands;
using Prism.Navigation;
using System;

namespace KegID.ViewModel
{
    public class AddTagsViewModel : BaseViewModel
    {
        #region Properties

        public DateTime ProductionDate { get; set; } = DateTime.Now.Date;
        public DateTime BestByDataDate { get; set; } = DateTime.Now;
        public string Barcode { get; set; }

        #endregion

        #region Commands

        public DelegateCommand SaveCommand { get; }

        #endregion

        #region Contructor

        public AddTagsViewModel(INavigationService navigationService) : base(navigationService)
        {
            SaveCommand = new DelegateCommand(SaveCommandReciever);
        }

        #endregion

        #region Methods

        private async void SaveCommandReciever()
        {
            if (!string.IsNullOrEmpty(Barcode))
            {
                await NavigationService.GoBackAsync(new NavigationParameters
                                {
                                    { "AddTags", ConstantManager.Tags },
                                    { "Barcode", Barcode }
                            }, animated: false);
            }
            else
            {
                await NavigationService.GoBackAsync(new NavigationParameters
                                {
                                    { "AddTags", ConstantManager.Tags },
                            }, animated: false);
            }
        }

        #endregion
    }
}
