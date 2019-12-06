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
            var context = global::Android.App.Application.Context;
            var name = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
            var code = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionCode;

            return new UniqueIdentifierValue
            {
                Serial = global::Android.OS.Build.Serial,
                Model = global::Android.OS.Build.Model,
                Manufacturer = global::Android.OS.Build.Manufacturer,
                Product = global::Android.OS.Build.Product,
                User = global::Android.OS.Build.User,
                Id = global::Android.OS.Build.Id,
                Device = global::Android.OS.Build.Device,
                Host = global::Android.OS.Build.Host,
                Display = global::Android.OS.Build.Display,
                AppVersion = name,
                OS = global::Android.OS.Build.Board
            };
        }

        public string AppVersion()
        {
            var context = global::Android.App.Application.Context;
            var name = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
            var code = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionCode;

            return string.Format("{0}", name);
        }

        public string AppName()
        {
            var context = global::Android.App.Application.Context.PackageName;
            return context;
        }

        public string AppPackageCreationDate()
        {
            string date = DateTimeOffset.UtcNow.Date.ToShortDateString();
            return date;
        }
    }
}