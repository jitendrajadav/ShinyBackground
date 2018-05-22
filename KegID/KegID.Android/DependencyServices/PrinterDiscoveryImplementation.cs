//using Android;
//using Android.Bluetooth;
//using Android.Content;
//using Android.Content.PM;
//using Android.Support.V4.App;
//using Android.Support.V4.Content;
//using KegID.DependencyServices;
//using KegID.Droid.DependencyServices;
//using LinkOS.Plugin;
//using LinkOS.Plugin.Abstractions;

//[assembly: Xamarin.Forms.Dependency(typeof(PrinterDiscoveryImplementation))]
//namespace KegID.Droid.DependencyServices
//{
//    public class PrinterDiscoveryImplementation : IPrinterDiscovery
//    {
//        Context context = Android.App.Application.Context;
//        public PrinterDiscoveryImplementation()
//        {

//        }
//        public void CancelDiscovery()
//        {
//            if (BluetoothAdapter.DefaultAdapter.IsDiscovering)
//            {
//                BluetoothAdapter.DefaultAdapter.CancelDiscovery();
//                System.Diagnostics.Debug.WriteLine("Cancelling Discovery");
//            }
//        }

//        public void FindBluetoothPrinters(IDiscoveryHandler handler)
//        {
//            const string permission = Manifest.Permission.AccessCoarseLocation;
//            if (ContextCompat.CheckSelfPermission(context, permission) == Permission.Granted)
//            {
//                BluetoothDiscoverer.Current.FindPrinters(context, handler);
//                return;
//            }
//            TempHandler = handler;
//            //Finally request permissions with the list of permissions and Id
//            ActivityCompat.RequestPermissions(MainActivity.GetActivity(), PermissionsLocation, RequestLocationId);
//        }
//        public static IDiscoveryHandler TempHandler { get; set; }

//        public readonly string[] PermissionsLocation =
//        {
//          Manifest.Permission.AccessCoarseLocation
//        };
//        public const int RequestLocationId = 0;

//        public void FindUSBPrinters(IDiscoveryHandler handler)
//        {
//            UsbDiscoverer.Current.FindPrinters(context, handler);
//        }

//        public void RequestUSBPermission(IDiscoveredPrinterUsb printer)
//        {
//            if (!printer.HasPermissionToCommunicate)
//            {
//                printer.RequestPermission(context);
//            }
//        }
//    }

//}