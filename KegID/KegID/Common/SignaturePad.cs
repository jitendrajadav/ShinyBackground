using System;
using System.IO;
using Xamarin.Forms;

namespace KegID.Common
{
    public class SignaturePad : BoxView
    {
        public static readonly Color DefaultBackgroundColor = Color.LightGray;
        public static readonly Color DefaultStrokeColor = Color.Black;
        public static readonly float DefaultStrokeWidth = 3.0f;

        public event EventHandler<ImageStreamRequestedEventArgs> ImageStreamRequested;

        public class ImageStreamRequestedEventArgs : EventArgs
        {
            public double PrintWidth { get; set; }
            public Stream ImageStream { get; set; }
        }

        public Stream GetImageStream(double printWidth)
        {
            var args = new ImageStreamRequestedEventArgs
            {
                PrintWidth = printWidth
            };

            ImageStreamRequested?.Invoke(this, args);
            return args.ImageStream;
        }
    }
}
