using LinkOS.Plugin;
using LinkOS.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KegID.PrintTemplates
{
    public class FormatDemoView : BaseDemoView
    {
        public delegate void FormatSelectedHandler(Format fileName);
        public event FormatSelectedHandler OnFormatSelected;

        ObservableCollection<Format> formatList;
        Label title;
        ActivityIndicator spinner;
        Button refreshBtn;
        ListView formatLv;

        public FormatDemoView() : base()
        {
            formatList = new ObservableCollection<Format>();
            title = new Label { Text = "Format Demo" };
            refreshBtn = new Button { Text = "Get Formats" };
            refreshBtn.Clicked += RefreshBtn_Clicked;
            spinner = new ActivityIndicator { IsRunning = false };

            formatLv = new ListView
            {
                ItemsSource = formatList
            };
            formatLv.ItemSelected += FormatLv_ItemSelected;

            Children.Add(title);
            Children.Add(refreshBtn);
            Children.Add(spinner);
            Children.Add(formatLv);
            //GetTemplates();
        }

        private void RefreshBtn_Clicked(object sender, EventArgs e)
        {
            if (CheckPrinter())
            {
                title.Text = "Format Demo";
                refreshBtn.IsEnabled = false;
                spinner.IsRunning = true;
                formatLv.IsEnabled = false;
                GetTemplates();
            }
        }

        private void GetTemplates()
        {
            formatList.Clear();
            new Task(new Action(() =>
            {
                StartGetTemplatesFromDevice();
                StartGetTemplatesFromPrinter();
            })).Start();
        }

        //modified from https://github.com/Zebra/LinkOS-Xamarin-Samples/blob/ZSDK_DevDemos/ZSDK_DevDemos/Xamarin_LinkOS_Developer_Demo/Xamarin_LinkOS_Developer_Demo/FormatDemoView.cs  
        private void StartGetTemplatesFromPrinterFMT()
        {
            //CheckPrinter();  
            IConnection connection = myPrinter.Connection;
            try
            {
                connection.MaxTimeoutForRead = 5000;
                connection.TimeToWaitForMoreData = 1000;
                connection.Open();


                if (!CheckPrinterLanguage(connection))
                {
                    ResetPage();
                    return;
                }
                IZebraPrinter printer = ZebraPrinterFactory.Current.GetInstance(PrinterLanguage.CPCL, connection);
                string[] fileNames = printer.RetrieveFileNames(new string[] { "FMT" });
            }
            catch (Exception e)
            {
                ShowErrorAlert(e.Message + " - On printer");
            }
            finally
            {
                if ((connection != null) && (connection.IsConnected))
                    connection.Close();
                ResetPage();
            }
        }

        //modified from https://github.com/Zebra/LinkOS-Xamarin-Samples/blob/ZSDK_DevDemos/ZSDK_DevDemos/Xamarin_LinkOS_Developer_Demo/Xamarin_LinkOS_Developer_Demo/FormatView.cs  
        private IFieldDescription[] PrintFormatOnPrinter(string fileNameOnPrinter, string[] mydata)
        {
            IConnection connection = myPrinter.Connection;
            IFieldDescription[] fields = new IFieldDescription[0];
            try
            {
                connection.Open();
                CheckPrinterLanguage(connection);
                IZebraPrinter printer = ZebraPrinterFactory.Current.GetInstance(connection);


                byte[] rawformat = printer.RetrieveFormatFromPrinter(fileNameOnPrinter);
                string formatstring = Encoding.UTF8.GetString(rawformat, 0, rawformat.Length);
                fields = printer.GetVariableFields(formatstring);


                Dictionary<int, string> vars = new Dictionary<int, string>();
                int i = 0;
                foreach (IFieldDescription field in fields)
                {
                    vars.Add(field.FieldNumber, mydata[i]);
                    i++;
                }
                printer.PrintStoredFormat(fileNameOnPrinter, vars);
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

        private void StartGetTemplatesFromPrinter()
        {
            //CheckPrinter();
            IConnection connection = myPrinter.Connection;
            try
            {
                connection.MaxTimeoutForRead = 5000;
                connection.TimeToWaitForMoreData = 1000;
                connection.Open();

                if (!CheckPrinterLanguage(connection))
                {
                    ResetPage();
                    return;
                }
                IZebraPrinter printer = ZebraPrinterFactory.Current.GetInstance(connection);
                string[] fileNames = printer.RetrieveFileNames(new string[] { "ZPL" });
                AddFormatsToList(fileNames, Format.Location.printer);
            }
            catch (Exception e)
            {
                ShowErrorAlert(e.Message + " - On printer");
            }
            finally
            {
                if ((connection != null) && (connection.IsConnected))
                    connection.Close();
                ResetPage();
            }
        }
        private void AddFormatsToList(string[] fileNames, Format.Location location)
        {
            if (fileNames == null)
                return;
            Device.BeginInvokeOnMainThread(() =>
            {
                foreach (string file in fileNames)
                {
                    Format fmt = new Format { Path = file, FileLocation = location };
                    formatList.Add(fmt);
                }
            });
        }

        private void ResetPage()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                spinner.IsRunning = false;
                refreshBtn.IsEnabled = true;
                formatLv.IsEnabled = true;
                title.Text = "Format Demo - " + formatList.Count + " formats found";
                if (0 == formatList.Count)
                {
                    ShowAlert("No Formats Found.", "Notification");
                }
            });
        }

        private void StartGetTemplatesFromDevice()
        {
            string[] fileList = DependencyService.Get<DependencyServices.IFileUtil>().GetFiles("ZPL");
            AddFormatsToList(fileList, Format.Location.device);
        }

        private void FormatLv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Format)
            {
                OnFormatSelected?.Invoke((Format)e.SelectedItem);

                formatLv.SelectedItem = null;
            }
        }
    }

    public class Format
    {
        public string Path { get; set; }
        public enum Location
        {
            device,
            printer
        }
        public Location FileLocation { get; set; }
        public override string ToString()
        {
            return Path;
        }
        public string PrinterPath { get { return Path.Substring(Path.LastIndexOf('/') + 1); } }
    }

}
