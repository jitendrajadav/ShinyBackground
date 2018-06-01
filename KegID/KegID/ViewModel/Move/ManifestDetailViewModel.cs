using FormsEZPrint.PrintTemplates;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.DependencyServices;
using KegID.Model;
using KegID.Model.PrintPDF;
using KegID.Views;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Plugin.Share;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Xsl;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class ManifestDetailViewModel : BaseViewModel
    {
        #region Properties

        private Manifest manifestPrintModels = null;

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
            try
            {
                var xslInput = DependencyService.Get<IXsltContent>().GetXsltContent("manifestprint.xslt");

                var xmlInput =

                new XmlSerializerHelper()
                    .Serialize(manifestPrintModels);

                string output = String.Empty;
                using (StringReader srt = new StringReader(xslInput)) // xslInput is a string that contains xsl
                using (StringReader sri = new StringReader(xmlInput)) // xmlInput is a string that contains xml
                {
                    using (XmlReader xrt = XmlReader.Create(srt))
                    using (XmlReader xri = XmlReader.Create(sri))
                    {
                        XslCompiledTransform xslt = new XslCompiledTransform();
                        xslt.Load(xrt);
                        using (StringWriter sw = new StringWriter())
                        using (XmlWriter xwo = XmlWriter.Create(sw, xslt.OutputSettings)) // use OutputSettings of xsl, so it can be output as HTML
                        {
                            xslt.Transform(xri, xwo);
                            output = sw.ToString();
                        }
                    }
                }

                // Create a source for the webview
                var htmlSource = new HtmlWebViewSource
                {
                    Html = output
                };

                // Create and populate the Xamarin.Forms.WebView
                var browser = new WebView
                {
                    Source = htmlSource
                };

                var printService = DependencyService.Get<IPrintService>();
                printService.Print(browser);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

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
                manifestPrintModels = new Manifest
                {
                    ManifestItems = new ManifestItems
                    {
                        ManifestItem = new List<Model.PrintPDF.ManifestItem>
                         {
                             new Model.PrintPDF.ManifestItem
                             {
                                  Pallet = new Model.PrintPDF.Pallet
                                  {
                                       Barcode = manifest.ManifestItems.FirstOrDefault().Barcode,
                                  },
                             }
                         }
                    },
                    TrackingNumber = manifest.TrackingNumber,
                    ShipDate = new ShipDate
                    {
                        DateTime = manifest.ShipDate,
                        OffsetMinutes = "0"
                    },
                    SubmittedDate = new SubmittedDate
                    {
                        DateTime = manifest.ShipDate,
                        OffsetMinutes = "0"
                    },
                    SenderPartner = new SenderPartner
                    {
                        Address = manifest.SenderPartner.Address,
                        Address1 = manifest.SenderPartner.Address1,
                        City = manifest.SenderPartner.City,
                        FullName = manifest.SenderPartner.FullName,
                        IsActive = manifest.SenderPartner.IsActive.ToString(),
                        IsInternal = manifest.SenderPartner.IsInternal.ToString(),
                        IsShared = manifest.SenderPartner.IsShared.ToString(),
                        Lat = manifest.SenderPartner.Lat.ToString(),
                        LocationCode = manifest.SenderPartner.LocationCode,
                        Lon = manifest.SenderPartner.Lon.ToString(),
                        //ParentPartnerId = manifest.SenderPartner.ParentPartnerId,
                        //ParentPartnerName = manifest.SenderPartner.ParentPartnerName,
                        PartnerId = manifest.SenderPartner.PartnerId,
                        PartnerTypeCode = manifest.SenderPartner.PartnerTypeCode,
                        PartnerTypeName = manifest.SenderPartner.PartnerTypeName,
                        PostalCode = manifest.SenderPartner.PostalCode,
                        State = manifest.SenderPartner.State,
                    },

                    ReceiverPartner = new ReceiverPartner
                    {
                        Address = manifest.ReceiverPartner.Address,
                        Address1 = manifest.ReceiverPartner.Address1,
                        City = manifest.ReceiverPartner.City,
                        FullName = manifest.ReceiverPartner.FullName,
                        IsActive = manifest.ReceiverPartner.IsActive.ToString(),
                        IsInternal = manifest.ReceiverPartner.IsInternal.ToString(),
                        IsShared = manifest.ReceiverPartner.IsShared.ToString(),
                        Lat = manifest.ReceiverPartner.Lat.ToString(),
                        LocationCode = manifest.ReceiverPartner.LocationCode,
                        Lon = manifest.ReceiverPartner.Lon.ToString(),
                        ParentPartnerId = manifest.ReceiverPartner.ParentPartnerId,
                        ParentPartnerName = manifest.ReceiverPartner.ParentPartnerName,
                        PartnerId = manifest.ReceiverPartner.PartnerId,
                        PartnerTypeCode = manifest.ReceiverPartner.PartnerTypeCode,
                        PartnerTypeName = manifest.ReceiverPartner.PartnerTypeName,
                        PostalCode = manifest.ReceiverPartner.PostalCode,
                        State = manifest.ReceiverPartner.State,
                    },

                };

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
