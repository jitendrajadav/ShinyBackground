using System;
using KegID.DependencyServices;
using KegID.iOS.DependencyServices;
using LinkOS.Plugin;
using LinkOS.Plugin.Abstractions;

[assembly: Xamarin.Forms.Dependency(typeof(PrinterDiscoveryImplementation))]
namespace KegID.iOS.DependencyServices
{
    public class PrinterDiscoveryImplementation : IPrinterDiscovery
    {
        public PrinterDiscoveryImplementation() { }

        public void CancelDiscovery()
        {
        }

        public void FindBluetoothPrinters(IDiscoveryHandler handler)
        {
            try
            {
                BluetoothDiscoverer.Current.FindPrinters(null, handler);
            }
            catch (Exception)
            {

            }
        }

        public void FindUSBPrinters(IDiscoveryHandler handler)
        {
            throw new NotImplementedException();
        }

        public void RequestUSBPermission(IDiscoveredPrinterUsb printer)
        {
            throw new NotImplementedException();
        }
    }

}