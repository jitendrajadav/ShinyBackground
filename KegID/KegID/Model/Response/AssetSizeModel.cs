using Realms;
//using SQLite.Net.Attributes;

namespace KegID.Model
{
    public class AssetSizeModel : RealmObject
    {
        //[PrimaryKey]
        //public int Id { get; set; }
        public string AssetSize { get; set; }
        public bool HasInitial { get; set; }
    }

    public class AssetTypeModel : RealmObject
    {
        //[PrimaryKey]
        //public int Id { get; set; }
        public string AssetType { get; set; }
        public bool HasInitial { get; set; }
    }

    public class AssetVolumeModel : RealmObject
    {
        //[PrimaryKey]
        //public int Id { get; set; }
        public string AssetVolume { get; set; }
    }
}
