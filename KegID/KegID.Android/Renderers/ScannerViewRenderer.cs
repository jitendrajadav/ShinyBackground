
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Cognex.Dataman.Sdk;
using Com.Cognex.Mobile.Barcode.Sdk;
using Java.Lang;
using KegID;
using KegID.Droid.Renderers;
using KegID.Views;
using Xamarin.Forms.Platform.Android;
using static Com.Cognex.Mobile.Barcode.Sdk.ReaderDevice;

[assembly: Xamarin.Forms.ExportRenderer(typeof(CameraPreview), typeof(ScannerViewRenderer))]
namespace KegID.Droid.Renderers
{
    public class ScannerViewRenderer : ViewRenderer<CameraPreview, RelativeLayout>, IOnConnectionCompletedListener, IReaderDeviceListener
    {
        private Xamarin.Forms.Label tvConnectionStatus, tvSymbology, tvCode;
        private Xamarin.Forms.Button btnScan;
        private RelativeLayout rlMainContainer;
        private ImageView ivPreview;
        private Context mContext;

        private bool isScanning = false;

        // public
        public static ReaderDevice readerDevice;
        public static ISharedPreferences sharedPreferences;

        // SHARED PREFERENCES KEYS
        public static string SELECTED_DEVICE = "selectedDevice";
        public static string ACTIVE_SYMBOLOGIES = "activeSymbologies";

        //------
        public enum DeviceType { MX_1000, MOBILE_DEVICE }

        public static bool isDevicePicked = false;
        public static DeviceType param_deviceType = DeviceType.MOBILE_DEVICE;

        public static bool dialogAppeared = false;
        //------

        public static string selectedDevice = "";
        public static bool fragmentActive = false;


        public ScannerViewRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<CameraPreview> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            mContext = Context;

            rlMainContainer = new RelativeLayout(mContext);
            rlMainContainer.LayoutParameters = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
            ivPreview = new ImageView(mContext);
            ivPreview.LayoutParameters = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
            ivPreview.SetScaleType(ImageView.ScaleType.FitCenter);
            rlMainContainer.AddView(ivPreview);

            if (Control == null)
                SetNativeControl(rlMainContainer);

            // views
            tvConnectionStatus = CognexScanView._lblStatus;
            tvSymbology = CognexScanView._lblResultType;
            tvCode = CognexScanView._lblResult;

            Xamarin.Forms.ToolbarItem toolbarItemSelectDevice = CognexScanView._toolbarItemSelectDevice;
            toolbarItemSelectDevice.Clicked += (sender, eve) =>
            {
                pickDevice(true);
            };

            btnScan = CognexScanView._btnScan;
            btnScan.IsEnabled = false;
            btnScan.Clicked += (sender, ev) =>
            {
                if (ScannerViewRenderer.readerDevice != null)
                {
                    if (!isScanning)
                    {
                        tvSymbology.Text = "";
                        tvCode.Text = "";
                        tvConnectionStatus.IsVisible = false;

                        ScannerViewRenderer.readerDevice.StartScanning();
                        isScanning = true;
                        btnScan.Text = "STOP SCANNING";
                    }
                    else
                    {
                        ScannerViewRenderer.readerDevice.StopScanning();
                        isScanning = false;
                        btnScan.Text = "START SCANNING";
                    }
                }
                else
                {
                    isScanning = false;
                }
            };
        }

        protected override void OnVisibilityChanged(View changedView, [GeneratedEnum] ViewStates visibility)
        {
            base.OnVisibilityChanged(changedView, visibility);

            if (visibility == ViewStates.Invisible || visibility == ViewStates.Gone)
            {
                if (readerDevice != null && readerDevice.ConnectionState == ConnectionState.Connected)
                {
                    readerDevice.Disconnect();
                }

                if (readerDevice != null)
                    try
                    {
                        readerDevice.StopAvailabilityListening();
                    }
                    catch (System.Exception e) { }
            }
            else
            {
                initDevice();
            }
        }

        private void initDevice()
        {
            if (!ScannerViewRenderer.isDevicePicked)
            {
                if (!ScannerViewRenderer.dialogAppeared)
                    pickDevice(false);
                return;
            }

            switch (ScannerViewRenderer.param_deviceType)
            {

                default:
                case DeviceType.MX_1000:
                    ScannerViewRenderer.readerDevice = ReaderDevice.GetMXDevice(mContext);
                    if (!listeningForUSB)
                    {
                        listeningForUSB = true;
                    }
                    selectedDevice = "MX Scanner";
                    break;

                case DeviceType.MOBILE_DEVICE:
                    ScannerViewRenderer.readerDevice = ReaderDevice.GetPhoneCameraDevice(mContext, CameraMode.NoAimer, PreviewOption.Defaults, rlMainContainer);
                    selectedDevice = "Mobile Camera";
                    break;
            }

            ScannerViewRenderer.readerDevice.SetReaderDeviceListener(this);
            ScannerViewRenderer.readerDevice.EnableImage(true);
            ScannerViewRenderer.readerDevice.Connect(this);
        }

        private void pickDevice(bool cancelable)
        {
            if (listeningForUSB)
            {
                readerDevice.StopAvailabilityListening();

                listeningForUSB = false;
            }

            if (ScannerViewRenderer.readerDevice != null)
            {
                ScannerViewRenderer.readerDevice.Disconnect();
                ScannerViewRenderer.readerDevice = null;
            }

            AlertDialog.Builder devicePickerBuilder = new AlertDialog.Builder(mContext);
            devicePickerBuilder.SetTitle("Select device");

            ArrayAdapter<string> arrayAdapter = new ArrayAdapter<string>(mContext, Android.Resource.Layout.SimpleListItem1);
            arrayAdapter.Add("MX Scanner");
            arrayAdapter.Add("Mobile Camera");

            if (cancelable)
            {
                devicePickerBuilder.SetNegativeButton("Cancel", (sender, e) =>
                {
                    initDevice();
                });
            }

            devicePickerBuilder.SetAdapter(arrayAdapter, (sender, e) =>
            {
                ScannerViewRenderer.param_deviceType = (e.Which == 0 ? DeviceType.MX_1000 : DeviceType.MOBILE_DEVICE);
                ScannerViewRenderer.isDevicePicked = true;
                ScannerViewRenderer.dialogAppeared = false;
                initDevice();
            });

            AlertDialog devicePicker = devicePickerBuilder.Create();
            devicePicker.SetCancelable(false);
            devicePicker.SetCanceledOnTouchOutside(false);
            devicePicker.Show();

            ScannerViewRenderer.dialogAppeared = true;
        }

        // USB Listener, no need for conditional code
        bool listeningForUSB = false;

        public void OnConnectionStateChanged(ReaderDevice reader)
        {
            if (reader.ConnectionState == ConnectionState.Connected)
            {
                readerConnected();
            }
            else if (reader.ConnectionState == ConnectionState.Disconnected)
            {
                readerDisconnected();
            }
        }

        public void OnReadResultReceived(ReaderDevice reader, ReadResults results)
        {
            if (results.Count > 0)
            {
                ReadResult result = results.GetResultAt(0);

                if (result.IsGoodRead)
                {
                    Symbology sym = result.Symbology;
                    if (sym != null)
                    {
                        tvSymbology.Text = sym.Name;
                    }
                    else
                    {
                        tvSymbology.Text = "UNKNOWN SYMBOLOGY";
                    }
                    tvCode.Text = result.ReadString;
                }
                else
                {
                    tvSymbology.Text = "NO READ";
                    tvCode.Text = "";
                }


                ivPreview.SetImageBitmap(result.Image);
            }

            tvConnectionStatus.IsVisible = true;
            isScanning = false;
            btnScan.Text = "START SCANNING";
        }

        public void OnAvailabilityChanged(ReaderDevice reader)
        {
            if (reader.GetAvailability() == Availability.Available)
            {
                ScannerViewRenderer.readerDevice.Connect(this);
            }
            else
            {
                // DISCONNECTED USB DEVICE
                ScannerViewRenderer.readerDevice.Disconnect();
                readerDisconnected();
            }
        }

        public void OnConnectionCompleted(ReaderDevice reader, Throwable error)
        {
            if (error != null)
            {
                readerDisconnected();
            }
        }

        ///////////////////////////////////////////////
        ///////////////////////////////////////////////
        // the methods below are NOT from the Cognex SDK
        private void readerDisconnected()
        {
            tvConnectionStatus.Text = "Disconnected";
            tvConnectionStatus.BackgroundColor = Xamarin.Forms.Color.FromHex("#ffff4444");
            btnScan.IsEnabled = false;
            isScanning = false;
            btnScan.Text = "START SCANNING";
            tvConnectionStatus.IsVisible = true;
        }

        private void readerConnected()
        {
            // there is a SDK method for onConnected, but not for onDisconnected,
            // for
            // consistency I created this one and use the onReaderStatusChanged

            tvConnectionStatus.Text = selectedDevice + "\nConnected";
            tvConnectionStatus.BackgroundColor = Xamarin.Forms.Color.FromHex("#ff669900");
            btnScan.IsEnabled = true;
            isScanning = false;
            btnScan.Text = "START SCANNING";
            tvConnectionStatus.IsVisible = true;

            //example setSymbologyEnabled
            ScannerViewRenderer.readerDevice.SetSymbologyEnabled(Symbology.C128, true, null);
            ScannerViewRenderer.readerDevice.SetSymbologyEnabled(Symbology.Datamatrix, true, null);
            ScannerViewRenderer.readerDevice.SetSymbologyEnabled(Symbology.UpcEan, true, null);
            ScannerViewRenderer.readerDevice.SetSymbologyEnabled(Symbology.Qr, true, null);

            //example sendCommand
            ScannerViewRenderer.readerDevice.DataManSystem.SendCommand("SET SYMBOL.MICROPDF417 ON");
            ScannerViewRenderer.readerDevice.DataManSystem.SendCommand("SET IMAGE.SIZE 0");
        }

        private Symbology symbologyFromString(string symbol)
        {
            symbol = symbol.ToUpper();

            if (symbol.Equals("SYMBOL.DATAMATRIX"))
                return Symbology.Datamatrix;

            if (symbol.Equals("SYMBOL.QR"))
                return Symbology.Qr;

            if (symbol.Equals("SYMBOL.C128"))
                return Symbology.C128;

            if (symbol.Equals("SYMBOL.UPC-EAN"))
                return Symbology.UpcEan;

            if (symbol.Equals("SYMBOL.C39"))
                return Symbology.C39;

            if (symbol.Equals("SYMBOL.C93"))
                return Symbology.C93;

            if (symbol.Equals("SYMBOL.I2O5"))
                return Symbology.I2o5;

            if (symbol.Equals("SYMBOL.CODABAR"))
                return Symbology.Codabar;

            if (symbol.Equals("SYMBOL.EAN-UCC"))
                return Symbology.EanUcc;

            if (symbol.Equals("SYMBOL.PHARMACODE"))
                return Symbology.Pharmacode;

            if (symbol.Equals("SYMBOL.MAXICODE"))
                return Symbology.Maxicode;

            if (symbol.Equals("SYMBOL.PDF417"))
                return Symbology.Pdf417;

            if (symbol.Equals("SYMBOL.MICROPDF417"))
                return Symbology.Micropdf417;

            if (symbol.Equals("SYMBOL.DATABAR"))
                return Symbology.Databar;

            if (symbol.Equals("SYMBOL.POSTNET"))
                return Symbology.Postnet;

            if (symbol.Equals("SYMBOL.PLANET"))
                return Symbology.Planet;

            if (symbol.Equals("SYMBOL.4STATE-JAP"))
                return Symbology.FourStateJap;

            if (symbol.Equals("SYMBOL.4STATE-AUS"))
                return Symbology.FourStateAus;

            if (symbol.Equals("SYMBOL.4STATE-UPU"))
                return Symbology.FourStateUpu;

            if (symbol.Equals("SYMBOL.4STATE-IMB"))
                return Symbology.FourStateImb;

            if (symbol.Equals("SYMBOL.VERICODE"))
                return Symbology.Vericode;

            if (symbol.Equals("SYMBOL.RPC"))
                return Symbology.Rpc;

            if (symbol.Equals("SYMBOL.MSI"))
                return Symbology.Msi;

            if (symbol.Equals("SYMBOL.AZTECCODE"))
                return Symbology.Azteccode;

            if (symbol.Equals("SYMBOL.DOTCODE"))
                return Symbology.Dotcode;

            if (symbol.Equals("SYMBOL.C25"))
                return Symbology.C25;

            if (symbol.Equals("SYMBOL.C39-CONVERT-TO-C32"))
                return Symbology.C39ConvertToC32;

            if (symbol.Equals("SYMBOL.OCR"))
                return Symbology.Ocr;

            if (symbol.Equals("SYMBOL.4STATE-RMC"))
                return Symbology.FourStateRmc;

            return null;
        }
    }
}