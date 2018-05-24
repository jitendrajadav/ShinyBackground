using LinkOS.Plugin.Abstractions;

namespace KegID.DependencyServices
{
    public interface IPrinterDiscovery
    {
        void FindBluetoothPrinters(IDiscoveryHandler handler);
        void FindUSBPrinters(IDiscoveryHandler handler);
        void RequestUSBPermission(IDiscoveredPrinterUsb printer);
        void CancelDiscovery();
    }
}
