using Microsoft.AppCenter.Crashes;
using Prism.Services;
using System;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer;
using Zebra.Sdk.Printer.Discovery;

namespace KegID.Services
{
    public class ZebraPrinterManager: IZebraPrinterManager
    {
        public enum ConnectionType
        {
            Network,
            Bluetooth,
            UsbDirect,
            UsbDriver
        }

        private readonly IPageDialogService _dialogService;
        public static DiscoveredPrinter myPrinter;

        public string TestPrint { get; set; } = @"^XA
                                              ^FO17,16
                                              ^GB379,371,8^FS
                                              ^FT65,255
                                              ^A0N,135,134
                                              ^FDTEST^FS
                                              ^XZ";

        #region ZPL Printer fill pallet format

        /*
                This routine is provided to you as an example of how to create a variable length label with user specified data.
                The basic flow of the example is as follows

                   Header of the label with some variable data
                   Body of the label
                       Loops thru user content and creates small line items of printed material
                   Footer of the label

                As you can see, there are some variables that the user provides in the header, body and footer, and this routine uses that to build up a proper ZPL string for printing.
                Using this same concept, you can create one label for your receipt header, one for the body and one for the footer. The body receipt will be duplicated as many items as there are in your variable data

                */

        public string PalletHeader { get; set; } =
                            /*
                             Some basics of ZPL. Find more information here : http://www.zebra.com

                             ^XA indicates the beginning of a label
                             ^PW sets the width of the label (in dots)
                             ^MNN sets the printer in continuous mode (variable length receipts only make sense with variably sized labels)
                             ^LL sets the length of the label (we calculate this value at the end of the routine)
                             ^LH sets the reference axis for printing.
                                You will notice we change this positioning of the 'Y' axis (length) as we build up the label. Once the positioning is changed, all new fields drawn on the label are rendered as if '0' is the new home position
                             ^FO sets the origin of the field relative to Label Home ^LH
                             ^A sets font information
                             ^FD is a field description
                             ^GB is graphic boxes (or lines)
                             ^B sets barcode information
                             ^XZ indicates the end of a label
                             */

                            "^XA" +
                            "^FX--the box--^FS" +
                            "^FO25,25" +
                            "^GB775,1175,4,B,0 ^FS" +
                            "^FX--the kegid text top left-- ^FS" +
                            "^FO50,50" +
                            "^AVB,40,40" +
                            "^FDKegID ^FS" +
                            "^FX--the pallet num-- ^FS" +
                            "^FO200,50" +
                            "^A0N,56" +
                            "^FD{0}^FS" + //@@PALLETBARCODE@@
                            "^FX--ownername, top right--^FS" +
                            "^FO775,110,1" +
                            "^A0N,42" +
                            "^FD{1}^FS" + //@@OWNERNAME@@
                            "^FX--owner address--^FS" +
                            "^FO775,160,1" +
                            "^A0N,32" +
                            "^FD{2}^FS" +   //@@OWNERADDRESS@@
                            "^FO775,195,1" +
                            "^A0N,32" +
                            "^FD{3}^FS" +   //@@OWNERCSZ@@
                            "^FO775,225,1" +
                            "^A0N,32" +
                            "^FD{4}^FS" +   //@@OWNERPHONE@@
                            "^FX--a line under the owner-- ^FS" +
                            "^FO25,275" +
                            "^GB750,1,4,B,1 ^FS" +
                            "^FX--build location^FS" +
                            "^FO50,300,0" +
                            "^ADN,16" +
                            "^FDBUILD LOCATION ^FS" +
                            "^FO75,325,0" +
                            "^A0N,32" +
                            "^FD{5}^FS" +   //@@STOCKNAME@@
                            "^FX--batch ^FS" +
                            "^FO50,375,0" +
                            "^ADN,16" +
                            "^FDBATCH ^FS" +
                            "^FO75,400,0" +
                            "^A0N,32" +
                            "^FD{6}^FS" +   //@@BATCHNUM@@
                            "^FX--build date^FS" +
                            "^FO350,375,0" +
                            "^ADN,16" +
                            "^FDBUILD DATE ^FS" +
                            "^FO375,400,0" +
                            "^A0N,32" +
                            "^FD{7}^FS" +   //@@BUILDDATE@@
                            "^FX--the kegs box-- ^FS" +
                            "^FO800,275,1" +
                            "^GB200,200,3,B,0 ^FS" +
                            "^FX--the num of kegs^FS" +
                            "^FO775,325,1" +
                            "^A0N,128" +
                            "^FD{8}^FS" +   //@@TOTALKEGS@@
                            "^FX--a line under the header-- ^FS" +
                            "^FO25,475" +
                            "^GB750,1,4,B,1 ^FS" +
                            "^FX--summary line 1, box 1-- ^FS" +
                            "^FO25,475" +
                            "^GB250,40,2,B,0 ^FS" +
                            "^FX--summary line 1, text 1-- ^FS" +
                            "^FO40,485" +
                            "^AFN,18,10" +
                            "^FD{9}^FS" +   //@@SIZE1@@
                            "^FX--summary line 1, box 2-- ^FS" +
                            "^FO275,475" +
                            "^GB425,40,2,B,0 ^FS" +
                            "^FX--summary line 1, text 2-- ^FS" +
                            "^FO290,485" +
                            "^AFN,18,10" +
                            "^FD{10}^FS" +  //@@CONTENTS1@@
                            "^FX--summary line 1, box 3-- ^FS" +
                            "^FO800,475,1" +
                            "^GB100,40,2,B,0 ^FS" +
                            "^FX--summary line 1, text 3-- ^FS" +
                            "^FO785,485,1" +
                            "^AFN,18,10" +
                            "^FD{11}^FS" + "\r\n" +  //@@QTY1@@
                            "^FX--summary line 2, box 1-- ^FS" +
                            "^FO25,515" +
                            "^GB250,40,2,B,0 ^FS" +
                            "^FX--summary line 2, text 1-- ^FS" +
                            "^FO40,525" +
                            "^AFN,18,10" +
                            "^FD{12}^FS" +  //@@SIZE2@@
                            "^FX--summary line 2, box 2-- ^FS" +
                            "^FO275,515" +
                            "^GB425,40,2,B,0 ^FS" +
                            "^FX--summary line 2, text 2-- ^FS" +
                            "^FO290,525" +
                            "^AFN,18,10" +
                            "^FD{13}^FS" +  //@@CONTENTS2@@
                            "^FX--summary line 2, box 3-- ^FS" +
                            "^FO800,515,1" +
                            "^GB100,40,2,B,0 ^FS" +
                            "^FX--summary line 2, text 3-- ^FS" +
                            "^FO785,525,1" +
                            "^AFN,18,10" +
                            "^FD{14}^FS" + "\r\n" + //@@QTY2@@
                            "^FX--summary line 3, box 1-- ^FS" +
                            "^FO25,555" +
                            "^GB250,40,2,B,0 ^FS" +
                            "^FX--summary line 3, text 1-- ^FS" +
                            "^FO40,565" +
                            "^AFN,18,10" +
                            "^FD{15}^FS" +  //@@SIZE3@@
                            "^FX--summary line 3, box 2-- ^FS" +
                            "^FO275,555" +
                            "^GB425,40,2,B,0 ^FS" +
                            "^FX--summary line 3, text 2-- ^FS" +
                            "^FO290,565" +
                            "^AFN,18,10" +
                            "^FD{16}^FS" +  //@@CONTENTS3@@
                            "^FX--summary line 3, box 3-- ^FS" +
                            "^FO800,555,1" +
                            "^GB100,40,2,B,0 ^FS" +
                            "^FX--summary line 3, text 3-- ^FS" +
                            "^FO785,565,1" +
                            "^AFN,18,10" +
                            "^FD{17}^FS" + "\r\n" + //@@QTY3@@
                            "^FX--summary line 4, box 1-- ^FS" +
                            "^FO25,595" +
                            "^GB250,40,2,B,0 ^FS" +
                            "^FX--summary line 4, text 1-- ^FS" +
                            "^FO40,605" +
                            "^AFN,18,10" +
                            "^FD{18}^FS" +  //@@SIZE4@@
                            "^FX--summary line 4, box 2-- ^FS" +
                            "^FO275,595" +
                            "^GB425,40,2,B,0 ^FS" +
                            "^FX--summary line 4, text 2-- ^FS" +
                            "^FO290,605" +
                            "^AFN,18,10" +
                            "^FD{19}^FS" +  //@@CONTENTS4@@
                            "^FX--summary line 4, box 3-- ^FS" +
                            "^FO800,595,1" +
                            "^GB100,40,2,B,0 ^FS" +
                            "^FX--summary line 4, text 3-- ^FS" +
                            "^FO785,605,1" +
                            "^AFN,18,10" +
                            "^FD{20}^FS" + "\r\n" + //@@QTY4@@
                            "^FX--summary line 5, box 1-- ^FS" +
                            "^FO25,635" +
                            "^GB250,40,2,B,0 ^FS" +
                            "^FX--summary line 5, text 1-- ^FS" +
                            "^FO40,645" +
                            "^AFN,18,10" +
                            "^FD{21}^FS" +  //@@SIZE5@@
                            "^FX--summary line 5, box 2-- ^FS" +
                            "^FO275,635" +
                            "^GB425,40,2,B,0 ^FS" +
                            "^FX--summary line 5, text 2-- ^FS" +
                            "^FO290,645" +
                            "^AFN,18,10" +
                            "^FD{22}^FS" +  //@@CONTENTS5@@
                            "^FX--summary line 5, box 3-- ^FS" +
                            "^FO800,635,1" +
                            "^GB100,40,2,B,0 ^FS" +
                            "^FX--summary line 5, text 3-- ^FS" +
                            "^FO785,645,1" +
                            "^AFN,18,10" +
                            "^FD{23}^FS" + "\r\n" + //@@QTY5@@
                            "^FX--summary line 6, box 1-- ^FS" +
                            "^FO25,675" +
                            "^GB250,40,2,B,0 ^FS" +
                            "^FX--summary line 6, text 1-- ^FS" +
                            "^FO40,685" +
                            "^AFN,18,10" +
                            "^FD{24}^FS" +  //@@SIZE6@@
                            "^FX--summary line 6, box 2-- ^FS" +
                            "^FO275,675" +
                            "^GB425,40,2,B,0 ^FS" +
                            "^FX--summary line 6, text 2-- ^FS" +
                            "^FO290,685" +
                            "^AFN,18,10" +
                            "^FD{25}^FS" +  //@@CONTENTS6@@
                            "^FX--summary line 6, box 3-- ^FS" +
                            "^FO800,675,1" +
                            "^GB100,40,2,B,0 ^FS" +
                            "^FX--summary line 6, text 3-- ^FS" +
                            "^FO785,685,1" +
                            "^AFN,18,10" +
                            "^FD{26}^FS" + "\r\n" + "\r\n" +    //@@QTY6@@
                            "^FX--overflow ~line 6, box-- ^FS" +
                            "^FO25,715" +
                            "^GB775,60,2,B,0 ^FS" +
                            "^FX--summary line 7, text 1-- ^FS" +
                            "^FO40,725" +
                            "^AFN,16" +
                            "^FD{27}^FS" + "\r\n" +     //@@MORESUMMARY@@
                            "^FX--a line under the summary-- ^FS" +
                            "^FO25,275" +
                            "^GB750,1,4,B,1 ^FS" + "\r\n" +
                            "^FX--the SSCC label-- ^FS" +
                            "^FO50,780,0" +
                            "^ADN,16" +
                            "^FDSSCC ^FS" + "\r\n" + "\r\n" +
                            "^FX--prepared for--^FS" +
                            "^FO775, 780, 1" +
                            "^ADN, 16" +
                            "^FDPREPARED FOR ^FS" +
                            "^FX--a line under the prepared-- ^FS" +
                            "^FO800, 805, 1" +
                            "^GB200, 1, 2, B, 1 ^FS" +
                            "^FX--target location-- ^FS" +
                            "^FO775, 820, 1" +
                            "^A0N, 42" +
                            "^FD{28}^FS" +  //@@TARGETNAME@@
                            "^FX--owner address-- ^FS" +
                            "^FO775, 860, 1" +
                            "^A0N, 32" +
                            "^FD{29}^FS" +  //@@TARGETADDRESS@@
                            "^FO775, 900, 1" +
                            "^A0N, 32" +
                            "^FD{30}^FS" +  //@@TARGETCSZ@@
                            "^FO775, 940, 1" +
                            "^A0N, 32" +
                            "^FD{31}^FS" + "\r\n" + "\r\n" + "\r\n" + "\r\n" +  //@@TARGETPHONE@@
                            "^FX--the QR barcode-- ^FS" +
                            "^FO50, 800, 0" +
                            "^BQN, 2, 9, Q" +
                            "^FDQA,{32}^FS" + "\r\n" +  //@@PALLETBARCODE@@
                            "^FX--the code128 barcode-- ^FS" +
                            "^FO50, 1050, 0 ^BY2" +
                            "^BCN, 100, Y, N, N" +
                            "^FD{33}^FS" + "\r\n" + "\r\n" + //@@PALLETBARCODE@@
                            "^FX--the kegid text bottom right-- ^FS" +
                            "^FO675, 1000, 0" +
                            "^AVB, 48, 40" +
                            "^FDKegID ^FS" +
                            "^XZ";
        #endregion

        public ZebraPrinterManager(IPageDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        public async void SendZplPalletAsync(string header, string ipAddr)
        {
            Connection connection = null;
            try
            {
                if (!string.IsNullOrEmpty(ipAddr))
                {
                    connection = new TcpConnection(ipAddr, 9100);
                }
                else
                {
                    myPrinter = ConstantManager.PrinterSetting;
                    connection = myPrinter.GetConnection();
                }

                await Task.Run(async () => {
                    try
                    {
                        connection.Open();
                        PrinterLanguage printerLanguage = ZebraPrinterFactory.GetInstance(connection).PrinterControlLanguage;
                        connection.Write(GetTestLabelBytes(printerLanguage, header));
                        await Task.Delay(1000);
                    }
                    catch (Exception e)
                    {
                        // Connection Exceptions and issues are caught here
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await _dialogService.DisplayAlertAsync("Error", $"Error: {e.Message}", "Ok");
                        });
                    }
                    finally
                    {
                        try
                        {
                            connection?.Close();
                        }
                        catch (ConnectionException) { }
                    }
                });
            }
            catch (Exception ex)
            {
                // Connection Exceptions and issues are caught here
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await _dialogService.DisplayAlertAsync("Error", "Could not connect to printer", "Ok");
                });
                    Crashes.TrackError(ex);
            }
        }

        private byte[] GetTestLabelBytes(PrinterLanguage printerLanguage, string str)
        {
            if (printerLanguage == PrinterLanguage.ZPL)
            {
                return Encoding.UTF8.GetBytes(str);
            }
            else if (printerLanguage == PrinterLanguage.CPCL || printerLanguage == PrinterLanguage.LINE_PRINT)
            {
                return Encoding.UTF8.GetBytes(str);
            }
            else
            {
                throw new ZebraPrinterLanguageUnknownException();
            }
        }
    }
}
