using System.IO;

using Foundation;
using KegID.DependencyServices;
using KegID.iOS.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(XsltContent))]
namespace KegID.iOS.DependencyServices
{
    class XsltContent : IXsltContent
    {
        public string GetXsltContent(string filename)
        {
            var dataPath = Path.Combine(NSBundle.MainBundle.BundlePath);
            var dataFileName = Path.Combine(dataPath, filename);

            string content;
            using (StreamReader sr = new StreamReader(dataFileName))
            {
                content = sr.ReadToEnd();
            }
            return content;
        }
    }
}