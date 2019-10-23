using PropertyChanged;
using Realms;
using System;

namespace KegID.Model
{
    public class AssetSizeModel : RealmObject
    {
        [PrimaryKey]
        [DoNotNotify]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [DoNotNotify]
        public string AssetSize { get; set; }
        [DoNotNotify]
        public bool HasInitial { get; set; }
    }

    public class AssetTypeModel : RealmObject
    {
        [PrimaryKey]
        [DoNotNotify]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [DoNotNotify]
        public string AssetType { get; set; }
        [DoNotNotify]
        public bool HasInitial { get; set; }
    }

    public class AssetVolumeModel : RealmObject
    {
        [PrimaryKey]
        [DoNotNotify]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [DoNotNotify]
        public string AssetVolume { get; set; }
    }

    public class OperatorModel : RealmObject
    {
        [PrimaryKey]
        [DoNotNotify]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [DoNotNotify]
        public string Operator { get; set; }
    }
}
