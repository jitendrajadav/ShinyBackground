using System;
using System.Collections.Generic;
using System.Text;

namespace PdfSharp.Xamarin.Forms.Attributes
{
    internal class PdfRendererAttribute : Attribute
    {
        public Type ViewType { get; set; }
    }
}
