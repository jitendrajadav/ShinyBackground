using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;
using KegID.Common;
using KegID.DependencyServices;
using KegID.Model;
using KegID.Model.PrintPDF;
using KegID.Services;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class PalletizeDetailViewModel : BaseViewModel
    {
        #region Properties

        private Model.PrintPDF.Pallet PalletPrintModels = null;
        private SearchPalletResponseModel Model { get; set; }
        public List<string> Barcodes { get; private set; }
        private readonly IUuidManager _uuidManager;

        public string ManifestId { get; set; }
        public string StockLocation { get; set; }
        public string PartnerTypeName { get; set; }
        public DateTime ShippingDate { get; set; } = DateTime.Today;
        public string TargetLocation { get; set; }
        public int ItemCount { get; set; }
        public string Contents { get; set; } = "No contents";
        public bool IsFromDashboard { get; set; }

        #endregion

        #region Commands

        public DelegateCommand HomeCommand { get; }
        public DelegateCommand ShareCommand { get; }
        public DelegateCommand GridTappedCommand { get; }
        public DelegateCommand EditPalletCommand { get; }
        public DelegateCommand MovePalletCommand { get; }

        #endregion

        #region Constructor

        public PalletizeDetailViewModel(INavigationService navigationService, IUuidManager uuidManager) : base(navigationService)
        {
            _uuidManager = uuidManager;

            HomeCommand = new DelegateCommand(HomeCommandRecieverAsync);
            ShareCommand = new DelegateCommand(ShareCommandReciever);
            GridTappedCommand = new DelegateCommand(GridTappedCommandRecieverAsync);
            MovePalletCommand = new DelegateCommand(MovePalletCommandRecieverAsync);
            EditPalletCommand = new DelegateCommand(EditPalletCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void EditPalletCommandRecieverAsync()
        {
            await NavigationService.NavigateAsync("PalletizeView", animated: false);
        }

        private async void MovePalletCommandRecieverAsync()
        {
            // here we needs to pass value for e.g Barcode only string value, KegId Optional and Manifest but those value we have to fetch from
            // AssingIntialValueAsync() this needs to correct
            await NavigationService.NavigateAsync("MoveView", new NavigationParameters
                    {
                        { "AssignInitialValueFromKegStatus", Barcodes },
                        { "KegId", string.Empty },
                        { "ManifestId", _uuidManager.GetUuId() }
                    }, animated: false);
        }

        private async void GridTappedCommandRecieverAsync()
        {
            await NavigationService.NavigateAsync("ContentTagsView", new NavigationParameters
                            {
                                { "Barcode", Barcodes }
                            }, animated: false);
        }

        private void ShareCommandReciever()
        {
            var share = DependencyService.Get<IShareFile>();

            string output = string.Empty;

            var xslInput = DependencyService.Get<IXsltContent>().GetXsltContent("palletprintnew.xslt");

            var xmlInput =

             new XmlSerializerHelper()
                .GetSerializedString(PalletPrintModels);

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

            string filePath = share.SafeHTMLToPDF(output, "Pallet", 1);
            share.ShareLocalFile(filePath, "Please check Pallet PDF", null);
        }

        private async void HomeCommandRecieverAsync()
        {
            ConstantManager.Barcodes.Clear();
            await NavigationService.GoBackToRootAsync();
        }

        internal void LoadInfo(PalletResponseModel value)
        {
            PalletPrintModels = new Model.PrintPDF.Pallet
            {
                Barcode = value.Barcode,
                BuildDate = value.BuildDate.UtcDateTime.ToShortDateString(),
                PalletId = value.PalletId,
                Location = value.Location,
                Owner = value.Owner,
                StockLocation = value.StockLocation,
                Container = new Container { Nil = "" },
                CreatedDate = DateTimeOffset.UtcNow.Date.ToShortDateString(),
                DateInfo = new DateInfo { },
                TargetLocation = value.TargetLocation,

                PalletItems = new PalletItems { PalletItem = new List<Model.PrintPDF.PalletItem> { } },
                SummaryItems = new SummaryItems
                {
                    SummaryItem = new List<SummaryItem>
                           {
                                new SummaryItem
                                {
                                     Combination = "test",
                                      Contents= "1/4 contents",
                                       Quantity = "1",
                                        Size = "1/2"
                                }
                           }
                }
            };

            foreach (var item in value.PalletItems)
            {
                PalletPrintModels.PalletItems.PalletItem.Add(new Model.PrintPDF.PalletItem
                {
                    Barcode = item.Barcode,
                    Contents = ConstantManager.Contents,
                    //ContentsKey = item.Contents,
                    DateScanned = item.DateScanned.Date.ToShortDateString(),
                    Keg = new Model.PrintPDF.Keg
                    {
                        Barcode = item?.Keg?.Barcode,
                        Contents = "Future Factory",
                        KegId = item?.Keg?.KegId,
                        LocationId = item?.Keg?.LocationId,
                        LocationName = item?.Keg?.LocationName,
                        OwnerId = item?.Keg?.OwnerId,
                        OwnerName = item?.Keg?.OwnerName,
                        PalletId = item?.Keg?.PalletId,
                        PalletName = item?.Keg?.PalletName,
                        ReceivedDate = item?.Keg?.ReceivedDate.Date.ToShortDateString(),
                        SizeName = item?.Keg?.SizeName,
                        TypeName = item?.Keg?.TypeName
                    },
                    PalletId = item.PalletId,
                    IsActive = item.IsActive.ToString(),
                });
            }
            ManifestId = value.Barcode;
            PartnerTypeName = value.StockLocation.PartnerTypeName;
            StockLocation = value.StockLocation.FullName;
            TargetLocation = value.StockLocation.FullName;
            ShippingDate = value.BuildDate.Date;
            ItemCount = value.PalletItems.Count;
            Barcodes = value.PalletItems.Select(selector: x => x.Barcode).ToList();
        }

        internal async void AssingIntialValueAsync(SearchPalletResponseModel model, bool v)
        {
            var response = await ApiManager.GetManifest(model.PalletId, Settings.SessionId);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = await Task.Run(() => JsonConvert.DeserializeObject<ManifestResponseModel>(json, GetJsonSetting()));
            }
            Model = model;
            IsFromDashboard = v;
            ManifestId = model.Barcode;
            PartnerTypeName = model.OwnerName;
            StockLocation = model.LocationName;
            TargetLocation = model.LocationName;
            ShippingDate = model.BuildDate.Date;
            ItemCount = (int)model.BuildCount;
            Barcodes = new List<string> { model.Barcode };
        }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            switch (parameters.Keys.FirstOrDefault())
            {
                case "LoadInfo":
                    LoadInfo(parameters.GetValue<PalletResponseModel>("LoadInfo"));
                    break;
                case "model":
                    AssingIntialValueAsync(parameters.GetValue<SearchPalletResponseModel>("model"), true);
                    break;
                default:
                    break;
            }

            return base.InitializeAsync(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("HomeCommandRecieverAsync"))
            {
                HomeCommandRecieverAsync();
            }
        }

        #endregion
    }
}
