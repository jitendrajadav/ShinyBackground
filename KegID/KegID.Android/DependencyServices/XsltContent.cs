using System.IO;
using Android.Content.Res;
using KegID.DependencyServices;
using KegID.Droid.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(XsltContent))]
namespace KegID.Droid.DependencyServices
{
    class XsltContent : IXsltContent
    {
        public string GetXsltContent(string filename)
        {
            string content;
            AssetManager assets =  Forms.Context.Assets;
            using (StreamReader sr = new StreamReader(assets.Open(filename)))
            {
                content = sr.ReadToEnd();
            }
            return content;
        }
    }
}