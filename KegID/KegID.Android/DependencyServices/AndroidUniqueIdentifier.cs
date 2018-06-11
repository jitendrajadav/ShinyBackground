using KegID.DependencyServices;
using KegID.Droid.DependencyServices;
using KegID.Model;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidUniqueIdentifier))]
namespace KegID.Droid.DependencyServices
{
    public class AndroidUniqueIdentifier : IUniqueIdentifier
    {
        public UniqueIdentifierValue GetUniqueIdentifier()
        {
            UniqueIdentifierValue _UniqueIdentifier = new UniqueIdentifierValue();

            _UniqueIdentifier.Serial = Android.OS.Build.Serial;
            _UniqueIdentifier.Model = Android.OS.Build.Model;
            _UniqueIdentifier.Manufacturer = Android.OS.Build.Manufacturer;
            _UniqueIdentifier.Product = Android.OS.Build.Product;
            _UniqueIdentifier.User = Android.OS.Build.User;
            _UniqueIdentifier.Id = Android.OS.Build.Id;
            _UniqueIdentifier.Device = Android.OS.Build.Device;
            _UniqueIdentifier.Host = Android.OS.Build.Host;
            _UniqueIdentifier.Display = Android.OS.Build.Display;

            return _UniqueIdentifier;
        }

        public string AppVersion()
        {
            var context = Android.App.Application.Context;
            var name = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
            var code = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionCode;

            return string.Format("{0}", name);
        }

        public string AppName()
        {
            var context = Android.App.Application.Context.PackageName;
            return context;
        }

        public string AppPackageCreationDate()
        {
            string date = DateTimeOffset.UtcNow.Date.ToShortDateString();
            return date;
        }
    }
}