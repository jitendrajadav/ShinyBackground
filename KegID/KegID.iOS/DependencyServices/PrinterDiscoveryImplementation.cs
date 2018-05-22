//using System;
//using KegID.DependencyServices;
//using LinkOS.Plugin;
//using LinkOS.Plugin.Abstractions;

//namespace KegID.iOS.DependencyServices
//{
//    public class PrinterDiscoveryImplementation : IPrinterDiscovery
//    {
//        public PrinterDiscoveryImplementation() { }

//        public void CancelDiscovery()
//        {
//        }

//        public void FindBluetoothPrinters(IDiscoveryHandler handler)
//        {
//            BluetoothDiscoverer.Current.FindPrinters(null, handler);
//        }

//        public void FindUSBPrinters(IDiscoveryHandler handler)
//        {
//            throw new NotImplementedException();
//        }

//        public void RequestUSBPermission(IDiscoveredPrinterUsb printer)
//        {
//            throw new NotImplementedException();
//        }
//    }

//}