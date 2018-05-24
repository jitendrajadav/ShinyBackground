using LinkOS.Plugin;
using LinkOS.Plugin.Abstractions;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KegID.PrintTemplates
{
    public class StatusDemoView : BaseDemoView
    {
        Label printerStatusLbl;
        Label causesLbl;
        Button refreshBtn;

        public StatusDemoView() : base()
        {
            printerStatusLbl = new Label { Text = "Printer Status:" };
            causesLbl = new Label { Text = "" };
            refreshBtn = new Button { Text = "Check Status" };
            refreshBtn.Clicked += RefreshBtn_Clicked;
            refreshBtn.IsEnabled = true;

            Children.Add(refreshBtn);
            Children.Add(printerStatusLbl);
            Children.Add(causesLbl);
        }

        private void CheckStatus()
        {
            new Task(new Action(() =>
            {
                GetPrinterStatus();
            })).Start();
        }

        private void GetPrinterStatus()
        {
            //CheckPrinter();
            IConnection connection = null;
            try
            {
                connection = myPrinter.Connection;
                connection.Open();
                if (!CheckPrinterLanguage(connection))
                {
                    resetPage();
                    return;
                }
                IZebraPrinter printer = ZebraPrinterFactory.Current.GetInstance(connection);
                IPrinterStatus status = printer.CurrentStatus;
                ShowStatus(status);
            }
            catch (Exception e)
            {
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

        private void ShowStatus(IPrinterStatus status)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (status.IsReadyToPrint)
                {

                    printerStatusLbl.Text = "Printer Status: Printer Ready";
                    printerStatusLbl.TextColor = Color.Green;
                    causesLbl.Text = "";
                }
                else
                {
                    printerStatusLbl.Text = "Printer Status: Printer Error";
                    printerStatusLbl.TextColor = Color.Red;
                    causesLbl.Text = status.Status;
                }
            });
        }

        private void ClearStatus()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                printerStatusLbl.Text = "Printer Status: ";
                printerStatusLbl.TextColor = Color.Default;
                causesLbl.Text = "";
            });
        }

        private void RefreshBtn_Clicked(object sender, EventArgs e)
        {
            ClearStatus();
            if (CheckPrinter())
            {
                refreshBtn.IsEnabled = false;
                CheckStatus();
            }
        }
        private void resetPage()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                refreshBtn.IsEnabled = true;
            });
        }
    }

}
