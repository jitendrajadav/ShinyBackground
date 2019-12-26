using System;
using Xamarin.Forms.Platform.Android;
using Android.App;
using Android.Content;
using Android.Widget;
using KegID.Common;
using Xamarin.Forms;
using KegID.Droid.Renderers;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(NullableDatePicker), typeof(NullableDatePickerRenderer))]
namespace KegID.Droid.Renderers
{
    public class NullableDatePickerRenderer : ViewRenderer<NullableDatePicker, EditText>
    {
        DatePickerDialog _dialog;

        public NullableDatePickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<NullableDatePicker> e)
        {
            base.OnElementChanged(e);

            SetNativeControl(new EditText(Context));
            if (Control == null || e.NewElement == null)
                return;

            var entry = Element;

            Control.Click += OnPickerClick;
            Control.Text = !entry.NullableDate.HasValue ? entry.PlaceHolder : Element.Date.ToString(Element.Format);
            Control.KeyListener = null;
            Control.FocusChange += OnPickerFocusChange;
            Control.Enabled = Element.IsEnabled;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Xamarin.Forms.DatePicker.DateProperty.PropertyName || e.PropertyName == Xamarin.Forms.DatePicker.FormatProperty.PropertyName)
            {
                var entry = Element;

                if (Element.Format == entry.PlaceHolder)
                {
                    Control.Text = entry.PlaceHolder;
                    return;
                }
            }

            base.OnElementPropertyChanged(sender, e);
        }

        private void OnPickerFocusChange(object sender, FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {
                ShowDatePicker();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (Control != null)
            {
                Control.Click -= OnPickerClick;
                Control.FocusChange -= OnPickerFocusChange;

                if (_dialog != null)
                {
                    _dialog.Hide();
                    _dialog.Dispose();
                    _dialog = null;
                }
            }

            base.Dispose(disposing);
        }

        private void OnPickerClick(object sender, EventArgs e)
        {
            ShowDatePicker();
        }

        private void SetDate(DateTimeOffset date)
        {
            Control.Text = date.ToString(Element.Format);
            Element.Date = date.Date;
        }

        private void ShowDatePicker()
        {
            CreateDatePickerDialog(Element.Date.Year, Element.Date.Month - 1, Element.Date.Day);
            _dialog.Show();
        }

        private void CreateDatePickerDialog(int year, int month, int day)
        {
            NullableDatePicker view = Element;
            _dialog = new DatePickerDialog(Context, (o, e) =>
            {
                view.Date = e.Date;
                ((IElementController)view).SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
                Control.ClearFocus();

                _dialog = null;
            }, year, month, day);

            _dialog.SetButton("Done", (sender, e) =>
            {
                Element.Format = Element._originalFormat;
                SetDate(_dialog.DatePicker.DateTime);
                Element.AssignValue();
            });
            _dialog.SetButton2("Clear", (sender, e) =>
            {
                Element.CleanDate();
                Control.Text = Element.Format;
            });
        }
    }
}