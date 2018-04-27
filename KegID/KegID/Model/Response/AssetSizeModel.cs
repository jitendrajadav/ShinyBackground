using SQLite.Net.Attributes;

namespace KegID.Model
{
    public class AssetSizeModel
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string AssetSize { get; set; }
    }

    public class AssetTypeModel
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string AssetType { get; set; }
    }

    public class AssetVolumeModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string AssetVolume { get; set; }
    }
}
