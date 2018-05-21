using FormsEZPrint.PrintTemplates;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.DependencyServices;
using KegID.Model;
using KegID.Views;
using Microsoft.AppCenter.Crashes;
using Plugin.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class ManifestDetailViewModel : BaseViewModel
    {
        #region Properties

        private List<ManifestPrintModel> manifestPrintModels = null;

        #region TrackingNumber

        /// <summary>
        /// The <see cref="TrackingNumber" /> property's name.
        /// </summary>
        public const string TrackingNumberPropertyName = "TrackingNumber";

        private string _TrackingNumber = default(string);

        /// <summary>
        /// Sets and gets the TrackingNumber property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string TrackingNumber
        {
            get
            {
                return _TrackingNumber;
            }

            set
            {
                if (_TrackingNumber == value)
                {
                    return;
                }

                _TrackingNumber = value;
                RaisePropertyChanged(TrackingNumberPropertyName);
            }
        }

        #endregion

        #region ManifestTo

        /// <summary>
        /// The <see cref="ManifestTo" /> property's name.
        /// </summary>
        public const string ManifestToPropertyName = "ManifestTo";

        private string _ManifestTo = default(string);

        /// <summary>
        /// Sets and gets the ManifestTo property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ManifestTo
        {
            get
            {
                return _ManifestTo;
            }

            set
            {
                if (_ManifestTo == value)
                {
                    return;
                }

                _ManifestTo = value;
                RaisePropertyChanged(ManifestToPropertyName);
            }
        }

        #endregion

        #region ShippingDate

        /// <summary>
        /// The <see cref="ShippingDate" /> property's name.
        /// </summary>
        public const string ShippingDatePropertyName = "ShippingDate";

        private DateTime _ShippingDate = DateTime.Today;

        /// <summary>
        /// Sets and gets the ShippingDate property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DateTime ShippingDate
        {
            get
            {
                return _ShippingDate;
            }

            set
            {
                if (_ShippingDate == value)
                {
                    return;
                }

                _ShippingDate = value;
                RaisePropertyChanged(ShippingDatePropertyName);
            }
        }

        #endregion

        #region ItemCount

        /// <summary>
        /// The <see cref="ItemCount" /> property's name.
        /// </summary>
        public const string ItemCountPropertyName = "ItemCount";

        private int _ItemCount = 0;

        /// <summary>
        /// Sets and gets the ItemCount property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int ItemCount
        {
            get
            {
                return _ItemCount;
            }

            set
            {
                if (_ItemCount == value)
                {
                    return;
                }

                _ItemCount = value;
                RaisePropertyChanged(ItemCountPropertyName);
            }
        }

        #endregion

        #region Contents

        /// <summary>
        /// The <see cref="Contents" /> property's name.
        /// </summary>
        public const string ContentsPropertyName = "Contents";

        private string _Contents = "No contents";

        /// <summary>
        /// Sets and gets the Contents property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Contents
        {
            get
            {
                return _Contents;
            }

            set
            {
                if (_Contents == value)
                {
                    return;
                }

                _Contents = value;
                RaisePropertyChanged(ContentsPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public RelayCommand ManifestsCommand { get;}
        public RelayCommand ShareCommand { get; }
        public RelayCommand GridTappedCommand { get; }

        #endregion

        #region Constructor

        public ManifestDetailViewModel()
        {
            ManifestsCommand = new RelayCommand(ManifestsCommandRecieverAsync);
            ShareCommand = new RelayCommand(ShareCommandReciever);
            GridTappedCommand = new RelayCommand(GridTappedCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void GridTappedCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PushModalAsync(new ContentTagsView(), animated: false);

        private void ShareCommandReciever()
        {
            // New up the Razor template
            var printTemplate = new ManifestPrintTemplate();

            // Set the model property (ViewModel is a custom property within containing view - FYI)
            printTemplate.Model = manifestPrintModels;//new System.Collections.Generic.List<EZPrintModel>();

            // Generate the HTML
            var htmlString = printTemplate.GenerateString();

            // Create a source for the webview
            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = htmlString;

            // Create and populate the Xamarin.Forms.WebView
            var browser = new WebView();
            browser.Source = htmlSource;

            var printService = DependencyService.Get<IPrintService>();
            printService.Print(browser);

            try
            {
                CrossShare.Current.Share(message: new Plugin.Share.Abstractions.ShareMessage
                {
                    Text = "Share",
                    Title = "Share",
                    Url = "https://www.slg.com/"
                });
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void ManifestsCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PopModalAsync();

        internal void AssignInitialValue(ManifestResponseModel manifest, string content)
        {
            try
            {
                manifestPrintModels = new List<ManifestPrintModel>();
                manifestPrintModels.Add(new ManifestPrintModel { Barcode = "1234566", Brand = "test", Destination = manifest.CreatorCompany.FullName + "\n" + manifest.CreatorCompany.PartnerTypeName, Item = "1", ShipDate = Convert.ToDateTime(manifest.ShipDate), Tracking = manifest.TrackingNumber, Order = "123", Original = "test123" });
                TrackingNumber = manifest.TrackingNumber;

                ManifestTo = manifest.CreatorCompany.FullName + "\n" + manifest.CreatorCompany.PartnerTypeName;

                ShippingDate = Convert.ToDateTime(manifest.ShipDate);
                ItemCount = manifest.ManifestItems.Count;
                SimpleIoc.Default.GetInstance<ContentTagsViewModel>().ContentCollection = manifest.ManifestItems.Select(x => x.Barcode).ToList();

                Contents = !string.IsNullOrEmpty(content) ? content : "No contens";
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        #endregion
    }
}
