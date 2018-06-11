//using SQLite.Net.Attributes;

using Realms;

namespace KegID.Model
{
    public class BarcodeModel : RealmObject
    {
        [PrimaryKey]
        public string Barcode { get; set; }

        public string BarcodeJson { get; set; }
    }
}
