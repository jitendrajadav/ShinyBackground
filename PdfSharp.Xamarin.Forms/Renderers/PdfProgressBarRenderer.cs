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
    [PdfRenderer(ViewType = typeof(ProgressBar))]
    public class PdfProgressBarRenderer : PdfRendererBase<ProgressBar>
    {
        public override void CreatePDFLayout(XGraphics page, ProgressBar progressBar, XRect bounds, double scaleFactor)
        {
            Color bgColor = progressBar.BackgroundColor != default(Color) ? progressBar.BackgroundColor : Color.LightGray;
            Color barColor = Color.FromHex("#189DC4");


            page.DrawRectangle(bgColor.ToXBrush(), bounds);

            XRect progress = new XRect(bounds.X + scaleFactor,
                                       bounds.Y + scaleFactor,
                                       bounds.Width * progressBar.Progress,
                                       bounds.Height - 2 * scaleFactor);


            page.DrawRectangle(bgColor.ToXBrush(), bounds);
            page.DrawRectangle(barColor.ToXBrush(), progress);
        }
    }
}
