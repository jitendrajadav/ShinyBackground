using KegID.Messages;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class CognexScanViewModel : BaseViewModel
    {

        #region Properties

        private const string Cloud = "collectionscloud.png";
        private IMoveService _moveService { get; set; }
        INavigationService _navigationService;
        public IList<Tag> Tags { get; set; }
        public string TagsStr { get; set; }
        public string Page { get; set; }
        IList<BarcodeModel> models = new List<BarcodeModel>();
        public ZXing.Result Result { get; set; }

        #region IsAnalyzing
        private bool isAnalyzing = true;
        public bool IsAnalyzing
        {
            get { return isAnalyzing; }
            set
            {
                if (!Equals(isAnalyzing, value))
                {
                    isAnalyzing = value;
                    RaisePropertyChanged(nameof(IsAnalyzing));
                }
            }
        }
        #endregion

        #region IsScanning

        private bool isScanning = true;
        public bool IsScanning
        {
            get { return isScanning; }
            set
            {
                if (!Equals(isScanning, value))
                {
                    isScanning = value;
                    RaisePropertyChanged(nameof(IsScanning));
                }
            }
        }
        #endregion

        #region BottonText

        private string bottonText = default(string);
        public string BottonText
        {
            get { return bottonText; }
            set
            {
                if (!Equals(bottonText, value))
                {
                    bottonText = value;
                    RaisePropertyChanged(nameof(BottonText));
                }
            }
        }
        #endregion

        #endregion

        #region Commands

        public DelegateCommand DoneCommand { get; }
        
        public Command QRScanResultCommand
        {
            get
            {
                return new Command(() =>
                {
                    //IsAnalyzing = false;
                    //IsScanning = false;

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        var check = models.Any(x => x.Barcode == Result.Text);

                        if (!check)
                        {
                            BottonText = "Last scan: " + Result.Text;
                            BarcodeModel model = new BarcodeModel()
                            {
                                Barcode = Result.Text,
                                TagsStr = TagsStr,
                                Icon = Cloud
                            };

                            if (Tags != null)
                            {
                                foreach (var item in Tags)
                                {
                                    model.Tags.Add(item);
                                }
                            }
                            models.Add(model);
                            try
                            {
                                // Use default vibration length
                                Vibration.Vibrate();
                            }
                            catch (FeatureNotSupportedException ex)
                            {
                                // Feature not supported on device
                                Crashes.TrackError(ex);
                            }
                            catch (Exception ex)
                            {
                                // Other error has occurred.
                                Crashes.TrackError(ex);
                            }
                        }
                    });
                });
            }
        }

        #endregion

        #region Constructor

        public CognexScanViewModel(IMoveService moveService, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _moveService = moveService;

            DoneCommand = new DelegateCommand(DoneCommandRecieverAsync);
        }


        #endregion

        #region Methods

        private async void DoneCommandRecieverAsync()
        {
            var message = new StartLongRunningTaskMessage
            {
                Barcode = models.Select(x => x.Barcode).ToList(),
                PageName = Page
            };
            MessagingCenter.Send(message, "StartLongRunningTaskMessage");

            var param = new NavigationParameters
                    {
                        { "models", models }
                    };
            await _navigationService.GoBackAsync(param, useModalNavigation: true, animated: false);
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("ViewTypeEnum"))
            {
                switch (parameters.GetValue<ViewTypeEnum>("ViewTypeEnum"))
                {
                    case ViewTypeEnum.BulkUpdateScanView:
                        Tags = parameters.GetValue<List<Tag>>("Tags");
                        TagsStr = parameters.GetValue<string>("TagsStr");
                        Page = ViewTypeEnum.BulkUpdateScanView.ToString();
                        break;
                    case ViewTypeEnum.KegSearchView:
                        Tags = parameters.GetValue<List<Tag>>("Tags");
                        TagsStr = parameters.GetValue<string>("TagsStr");
                        Page = ViewTypeEnum.KegSearchView.ToString();
                        break;
                    case ViewTypeEnum.FillScanView:
                        Tags = parameters.GetValue<List<Tag>>("Tags");
                        TagsStr = parameters.GetValue<string>("TagsStr");
                        Page = ViewTypeEnum.FillScanView.ToString();
                        break;
                    case ViewTypeEnum.MaintainScanView:
                        Tags = parameters.GetValue<List<Tag>>("Tags");
                        TagsStr = parameters.GetValue<string>("TagsStr");
                        Page = ViewTypeEnum.MaintainScanView.ToString();
                        break;
                    case ViewTypeEnum.ScanKegsView:
                        Tags = parameters.GetValue<List<Tag>>("Tags");
                        TagsStr = parameters.GetValue<string>("TagsStr");
                        Page = ViewTypeEnum.ScanKegsView.ToString();
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion
    }
}
