using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
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