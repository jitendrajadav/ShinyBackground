﻿//using KegID.DependencyServices;
//using System;
//using System.Collections.ObjectModel;
//using System.Threading.Tasks;
//using Xamarin.Forms;

//namespace KegID.PrintTemplates
//{
//    public enum ConnectionType
//    {
//        Bluetooth,
//        USB,
//        Network
//    }

//    public class SelectPrinterView : ContentPage
//    {
//        public delegate void PrinterSelectedHandler(IDiscoveredPrinter printer);
//        public static event PrinterSelectedHandler OnPrinterSelected;
//        public delegate void MainPageHandler();
//        public event MainPageHandler OnBackToMainPage;

//        ObservableCollection<IDiscoveredPrinter> printerList;
//        ListView printerLv;
//        ConnectionType connetionType;
//        Label statusLbl;

//        public SelectPrinterView()
//        {
//            Title = "Select a printer";

//            printerList = new ObservableCollection<IDiscoveredPrinter>();

//            printerLv = new ListView
//            {
//                ItemsSource = printerList,
//                ItemTemplate = new DataTemplate(() =>
//                {
//                    Label addressLbl = new Label();
//                    addressLbl.SetBinding(Label.TextProperty, "Address");

//                    Label friendlyLbl = new Label();
//                    friendlyLbl.SetBinding(Label.TextProperty, "FriendlyName");

//                    return new ViewCell
//                    {
//                        View = new StackLayout
//                        {
//                            Orientation = StackOrientation.Horizontal,
//                            Children = { addressLbl, friendlyLbl }
//                        }
//                    };
//                })
//            };
//            printerLv.ItemSelected += PrinterLv_ItemSelected;

//            Button backBtn = new Button
//            {
//                Text = "Back",
//                HorizontalOptions = LayoutOptions.Start
//            };
//            backBtn.Clicked += BackBtn_Clicked;

//            statusLbl = new Label
//            {
//                Text = "Discovering Printers..."
//            };

//            StackLayout topSection = new StackLayout
//            {
//                VerticalOptions = LayoutOptions.Start,
//                HorizontalOptions = LayoutOptions.FillAndExpand,
//                Orientation = StackOrientation.Horizontal,

//                Children = { backBtn, statusLbl }
//            };

//            this.Content = new StackLayout
//            {
//                Children = { topSection, printerLv }
//            };
//            StartSearch();
//        }

//        private void BackBtn_Clicked(object sender, EventArgs e)
//        {
//            DependencyService.Get<IPrinterDiscovery>().CancelDiscovery();
//            if (OnBackToMainPage != null)
//                OnBackToMainPage();
//        }
//        /// <summary>
//        /// Start discovery on all ports.  USB, then Bluetooth, then Network.
//        /// </summary>
//        private void StartSearch()
//        {
//            new Task(new Action(() => {
//                StartUSBDiscovery();
//            })).Start();
//        }
//        private void StartUSBDiscovery()
//        {
//            OnStatusMessage("Discovering USB Printers");
//            try
//            {
//                IDiscoveryEventHandler usbhandler = DiscoveryHandlerFactory.Current.GetInstance();
//                usbhandler.OnDiscoveryError += DiscoveryHandler_OnDiscoveryError;
//                usbhandler.OnDiscoveryFinished += DiscoveryHandler_OnDiscoveryFinished;
//                usbhandler.OnFoundPrinter += DiscoveryHandler_OnFoundPrinter;
//                connetionType = ConnectionType.USB;
//                System.Diagnostics.Debug.WriteLine("Starting USB Discovery");
//                DependencyService.Get<IPrinterDiscovery>().FindUSBPrinters(usbhandler);
//            }
//            catch (NotImplementedException)
//            {
//                //  USB not availible on iOS, so handle the exeption and move on to Bluetooth discovery
//                StartBluetoothDiscovery();
//            }
//        }

//        private void StartNetworkDiscovery()
//        {
//            OnStatusMessage("Discovering Network Printers");
//            try
//            {
//                IDiscoveryEventHandler nwhandler = DiscoveryHandlerFactory.Current.GetInstance();
//                nwhandler.OnDiscoveryError += DiscoveryHandler_OnDiscoveryError;
//                nwhandler.OnDiscoveryFinished += DiscoveryHandler_OnDiscoveryFinished;
//                nwhandler.OnFoundPrinter += DiscoveryHandler_OnFoundPrinter;
//                connetionType = ConnectionType.Network;
//                System.Diagnostics.Debug.WriteLine("Starting Network Discovery");
//                NetworkDiscoverer.Current.LocalBroadcast(nwhandler);
//            }
//            catch (Exception e)
//            {
//                System.Diagnostics.Debug.WriteLine("Network Exception: " + e.Message);
//            }
//        }

//        private void StartBluetoothDiscovery()
//        {
//            OnStatusMessage("Discovering Bluetooth Printers");
//            IDiscoveryEventHandler bthandler = DiscoveryHandlerFactory.Current.GetInstance();
//            bthandler.OnDiscoveryError += DiscoveryHandler_OnDiscoveryError;
//            bthandler.OnDiscoveryFinished += DiscoveryHandler_OnDiscoveryFinished;
//            bthandler.OnFoundPrinter += DiscoveryHandler_OnFoundPrinter;
//            connetionType = ConnectionType.Bluetooth;
//            System.Diagnostics.Debug.WriteLine("Starting Bluetooth Discovery");
//            DependencyService.Get<IPrinterDiscovery>().FindBluetoothPrinters(bthandler);
//        }

//        private void DiscoveryHandler_OnFoundPrinter(object sender, IDiscoveredPrinter discoveredPrinter)
//        {

//            System.Diagnostics.Debug.WriteLine("Found Printer:" + discoveredPrinter.ToString());
//            Device.BeginInvokeOnMainThread(() => {
//                printerLv.BatchBegin();

//                if (!printerList.Contains(discoveredPrinter))
//                {
//                    printerList.Add(discoveredPrinter);
//                }
//                printerLv.BatchCommit();
//            });
//        }

//        private void DiscoveryHandler_OnDiscoveryFinished(object sender)
//        {
//            System.Diagnostics.Debug.WriteLine("On Discovery Finished:" + connetionType.ToString());

//            if (connetionType == ConnectionType.USB)
//            {
//                StartBluetoothDiscovery();
//            }
//            else if (connetionType == ConnectionType.Bluetooth)
//            {
//                StartNetworkDiscovery();
//            }
//            else
//                OnStatusMessage("Discovery Finished");
//        }

//        private void DiscoveryHandler_OnDiscoveryError(object sender, string message)
//        {
//            System.Diagnostics.Debug.WriteLine("On Discovery Error: " + connetionType.ToString());
//            OnError(message);

//            if (connetionType == ConnectionType.USB)
//            {
//                StartBluetoothDiscovery();
//            }
//            else if (connetionType == ConnectionType.Bluetooth)
//            {
//                StartNetworkDiscovery();
//            }
//            else
//                OnStatusMessage("Discovery Finished");
//        }

//        private void OnError(string message)
//        {
//            Device.BeginInvokeOnMainThread(() =>
//            {
//                DisplayAlert("Error", message, "OK");
//            });
//        }

//        private void OnStatusMessage(string message)
//        {
//            Device.BeginInvokeOnMainThread(() =>
//            {
//                statusLbl.Text = message;
//            });
//        }

//        private void PrinterLv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
//        {
//            DependencyService.Get<IPrinterDiscovery>().CancelDiscovery();
//            if (e.SelectedItem is IDiscoveredPrinterUsb)
//            {
//                if (!((IDiscoveredPrinterUsb)e.SelectedItem).HasPermissionToCommunicate)
//                {
//                    DependencyService.Get<IPrinterDiscovery>().RequestUSBPermission(((IDiscoveredPrinterUsb)e.SelectedItem));
//                }
//            }
//            if (OnPrinterSelected != null)
//                OnPrinterSelected((IDiscoveredPrinter)e.SelectedItem);
//        }
//    }

//}
