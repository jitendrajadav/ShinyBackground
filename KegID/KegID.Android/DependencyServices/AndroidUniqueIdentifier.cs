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
            UniqueIdentifierValue _UniqueIdentifier = new UniqueIdentifierValue
            {
                Serial = Android.OS.Build.Serial,
                Model = Android.OS.Build.Model,
                Manufacturer = Android.OS.Build.Manufacturer,
                Product = Android.OS.Build.Product,
                User = Android.OS.Build.User,
                Id = Android.OS.Build.Id,
                Device = Android.OS.Build.Device,
                Host = Android.OS.Build.Host,
                Display = Android.OS.Build.Display
            };

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