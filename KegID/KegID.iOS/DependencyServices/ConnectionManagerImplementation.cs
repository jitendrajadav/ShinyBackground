using KegID.DependencyServices;
using KegID.iOS.DependencyServices;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer.Discovery;

[assembly: Dependency(typeof(ConnectionManagerImplementation))]
namespace KegID.iOS.DependencyServices
{
    public class ConnectionManagerImplementation : IConnectionManager
    {

        public string BuildBluetoothConnectionChannelsString(string macAddress)
        {
            throw new NotImplementedException();
        }

        public void FindBluetoothPrinters(DiscoveryHandler discoveryHandler)
        {
            BluetoothDiscoverer.FindPrinters(discoveryHandler);
        }

        public Connection GetBluetoothConnection(string macAddress)
        {
            return new BluetoothConnection(macAddress);
        }

        public StatusConnection GetBluetoothStatusConnection(string macAddress)
        {
            throw new NotImplementedException();
        }

        public MultichannelConnection GetMultichannelBluetoothConnection(string macAddress)
        {
            throw new NotImplementedException();
        }

        public Connection GetUsbConnection(string symbolicName)
        {
            throw new NotImplementedException();
        }

        public void GetZebraUsbDirectPrinters(DiscoveryHandler discoveryHandler)
        {
            throw new NotImplementedException();
        }

        public List<DiscoveredPrinter> GetZebraUsbDriverPrinters()
        {
            throw new NotImplementedException();
        }
    }
}