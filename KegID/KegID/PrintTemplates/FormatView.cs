﻿using LinkOS.Plugin;
using LinkOS.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KegID.PrintTemplates
{
    public class FormatView : BaseDemoView
    {
        private List<IFieldDescription> fields;
        private Format _format;
        ActivityIndicator spinner;
        Label title;
        Button printBtn;

        public FormatView() : base()
        {
            title = new Label { Text = "No Format Selected" };
            spinner = new ActivityIndicator { IsRunning = false };

            ClearFormat();
        }

        public void SetFormat(Format format)
        {
            _format = format;
            title = new Label { Text = "Format Demo :" + format.Path };
            ClearFormat();
            GetAndShowFields();
        }

        private void ClearFormat()
        {
            Children.Clear();
            Children.Add(title);
            Children.Add(spinner);
        }

        private void GetAndShowFields()
        {
            spinner.IsRunning = true;
            new Task(new Action(() =>
            {
                fields = new List<IFieldDescription>();
                if (_format.FileLocation == Format.Location.device)
                    fields.AddRange(GetFieldsFromDevice());
                else fields.AddRange(GetFieldsFromPrinter());
                ShowFields();
            })).Start();
        }
        private void ShowFields()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                foreach (IFieldDescription fieldDescription in fields)
                {
                    ShowAField(fieldDescription);
                }

                printBtn = new Button { Text = "Print" };
                printBtn.Clicked += PrintBtn_Clicked;
                Children.Add(printBtn);
                resetPage();
            });
        }

        private void ShowAField(IFieldDescription fieldDescription)
        {
            Label fieldLbl = new Label { Text = "Field " + fieldDescription.FieldNumber };
            if (fieldDescription.FieldName != null)
                fieldLbl.Text = fieldDescription.FieldName;
            Entry fieldEntry = new Entry { HorizontalOptions = LayoutOptions.FillAndExpand };
            fieldEntry.ClassId = fieldDescription.FieldNumber.ToString();
            StackLayout formCell = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Horizontal,
                Children = { fieldLbl, fieldEntry }
            };
            Children.Add(formCell);
        }

        private void PrintBtn_Clicked(object sender, EventArgs e)
        {
            printBtn.IsEnabled = false;
            spinner.IsRunning = true;

            new Task(new Action(() =>
            {
                PrintFormat();
            })).Start();
        }

        private void PrintFormat()
        {
            CheckPrinter();
            IConnection connection = myPrinter.Connection;
            try
            {
                connection.Open();
                IZebraPrinter printer = ZebraPrinterFactory.Current.GetInstance(connection);

                if ((!CheckPrinterLanguage(connection)) || (!PreCheckPrinterStatus(printer)))
                {
                    resetPage();
                    return;
                }

                Dictionary<int, string> vars = new Dictionary<int, string>();

                foreach (View item in Children)
                {
                    if (item is StackLayout)
                    {
                        foreach (View formItem in ((StackLayout)item).Children)
                            if (formItem is Entry)
                                vars.Add(Convert.ToInt32(formItem.ClassId), ((Entry)formItem).Text);
                    }
                }

                // Send a format that is on the device to the printer
                if (_format.FileLocation == Format.Location.device)
                    printer.SendFileContents(_format.Path);

                // Send the vairable fields
                printer.PrintStoredFormat(_format.PrinterPath, vars);

                PostPrintCheckStatus(printer);
            }
            catch (Exception e)
            {
                // Connection issues are thrown as Exceptions
                System.Diagnostics.Debug.WriteLine("Exception:" + e.Message);
                ShowErrorAlert(e.Message);
            }
            finally
            {
                if ((connection != null) && (connection.IsConnected))
                    connection.Close();
                resetPage();
            }
        }

        private void resetPage()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                printBtn.IsEnabled = true;
                spinner.IsRunning = false;
            });
        }

        private IFieldDescription[] GetFieldsFromDevice()
        {
            IConnection connection = myPrinter.Connection;
            IFieldDescription[] fields = null;
            try
            {
                IZebraPrinter printer = ZebraPrinterFactory.Current.GetInstance(PrinterLanguage.ZPL, connection);
                string formatstring = DependencyService.Get<DependencyServices.IFileUtil>().ReadAllText(_format.Path);
                fields = printer.GetVariableFields(formatstring);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception:" + e.Message);
                ShowErrorAlert("GetFieldsFromDevice:" + e.Message);
                resetPage();
            }
            return fields;
        }
        private IFieldDescription[] GetFieldsFromPrinter()
        {
            IConnection connection = myPrinter.Connection;
            IFieldDescription[] fields = new IFieldDescription[0];
            try
            {
                connection.Open();
                CheckPrinterLanguage(connection);
                IZebraPrinter printer = ZebraPrinterFactory.Current.GetInstance(connection);
                byte[] rawformat = printer.RetrieveFormatFromPrinter(_format.Path);
                string formatstring = Encoding.UTF8.GetString(rawformat, 0, rawformat.Length);
                fields = printer.GetVariableFields(formatstring);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception:" + e.Message);
                ShowErrorAlert("GetFieldsFromPrinter" + e.Message);
            }
            finally
            {
                if ((connection != null) && (connection.IsConnected))
                    connection.Close();
            }
            return fields;
        }
    }
}
