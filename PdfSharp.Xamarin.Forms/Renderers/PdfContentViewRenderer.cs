using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using PdfSharpCore.Drawing;
using PdfSharp.Xamarin.Forms.Attributes;

namespace PdfSharp.Xamarin.Forms.Renderers
{
    [PdfRenderer(ViewType = typeof(ContentView))]
    public class PdfContentViewRenderer : PdfRendererBase<ContentView>
    {
        public override void CreatePDFLayout(XGraphics page, ContentView view, XRect bounds, double scaleFactor)
        {
            if (view.BackgroundColor != null)
                page.DrawRectangle(view.BackgroundColor.ToXBrush(), bounds);
        }
    }
}
