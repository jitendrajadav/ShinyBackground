using Foundation;
using KegID.DependencyServices;
using KegID.iOS.DependencyServices;
using KegID.Model;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(IOSUniqueIdentifier))]
namespace KegID.iOS.DependencyServices
{
    public class IOSUniqueIdentifier : IUniqueIdentifier
    {
        public string AppVersion()
        {
            var context = NSBundle.MainBundle.InfoDictionary[new NSString("CFBundleVersion")].ToString();
            return context;
        }

        public string AppName()
        {
            string name = NSBundle.MainBundle.BundleIdentifier;
            return name;
        }

        public string AppPackageCreationDate()
        {
            string date = DateTimeOffset.Now.Date.ToShortDateString();
            return date;
        }

        public UniqueIdentifierValue GetUniqueIdentifier()
        {
            throw new NotImplementedException();
        }
    }

}
