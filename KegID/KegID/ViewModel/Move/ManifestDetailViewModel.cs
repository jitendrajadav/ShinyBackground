using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.DependencyServices;
using KegID.Model;
using KegID.Model.PrintPDF;
using KegID.Views;
using Microsoft.AppCenter.Crashes;
using System;
using System.IO;
using System.Linq;
using System.Text;
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

        private DateTimeOffset _ShippingDate = DateTimeOffset.UtcNow.Date;

        /// <summary>
        /// Sets and gets the ShippingDate property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DateTimeOffset ShippingDate
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
            ShareCommand = new RelayCommand(ShareCommandRecieverAsync);
            GridTappedCommand = new RelayCommand(GridTappedCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void GridTappedCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PushModalAsync(new ContentTagsView(), animated: false);

        private void ShareCommandRecieverAsync()
        {
            var share = DependencyService.Get<IShareFile>();

            string output = String.Empty;
            try
            {
                var xslInput = DependencyService.Get<IXsltContent>().GetXsltContent("manifestprint.xslt");

                var xmlInput =

                new XmlSerializerHelper()
                    .Serialize(manifestPrintModels);

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

                //// Create a source for the webview
                //var htmlSource = new HtmlWebViewSource
                //{
                //    Html = output
                //};

                //// Create and populate the Xamarin.Forms.WebView
                //var browser = new WebView
                //{
                //    Source = htmlSource
                //};

                //var printService = DependencyService.Get<IPrintService>();
                //printService.Print(browser);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            try
            {
                //var bytes = Encoding.Default.GetBytes(output);
                string filecontent = string.Empty;
                // Output to app UITextView
                using (var reader = new StreamReader("manifestPDF.html"))
                {
                    filecontent = reader.ReadToEnd();
                }
                string filePath = share.SafeHTMLToPDF(filecontent, "Manifest");
                share.ShareLocalFile(filePath,"Please check Manifest PDF",null);
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
                manifest.ManifestItems.FirstOrDefault().Contents = content;
                manifestPrintModels = new Manifest
                {
                    ManifestItems = new ManifestItems
                    {
                        ManifestItem = manifest.ManifestItems,
                    },
                    TrackingNumber = manifest.TrackingNumber,
                    ShipDate = DateTimeOffset.UtcNow.Date.ToShortDateString(),
                    SenderPartner = manifest.SenderPartner,
                    ReceiverPartner = manifest.ReceiverPartner,
                    ReceiverShipAddress = manifest.ReceiverShipAddress,
                    SenderShipAddress = manifest.SenderShipAddress,
                    Contents = content
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
