using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Xsl;
using KegID.Common;
using KegID.DependencyServices;
using KegID.Model;
using KegID.Model.PrintPDF;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class PalletizeDetailViewModel : BaseViewModel
    {
        #region Properties

        private Model.PrintPDF.Pallet PalletPrintModels = null;
        //private readonly INavigationService _navigationService;
        private readonly IMoveService _moveService;
        private SearchPalletResponseModel Model { get; set; }
        public List<string> Barcodes { get; private set; }

        #region ManifestId

        /// <summary>
        /// The <see cref="ManifestId" /> property's name.
        /// </summary>
        public const string ManifestIdPropertyName = "ManifestId";

        private string _ManifestId = default;

        /// <summary>
        /// Sets and gets the ManifestId property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ManifestId
        {
            get
            {
                return _ManifestId;
            }

            set
            {
                if (_ManifestId == value)
                {
                    return;
                }

                _ManifestId = value;
                RaisePropertyChanged(ManifestIdPropertyName);
            }
        }

        #endregion

        #region StockLocation

        /// <summary>
        /// The <see cref="StockLocation" /> property's name.
        /// </summary>
        public const string StockLocationPropertyName = "StockLocation";

        private string _StockLocation = default;

        /// <summary>
        /// Sets and gets the StockLocation property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string StockLocation
        {
            get
            {
                return _StockLocation;
            }

            set
            {
                if (_StockLocation == value)
                {
                    return;
                }

                _StockLocation = value;
                RaisePropertyChanged(StockLocationPropertyName);
            }
        }

        #endregion

        #region PartnerTypeName

        /// <summary>
        /// The <see cref="PartnerTypeName" /> property's name.
        /// </summary>
        public const string PartnerTypeNamePropertyName = "PartnerTypeName";

        private string _PartnerTypeName = default;

        /// <summary>
        /// Sets and gets the PartnerTypeName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string PartnerTypeName
        {
            get
            {
                return _PartnerTypeName;
            }

            set
            {
                if (_PartnerTypeName == value)
                {
                    return;
                }

                _PartnerTypeName = value;
                RaisePropertyChanged(PartnerTypeNamePropertyName);
            }
        }

        #endregion

        #region ShippingDate

        /// <summary>
        /// The <see cref="ShippingDate" /> property's name.
        /// </summary>
        public const string ShippingDatePropertyName = "ShippingDate";

        private DateTimeOffset _ShippingDate = DateTimeOffset.Now;

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

        #region TargetLocation

        /// <summary>
        /// The <see cref="TargetLocation" /> property's name.
        /// </summary>
        public const string TargetLocationPropertyName = "TargetLocation";

        private string _TargetLocation = default;

        /// <summary>
        /// Sets and gets the TargetLocation property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string TargetLocation
        {
            get
            {
                return _TargetLocation;
            }

            set
            {
                if (_TargetLocation == value)
                {
                    return;
                }

                _TargetLocation = value;
                RaisePropertyChanged(TargetLocationPropertyName);
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

        #region IsFromDashboard

        /// <summary>
        /// The <see cref="IsFromDashboard" /> property's name.
        /// </summary>
        public const string IsFromDashboardPropertyName = "IsFromDashboard";

        private bool _IsFromDashboard = false;

        /// <summary>
        /// Sets and gets the IsFromDashboard property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsFromDashboard
        {
            get
            {
                return _IsFromDashboard;
            }

            set
            {
                if (_IsFromDashboard == value)
                {
                    return;
                }

                _IsFromDashboard = value;
                RaisePropertyChanged(IsFromDashboardPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand HomeCommand { get; }
        public DelegateCommand ShareCommand { get; }
        public DelegateCommand GridTappedCommand { get; }
        public DelegateCommand EditPalletCommand { get; }
        public DelegateCommand MovePalletCommand { get; }

        #endregion

        #region Constructor

        public PalletizeDetailViewModel(IMoveService moveService, INavigationService navigationService) : base(navigationService)
        {
            //_navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            _moveService = moveService;
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
            try
            {
                await _navigationService.NavigateAsync("PalletizeView", animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void MovePalletCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync("MoveView", animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void GridTappedCommandRecieverAsync()
        {
            await _navigationService.NavigateAsync("ContentTagsView", new NavigationParameters
                            {
                                { "Barcode", Barcodes }
                            }, animated: false);
        }

        private void ShareCommandReciever()
        {
            var share = DependencyService.Get<IShareFile>();

            string output = string.Empty;
            try
            {
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
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            try
            {
                string filePath = share.SafeHTMLToPDF(output, "Pallet",1);
                share.ShareLocalFile(filePath, "Please check Pallet PDF", null);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void HomeCommandRecieverAsync()
        {
            ConstantManager.Barcodes.Clear();
            await _navigationService.GoBackToRootAsync();
        }

        internal void LoadInfo(PalletResponseModel value)
        {
            try
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
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal async void AssingIntialValueAsync(SearchPalletResponseModel model, bool v)
        {
            try
            {
                var manifest = await _moveService.GetManifestAsync(AppSettings.SessionId, model.Barcode);

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
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
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
