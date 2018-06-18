using System.Collections.Generic;

namespace KegID.Model
{
    public class PalletModel
    {
        public string ManifestId { get; set; }
        public int Count { get; set; }
        public IList<BarcodeModel> Barcode { get; set; }
    }
}
