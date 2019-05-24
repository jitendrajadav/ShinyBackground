﻿using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using System.ComponentModel;
using UIKit;
using Foundation;
using System;
using CoreGraphics;
using KegID.Common;
using KegID.iOS.Renderers;
using KegID.iOS.Extensions;

[assembly: ExportRenderer(typeof(ExtendedDatePicker), typeof(ExtendedDatePickerRenderer))]
namespace KegID.iOS.Renderers
{
    /// <summary>
    ///  Extended DatePicker Renderer for Nullable Values
    ///  Via: https://forums.xamarin.com/discussion/20028/datepicker-possible-to-bind-to-nullable-date-value
    ///  Via: https://github.com/XLabs/Xamarin-Forms-Labs/wiki/ExtendedEntry
    /// </summary>
    public class ExtendedDatePickerRenderer : DatePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);
            var view = Element as ExtendedDatePicker;

            if (view != null)
            {
                SetFont(view);
                SetTextAlignment(view);
                SetBorder(view);
                SetNullableText(view);
                SetPlaceholderTextColor(view);
                ResizeHeight();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var view = (ExtendedDatePicker)Element;
            if (e.PropertyName == ExtendedDatePicker.FontProperty.PropertyName)
                SetFont(view);
            else if (e.PropertyName == ExtendedDatePicker.XAlignProperty.PropertyName)
                SetTextAlignment(view);
            else if (e.PropertyName == ExtendedDatePicker.HasBorderProperty.PropertyName)
                SetBorder(view);
            else if (e.PropertyName == ExtendedDatePicker.NullableDateProperty.PropertyName)
                SetNullableText(view);
            else if (e.PropertyName == ExtendedDatePicker.PlaceholderTextColorProperty.PropertyName)
                SetPlaceholderTextColor(view);
            ResizeHeight();
        }

        /// <summary>
        /// Sets the text alignment.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetTextAlignment(ExtendedDatePicker view)
        {
            switch (view.XAlign)
            {
                case TextAlignment.Center:
                    Control.TextAlignment = UITextAlignment.Center;
                    break;
                case TextAlignment.End:
                    Control.TextAlignment = UITextAlignment.Right;
                    break;
                case TextAlignment.Start:
                    Control.TextAlignment = UITextAlignment.Left;
                    break;
            }
        }
        
        /// <summary>
        /// Sets the font.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetFont(ExtendedDatePicker view)
        {
            UIFont uiFont;
            if (view.Font != Font.Default && (uiFont = view.Font.ToUIFont()) != null)
                Control.Font = uiFont;
            else if (view.Font == Font.Default)
                Control.Font = UIFont.SystemFontOfSize(17f);
        }
        
        /// <summary>
        /// Sets the border.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetBorder(ExtendedDatePicker view)
        {
            Control.BorderStyle = view.HasBorder ? UITextBorderStyle.RoundedRect : UITextBorderStyle.None;
        }

        /// <summary>
        /// Set text based on nullable value
        /// </summary>
        /// <param name="view"></param>
        private void SetNullableText(ExtendedDatePicker view)
        {
            if (view.NullableDate == null)
                Control.Text = string.Empty;
        }

        /// <summary>
        /// Resizes the height.
        /// </summary>
        private void ResizeHeight()
        {
            if (Element.HeightRequest >= 0) return;
            var height = Math.Max(Bounds.Height,
                new UITextField { Font = Control.Font }.IntrinsicContentSize.Height) * 2;
            Control.Frame = new CGRect(0.0f, 0.0f, (nfloat)Element.Width, (nfloat)height);
            Element.HeightRequest = height;
        }

        /// <summary>
        /// Sets the color of the placeholder text.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetPlaceholderTextColor(ExtendedDatePicker view)
        {
            if (!string.IsNullOrEmpty(view.Placeholder))
            {
                var foregroundUIColor = view.PlaceholderTextColor.ToUIColor(CheckColorExtensions.SeventyPercentGrey);
                var backgroundUIColor = view.BackgroundColor.ToUIColor();
                var targetFont = Control.Font;
                Control.AttributedPlaceholder = new NSAttributedString(view.Placeholder, targetFont, foregroundUIColor, backgroundUIColor);
            }
        }
    }
}