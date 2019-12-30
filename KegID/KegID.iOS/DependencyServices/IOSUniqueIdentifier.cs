//using Foundation;
//using KegID.DependencyServices;
//using KegID.iOS.DependencyServices;
//using KegID.Model;
//using System;

//[assembly: Xamarin.Forms.Dependency(typeof(IOSUniqueIdentifier))]
//namespace KegID.iOS.DependencyServices
//{
//    public class IOSUniqueIdentifier : IUniqueIdentifier
//    {
//        public string AppVersion()
//        {
//            var context = NSBundle.MainBundle.InfoDictionary[new NSString("CFBundleVersion")].ToString();
//            return context;
//        }

//        public string AppName()
//        {
//            string name = NSBundle.MainBundle.BundleIdentifier;
//            return name;
//        }

//        public string AppPackageCreationDate()
//        {
//            string date = DateTimeOffset.Now.Date.ToShortDateString();
//            return date;
//        }

//        public UniqueIdentifierValue GetUniqueIdentifier()
//        {
//            UniqueIdentifierValue _UniqueIdentifier = new UniqueIdentifierValue
//            {
//                //Serial = NSBundle.MainBundle.Serial,
//                //Model = NSBundle.MainBundle.Model,
//                //Manufacturer = NSBundle.MainBundle.Manufacturer,
//                //Product = NSBundle.MainBundle.Product,
//                //User = NSBundle.MainBundle.User,
//                //Id = NSBundle.MainBundle.Id,
//                //Device = NSBundle.MainBundle.Device,
//                //Host = NSBundle.MainBundle.Host,
//                //Display = NSBundle.MainBundle.Display
//            };

//            return _UniqueIdentifier;
//        }
//    }

//}
