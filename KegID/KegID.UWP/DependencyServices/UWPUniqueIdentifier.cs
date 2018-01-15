using KegID.DependencyServices;
using KegID.Model;
using KegID.UWP.DependencyServices;
using System;
using Windows.ApplicationModel;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.System.Profile;
using Xamarin.Forms;

[assembly: Dependency(typeof(UWPUniqueIdentifier))]
namespace KegID.UWP.DependencyServices
{
    public sealed class UWPUniqueIdentifier : IUniqueIdentifier
    {
        private static string GetId()
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.System.Profile.HardwareIdentification"))
            {
                var token = HardwareIdentification.GetPackageSpecificToken(null);
                var hardwareId = token.Id;
                var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(hardwareId);

                byte[] bytes = new byte[hardwareId.Length];
                dataReader.ReadBytes(bytes);

                return BitConverter.ToString(bytes).Replace("-", "");
            }

            throw new Exception("NO API FOR DEVICE ID PRESENT!");
        }

        public UniqueIdentifierValue GetUniqueIdentifier()
        {
            UniqueIdentifierValue _uniqueidentifiervalue = new UniqueIdentifierValue();

            var deviceInformation = new EasClientDeviceInformation();
            _uniqueidentifiervalue.Id = GetId();
            _uniqueidentifiervalue.Model = deviceInformation.SystemProductName;
            _uniqueidentifiervalue.Manufacturer = deviceInformation.SystemManufacturer;
            _uniqueidentifiervalue.Name = deviceInformation.FriendlyName;
            _uniqueidentifiervalue.OSName = deviceInformation.OperatingSystem;

            return _uniqueidentifiervalue;
        }


        public string AppVersion()
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;
            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }

        public string AppName()
        {
            string name = Package.Current.DisplayName;
            name = name.Remove(name.Length - 4);
            return name;
        }

        public string AppPackageCreationDate()
        {
            string date = DateTime.Today.ToShortDateString();
            return date;
        }
    }
}
