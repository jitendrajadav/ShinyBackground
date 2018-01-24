using SQLite.Net.Attributes;

namespace KegID.Model
{
    public class BarcodeModel
    {
        [PrimaryKey]
        public string Barcode { get; set; }

        public string BarcodeJson { get; set; }
    }
}
