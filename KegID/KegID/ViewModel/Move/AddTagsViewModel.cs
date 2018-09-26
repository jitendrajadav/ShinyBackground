﻿using KegID.Messages;
using KegID.Services;
using Prism.Commands;
using Prism.Navigation;
using System;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class AddTagsViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;

        #region ProductionDate

        /// <summary>
        /// The <see cref="ProductionDate" /> property's name.
        /// </summary>
        public const string ProductionDatePropertyName = "ProductionDate";

        private DateTime _ProductionDate = DateTime.Now.Date;

        /// <summary>
        /// Sets and gets the ProductionDate property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DateTime ProductionDate
        {
            get
            {
                return _ProductionDate;
            }

            set
            {
                if (_ProductionDate == value)
                {
                    return;
                }

                _ProductionDate = value;
                RaisePropertyChanged(ProductionDatePropertyName);
            }
        }

        #endregion

        #region BestByDataDate

        /// <summary>
        /// The <see cref="BestByDataDate" /> property's name.
        /// </summary>
        public const string BestByDataDatePropertyName = "BestByDataDate";

        private DateTime _BestByDataDate = DateTime.Now;

        /// <summary>
        /// Sets and gets the BestByDataDate property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DateTime BestByDataDate
        {
            get
            {
                return _BestByDataDate;
            }

            set
            {
                if (_BestByDataDate == value)
                {
                    return;
                }

                _BestByDataDate = value;
                RaisePropertyChanged(BestByDataDatePropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand SaveCommand { get; }

        #endregion

        #region Contructor

        public AddTagsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
        }

        #endregion

        #region Methods

        private void HandleReceivedMessages()
        {
            MessagingCenter.Subscribe<PagesMessage>(this, "PagesMessage", message =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var value = message;
                    if (value != null)
                    {
                        if (!string.IsNullOrEmpty(value.Barcode))
                        {
                            await _navigationService.GoBackAsync(new NavigationParameters
                                {
                                    { "AddTags", ConstantManager.Tags },
                                    { "Barcode", value.Barcode }
                            }, useModalNavigation: true, animated: false);
                        }
                        else
                        {
                            await _navigationService.GoBackAsync(new NavigationParameters
                                {
                                    { "AddTags", ConstantManager.Tags },
                            }, useModalNavigation: true, animated: false);
                        }
                    }
                });
            });
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            HandleReceivedMessages();
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            MessagingCenter.Unsubscribe<PagesMessage>(this, "PagesMessage");
        }

        #endregion
    }
}
