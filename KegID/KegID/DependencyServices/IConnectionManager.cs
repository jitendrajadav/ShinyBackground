using System.Collections.Generic;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer.Discovery;

namespace KegID.DependencyServices
{
    public interface IConnectionManager
    {
        string BuildBluetoothConnectionChannelsString(string macAddress);

        void FindBluetoothPrinters(DiscoveryHandler discoveryHandler);

        Connection GetBluetoothConnection(string macAddress);

        StatusConnection GetBluetoothStatusConnection(string macAddress);

        MultichannelConnection GetMultichannelBluetoothConnection(string macAddress);

        Connection GetUsbConnection(string symbolicName);

        void GetZebraUsbDirectPrinters(DiscoveryHandler discoveryHandler);

        List<DiscoveredPrinter> GetZebraUsbDriverPrinters();
    }
}
