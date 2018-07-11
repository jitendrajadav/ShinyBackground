//using CMBSDK;
//using CoreFoundation;
//using Foundation;
//using KegID;
//using KegID.iOS.Renderers;
//using KegID.Views;
//using System;
//using System.Xml;
//using UIKit;
//using Xamarin.Forms.Platform.iOS;

//[assembly: Xamarin.Forms.ExportRenderer(typeof(CameraPreview), typeof(ScannerViewRenderer))]
//namespace KegID.iOS.Renderers
//{

//    public class ScannerViewRenderer : ViewRenderer<CameraPreview, UIImageView>, ICMBReaderDeviceDelegate
//    {
//        CMBReaderDevice readerDevice;
//        public bool isScanning = false;
//        UIImageView ivPreview;

//        public Xamarin.Forms.Label lblConnection;
//        public Xamarin.Forms.Label lblSymbology;
//        public Xamarin.Forms.Label lblCode;
//        public Xamarin.Forms.Button btnScan;

//        protected override void OnElementChanged(ElementChangedEventArgs<CameraPreview> e)
//        {
//            base.OnElementChanged(e);

//            if (e.OldElement != null || Element == null)
//            {
//                return;
//            }

//            ivPreview = new UIImageView
//            {
//                ContentMode = UIViewContentMode.ScaleToFill
//            };

//            if (Control == null)
//                SetNativeControl(ivPreview);

//            //Xamarin.Forms.ToolbarItem toolbarItemSelectDevice = CognexScanView._toolbarItemSelectDevice;
//            //toolbarItemSelectDevice.Clicked += (sender, eve) =>
//            //{
//            //    PickDevice(sender);
//            //};

//            //lblConnection = CognexScanView._lblStatus;
//            //lblSymbology = CognexScanView._lblResultType;
//            //lblCode = CognexScanView._lblResult;

//            //btnScan = CognexScanView._btnScan;
//            btnScan.IsEnabled = false;
//            btnScan.Clicked += (sender, ev) =>
//            {
//                ToggleScanner(btnScan);
//            };

//            InitReader();
//        }

//        private void InitReader()
//        {
//            if (readerDevice == null)
//            {
//                if (NSUserDefaults.StandardUserDefaults.ValueForKey((NSString)@"selectedDevice") != null)
//                {
//                    DispatchQueue.DefaultGlobalQueue.DispatchAsync(() =>
//                    {
//                        int selectedDevice = (int)NSUserDefaults.StandardUserDefaults.IntForKey((NSString)@"selectedDevice");

//                        switch (selectedDevice)
//                        {
//                            case 1:
//                                readerDevice = CMBReaderDevice.ReaderOfDeviceCameraWithCameraMode(CDMCameraMode.NoAimer, CDMPreviewOption.Defaults, ivPreview);
//                                break;
//                            case 2:
//                                readerDevice = CMBReaderDevice.ReaderOfDeviceCameraWithCameraMode(CDMCameraMode.PassiveAimer, CDMPreviewOption.Defaults, ivPreview);
//                                break;
//                            default:
//                            case 0:
//                                readerDevice = CMBReaderDevice.ReaderOfMXDevice();
//                                break;
//                        }


//                        DispatchQueue.MainQueue.DispatchAsync(() =>
//                        {
//                            readerDevice.WeakDelegate = this;
//                            ConnectToReaderDevice();
//                        });
//                    });
//                }
//                else
//                {
//                    PickDevice(null);
//                }
//            }
//            else
//            {
//                readerDevice.WeakDelegate = this;
//                ConnectToReaderDevice();
//            }
//        }

//        private void ConnectToReaderDevice()
//        {
//            if (readerDevice.Availability == CMBReaderAvailibility.Available && readerDevice.ConnectionState != CMBConnectionState.Connected)
//            {
//                readerDevice.ConnectWithCompletion((error) =>
//                {
//                    if (error != null)
//                    {
//#pragma warning disable CS0618 // Type or member is obsolete
//                        new UIAlertView("Failed to connect", error.Description, null, "OK", null).Show();
//#pragma warning restore CS0618 // Type or member is obsolete

//                        ////Create Alert
//                        //var okAlertController = UIAlertController.Create("OK Alert", "This is a sample alert with an OK button.", UIAlertControllerStyle.Alert);

//                        ////Add Action
//                        //okAlertController.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));

//                        //// Present Alert
//                        //await PresentViewControllerAsync(okAlertController, true, null);

//                        btnScan.IsEnabled = false;
//                        lblConnection.Text = " Disconnected";
//                        lblConnection.BackgroundColor = Xamarin.Forms.Color.FromHex("#ffff4444");
//                    }
//                });
//            }
//            else if (readerDevice.ConnectionState != CMBConnectionState.Connected)
//            {
//                btnScan.IsEnabled = false;
//                lblConnection.Text = "Disconnected";
//                lblConnection.BackgroundColor = Xamarin.Forms.Color.FromHex("#ffff4444");
//            }
//        }

//        private void DisconnectReaderDevice()
//        {
//            if (readerDevice == null) return;
//            if (readerDevice != null && readerDevice.ConnectionState != CMBConnectionState.Disconnected)
//            {
//                readerDevice.Disconnect();
//            }
//        }

//        void PickDevice(object sender)
//        {
//            DisconnectReaderDevice();
//            readerDevice = null;

//            UIAlertController devicePicker = UIAlertController.Create("Select device", null, UIAlertControllerStyle.ActionSheet);
//            devicePicker.AddAction(UIAlertAction.Create("MX Scanner", UIAlertActionStyle.Default, (obj) =>
//            {
//                NSUserDefaults.StandardUserDefaults.SetInt(0, "selectedDevice");
//                InitReader();
//            }));
//            devicePicker.AddAction(UIAlertAction.Create("Mobile Camera", UIAlertActionStyle.Default, (obj) =>
//            {
//                NSUserDefaults.StandardUserDefaults.SetInt(1, "selectedDevice");
//                InitReader();
//            }));
//            devicePicker.AddAction(UIAlertAction.Create("Mobile Camera w/ basic Aimer", UIAlertActionStyle.Default, (obj) =>
//            {
//                NSUserDefaults.StandardUserDefaults.SetInt(2, "selectedDevice");
//                InitReader();
//            }));

//            if (sender != null)
//            {
//                devicePicker.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, (obj) =>
//                {
//                    InitReader();
//                }));
//            }

//            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(devicePicker, true, null);
//        }

//        private void ToggleScanner(Xamarin.Forms.Button sender)
//        {
//            if (isScanning)
//            {
//                StopScan();
//                sender.Text = "START SCANNING";
//            }
//            else
//            {
//                lblSymbology.Text = "";
//                lblCode.Text = "";
//                lblConnection.IsVisible = false;

//                StartScan();
//                sender.Text = "STOP SCANNING";
//            }

//            isScanning = !isScanning;
//        }

//        private void StartScan()
//        {
//            readerDevice.StartScanning();
//        }

//        private void StopScan()
//        {
//            readerDevice.StopScanning();
//        }

//        void LoadSettings()
//        {
//            readerDevice.SetSymbology(CMBSymbology.DataMatrix, true, (error) =>
//            {
//                if (error != null)
//                {
//                    System.Diagnostics.Debug.WriteLine("FALIED TO ENABLE [DataMatrix], ", error.LocalizedDescription);
//                }
//            });
//            readerDevice.SetSymbology(CMBSymbology.Qr, true, (error) =>
//            {
//                if (error != null)
//                {
//                    System.Diagnostics.Debug.WriteLine("FALIED TO ENABLE [Qr], ", error.LocalizedDescription);
//                }
//            });
//            readerDevice.SetSymbology(CMBSymbology.C128, true, (error) =>
//            {
//                if (error != null)
//                {
//                    System.Diagnostics.Debug.WriteLine("FALIED TO ENABLE [C128], ", error.LocalizedDescription);
//                }
//            });
//            readerDevice.SetSymbology(CMBSymbology.UpcEan, true, (error) =>
//            {
//                if (error != null)
//                {
//                    System.Diagnostics.Debug.WriteLine("FALIED TO ENABLE [UpcEan], ", error.LocalizedDescription);
//                }
//            });
//            readerDevice.DataManSystem.ResultTypes = (CDMResultTypes.ReadString | CDMResultTypes.ReadXml | CDMResultTypes.Image);
//            readerDevice.DataManSystem.SendCommand("SET IMAGE.SIZE 0");
//        }

//        // READERDEVICE DELEGATE METHODS
//        [Export("didReceiveReadResultFromReader:results:")]
//        public void DidReceiveReadResultFromReader(CMBReaderDevice reader, CMBReadResults readResults)
//        {
//            btnScan.Text = "START SCANNING";
//            isScanning = false;
//            lblConnection.IsVisible = true;

//            foreach (CMBReadResult readResult in readResults.ReadResults)
//            {
//                if (readResult.Image != null)
//                {
//                    lblConnection.IsVisible = true;
//                    ivPreview.Image = readResult.Image;
//                }

//                if (readResult.XML != null)
//                {
//                    XmlDocument xml = new XmlDocument();
//                    xml.LoadXml(readResult.XML.ToString(NSStringEncoding.UTF8));// "<?xml version=\"1.0\"?>\n<result id=\"1\" image_id=\"1\" version=\"1\">\n<general>\n<status>GOOD READ</status>\n<full_string encoding=\"base64\">Knd3Lm0qbmF0ZSp3b3JrKi5jb20=</full_string>\n<symbology>QR</symbology>\n<decode_time>0</decode_time>\n</general>\n<statistics>\n<read_stats>\n<good_reads>1</good_reads>\n<bad_reads>0</bad_reads>\n<passed_validations>0</passed_validations>\n<failed_validations>0</failed_validations>\n<triggers_missed>0</triggers_missed>\n<trigger_overruns>0</trigger_overruns>\n<buffer_overflows>0</buffer_overflows>\n<item_count>0</item_count>\n</read_stats>\n</statistics>\n</result>");

//                    XmlNodeList nll = xml.GetElementsByTagName("status");

//                    if (nll.Item(0).InnerText == "GOOD READ")
//                    {
//                        lblSymbology.Text = xml.GetElementsByTagName("symbology").Item(0).InnerText;

//                        byte[] base64result = Convert.FromBase64String(xml.GetElementsByTagName("full_string").Item(0).InnerText);
//                        lblCode.Text = System.Text.Encoding.UTF8.GetString(base64result);
//                    }
//                    else
//                    {
//                        lblSymbology.Text = "NO READ";
//                        lblCode.Text = "";
//                    }
//                }
//            }
//        }

//        [Export("availabilityDidChangeOfReader:")]
//        public void AvailabilityDidChangeOfReader(CMBReaderDevice reader)
//        {
//            //base.AvailabilityDidChangeOfReader(reader);
//            if (reader.Availability != CMBReaderAvailibility.Available)
//            {
//                DisconnectReaderDevice();
//            }
//            else
//            {
//                lblSymbology.Text = "";
//                readerDevice = reader;
//                ConnectToReaderDevice();
//            }
//        }

//        [Export("connectionStateDidChangeOfReader:")]
//        public void ConnectionStateDidChangeOfReader(CMBReaderDevice reader)
//        {
//            if (reader.ConnectionState == CMBConnectionState.Connected)
//            {
//                DispatchQueue.MainQueue.DispatchAsync(() =>
//                {
//                    LoadSettings();
//                });

//                btnScan.Text = "START SCANNING";
//                isScanning = false;
//                lblConnection.IsVisible = true;

//                btnScan.IsEnabled = true;
//                try
//                {
//                    lblConnection.Text = ((int)NSUserDefaults.StandardUserDefaults.IntForKey((NSString)@"selectedDevice") == 0 ? "MX Scanner" : ((int)NSUserDefaults.StandardUserDefaults.IntForKey((NSString)@"selectedDevice") == 1 ? "Mobile Camera" : "Mobile Camera w/ basic Aimer")) + "\nConnected";
//                }
//                catch
//                {
//                    lblConnection.Text = "Connected";
//                }
//                lblConnection.BackgroundColor = Xamarin.Forms.Color.FromHex("#ff669900");
//            }
//            else
//            {
//                btnScan.IsEnabled = false;
//                lblConnection.Text = "Disconnected";
//                lblConnection.BackgroundColor = Xamarin.Forms.Color.FromHex("#ffff4444");
//            }
//        }
//    }

//    public class HomeScreenAlertDelegate : UIAlertViewDelegate
//    {
//        public override void Clicked(UIAlertView alertview, nint buttonIndex)
//        {
//            base.Clicked(alertview, buttonIndex);
//        }
//    }
//}
