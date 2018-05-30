using KegID.iOS.DependencyServices;
using KegID.iOS.Renderers;
using MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes;
using PdfSharp.Xamarin.Forms.Contracts;

[assembly: Xamarin.Forms.Dependency(typeof(PdfHandler))]
namespace KegID.iOS.DependencyServices
{
    class PdfHandler : IPDFHandler
    {
        public ImageSource GetImageSource()
        {
            return new IosImageSource();
        }
    }
}