using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using PdfSharpCore.Fonts;
using PdfSharpCore.Drawing;
using PdfSharp.Xamarin.Forms.Attributes;

namespace PdfSharp.Xamarin.Forms.Renderers
{
	[PdfRenderer(ViewType = typeof(Entry))]
	public class PdfEntryRenderer : PdfRendererBase<Entry>
	{
		public override void CreatePDFLayout(XGraphics page, Entry entry, XRect bounds, double scaleFactor)
		{
			XFont font = new XFont(entry.FontFamily ?? GlobalFontSettings.FontResolver.DefaultFontName, entry.FontSize * scaleFactor);

			if (entry.BackgroundColor != default(Color))
				page.DrawRectangle(entry.BackgroundColor.ToXBrush(), bounds);

			// Border
			page.DrawRectangle(new XPen(Color.LightGray.ToXColor(), 1.5 * scaleFactor), bounds);

			if (!string.IsNullOrEmpty(entry.Text))
			{
				Color textColor = entry.TextColor != default(Color) ? entry.TextColor : Color.Black;
				page.DrawString(entry.Text, font, textColor.ToXBrush(), bounds.AddMargin(5 * scaleFactor),
					new XStringFormat()
					{
						Alignment = entry.HorizontalTextAlignment.ToXStringAlignment(),
						LineAlignment = entry.HorizontalTextAlignment.ToXLineAlignment()
					});
			}
			else if (!string.IsNullOrEmpty(entry.Placeholder))
			{
				Color placeholderColor = entry.PlaceholderColor != default(Color) ? entry.PlaceholderColor : Color.Gray;
				page.DrawString(entry.Placeholder, font, placeholderColor.ToXBrush(), bounds.AddMargin(5 * scaleFactor),
					new XStringFormat()
					{
						Alignment = entry.HorizontalTextAlignment.ToXStringAlignment(),
						LineAlignment = entry.HorizontalTextAlignment.ToXLineAlignment()
					});
			}
		}
	}
}
