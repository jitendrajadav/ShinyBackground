using System;
using System.Collections.Generic;
using System.Text;

namespace PdfSharp.Xamarin.Forms.Contracts
{
    public interface ICustomFontProvider
    {
        byte[] GetFont(string faceName);

        string ProvideFont(string fontName, bool isItalic, bool isBold);
    }
}
