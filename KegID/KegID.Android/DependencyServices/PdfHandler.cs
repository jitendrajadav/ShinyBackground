using KegID.Droid.DependencyServices;
using KegID.Droid.Renderers;
using MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes;
using PdfSharp.Xamarin.Forms.Contracts;

[assembly: Xamarin.Forms.Dependency(typeof(PdfHandler))]
namespace KegID.Droid.DependencyServices
{
    class PdfHandler : IPDFHandler
    {
        public ImageSource GetImageSource()
        {
            return new AndroidImageSource();
        }
    }
}