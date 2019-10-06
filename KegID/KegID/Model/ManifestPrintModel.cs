using System;

namespace KegID.Model
{
    public class ManifestPrintModel
    {
        public ManifestPrintModel()
        {
        }

        public string Original
        {
            get;
            set;
        }
        public string Destination
        {
            get;
            set;
        }

        public DateTimeOffset ShipDate { get; set; }
        public string Tracking { get; set; }
        public string Order { get; set; }

        public string Barcode { get; set; }
        public string Item { get; set; }
        public string Brand { get; set; }

    }

}
