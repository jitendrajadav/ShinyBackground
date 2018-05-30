using PdfSharpCore.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PdfSharp.Xamarin.Forms
{
	public static class PointExtension
	{
		public static XPoint ToXPoint(this Point point)
		{
			return new XPoint(point.X, point.Y);
		}
	}
}
