using KegID.Common;
using KegID.DependencyServices;
using KegID.Model;
using KegID.Model.PrintPDF;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Xsl;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class ManifestDetailViewModel : BaseViewModel
    {
        #region Properties

        private Manifest manifestPrintModels = null;
        public List<string> Barcode { get; set; }
        public string TrackingNumber { get; set; }
        public string ManifestTo { get; set; }
        public DateTimeOffset ShippingDate { get; set; } = DateTimeOffset.UtcNow.Date;
        public int ItemCount { get; set; }
        public string Contents { get; set; }

        #endregion

        #region Commands

        public DelegateCommand ManifestsCommand { get;}
        public DelegateCommand ShareCommand { get; }
        public DelegateCommand GridTappedCommand { get; }

        #endregion

        #region Constructor

        public ManifestDetailViewModel(INavigationService navigationService) : base(navigationService)
        {
            ManifestsCommand = new DelegateCommand(ManifestsCommandRecieverAsync);
            ShareCommand = new DelegateCommand(ShareCommandRecieverAsync);
            GridTappedCommand = new DelegateCommand(GridTappedCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void GridTappedCommandRecieverAsync()
        {
            await _navigationService.NavigateAsync("ContentTagsView", new NavigationParameters
                            {
                                { "Barcode", Barcode }
                            }, animated: false);
        }

        private void ShareCommandRecieverAsync()
        {
            var share = DependencyService.Get<IShareFile>();

            string output = string.Empty;
            try
            {
                var xslInput = DependencyService.Get<IXsltContent>().GetXsltContent("manifestprint.xslt");

                var xmlInput =

                 new XmlSerializerHelper()
                    .GetSerializedString(manifestPrintModels);

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
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            try
            {
                string filePath = share.SafeHTMLToPDF(output, "Manifest",0);
                share.ShareLocalFile(filePath,"Please check Manifest PDF",null);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void ManifestsCommandRecieverAsync()
        {
            ConstantManager.Barcodes.Clear();
            var pages = Application.Current.MainPage.Navigation.NavigationStack;
            await _navigationService.GoBackToRootAsync();
        }

        internal void AssignInitialValue(ManifestResponseModel manifest, string content)
        {
            try
            {
                try
                {
                    manifest.ManifestItems.FirstOrDefault().Contents = content;
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }

                // Generating PDF...
                manifestPrintModels = new Manifest
                {
                    ManifestItems = new ManifestItems
                    {
                        ManifestItem = manifest.ManifestItems,
                    },

                    TrackingNumber = Regex.Match(manifest.TrackingNumber, @"(.{8})\s*$").Value.ToUpper(),
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

                Barcode = manifest.ManifestItems.Select(x => x.Barcode).ToList();
                Contents = !string.IsNullOrEmpty(content) ? content : "No contents";
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
                case "manifest":
                    AssignInitialValue(parameters.GetValue<ManifestResponseModel>("manifest"), parameters.GetValue<string>("Contents"));
                    break;
                case "AssignInitialValue":
                    AssignInitialValue(parameters.GetValue<ManifestResponseModel>("AssignInitialValue"), string.Empty);
                    break;
                default:
                    break;
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("ManifestsCommandRecieverAsync"))
            {
                ManifestsCommandRecieverAsync();
            }
        }

        #endregion
    }
}
