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
    [PdfRenderer(ViewType = typeof(BoxView))]
    public class PdfBoxViewRenderer : PdfRendererBase<BoxView>
    {
        public override void CreatePDFLayout(XGraphics page, BoxView box, XRect bounds, double scaleFactor)
        {
            if (box.BackgroundColor != default(Color))
                page.DrawRectangle(box.BackgroundColor.ToXBrush(), bounds);

            if (box.Color != default(Color))
                page.DrawRectangle(box.Color.ToXBrush(), bounds);
        }
    }
}
