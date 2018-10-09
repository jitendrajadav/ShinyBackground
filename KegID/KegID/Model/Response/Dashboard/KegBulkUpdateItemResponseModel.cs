using System.Collections.Generic;

namespace KegID.Model
{
    public class KegMassUpdateKegModel 
    {
        public IList<KegMassUpdateKegResponseModel> Model { get; set; }
        public KegIDResponse Response { get; set; }
    }

    public partial class KegMassUpdateKegResponseModel 
    {
        public string KegId { get; set; }
        public Owner Owner { get; set; }
        public string Barcode { get; set; }
        public object KegName { get; set; }
        public string TypeName { get; set; }
        public string AssetTypeSourceKey { get; set; }
        public object TypeId { get; set; }
        public string SizeName { get; set; }
        public object AssetSizeSourceKey { get; set; }
        public object SizeId { get; set; }
        public string VolumeName { get; set; }
        public object AssetVolumeSourceKey { get; set; }
        public string AssetDesc { get; set; }
        public object AltBarcode { get; set; }
        public object Notes { get; set; }
        public object ReferenceKey { get; set; }
        public string FixedContents { get; set; }
        public List<object> Tags { get; set; }
        public string ProfileId { get; set; }
        public string AssetProfileName { get; set; }
        public object SkuId { get; set; }
        public object SkuCode { get; set; }
        public object SkuName { get; set; }
        public object Location { get; set; }
        public object ReceivedDate { get; set; }
        public object Pallet { get; set; }
        public string Contents { get; set; }
        public object Batch { get; set; }
        public object ContentsSkuId { get; set; }
        public object ContentsSkuCode { get; set; }
        public object ContentsSkuName { get; set; }
        public object BatchId { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public List<object> MaintenanceAlerts { get; set; }
        public object PurchaseDate { get; set; }
        public object PurchasePrice { get; set; }
        public object PurchaseOrder { get; set; }
        public object ManufacturerName { get; set; }
        public object ManufacturerId { get; set; }
        public object ManufactureLocation { get; set; }
        public object ManufactureDate { get; set; }
        public object Material { get; set; }
        public object Markings { get; set; }
        public object Colors { get; set; }
        public Profile Profile { get; set; }
    }

    public class KegBulkUpdateItemResponseModel 
    {
        public KegIDResponse Response { get; set; }
        public string UploadId { get; set; }
        public long TotalRecords { get; set; }
        public long ImportedKegs { get; set; }
        public long NewKegs { get; set; }
        public long UpdatedKegs { get; set; }
        public long ErroredKegs { get; set; }
        public List<ErrorDetail> ErrorDetails { get; set; }
    }

    public class ErrorDetail
    {
        public long RowId { get; set; }
        public string OwnerName { get; set; }
        public string Barcode { get; set; }
        public string AssetType { get; set; }
        public string AssetSize { get; set; }
        public string ErrorMessage { get; set; }
    }
}
