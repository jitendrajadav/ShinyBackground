﻿using System;
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
    [PdfRenderer(ViewType = typeof(Picker))]
    public class PdfPickerRenderer : PdfRendererBase<Picker>
    {
        public override void CreatePDFLayout(XGraphics page, Picker picker, XRect bounds, double scaleFactor)
        {
            if (picker.SelectedItem != null)
            {
              
				XFont font = new XFont(GlobalFontSettings.FontResolver.DefaultFontName, 14 * scaleFactor);

				Color textColor = picker.TextColor != default(Color) ? picker.TextColor : Color.Black;

				page.DrawRectangle(new XPen(Color.LightGray.ToXColor(), 2 * scaleFactor), bounds);

				page.DrawString(picker.SelectedItem.ToString(), font, textColor.ToXBrush(), bounds, new XStringFormat
				{
					Alignment = XStringAlignment.Center,
					LineAlignment = XLineAlignment.Center,
				});
			}
        }
    }
}
