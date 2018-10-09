using Realms;
using System;

namespace KegID.Model
{
    public class AssetSizeModel : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string AssetSize { get; set; }
        public bool HasInitial { get; set; }
    }

    public class AssetTypeModel : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string AssetType { get; set; }
        public bool HasInitial { get; set; }
    }

    public class AssetVolumeModel : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string AssetVolume { get; set; }
    }
}
