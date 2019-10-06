using PropertyChanged;
using Realms;
using System;
using System.Collections.Generic;

namespace KegID.Model
{

    public class MaintenanceModel : RealmObject
    {
        [DoNotNotify]
        public MaintenanceDoneRequestModel MaintenanceDoneRequestModel { get; set; }
        [DoNotNotify]
        public KegIDResponse Response { get; set; }
    }

    public class MaintenanceDoneRequestModel : RealmObject
    {
        [DoNotNotify]
        public string MaintenancePostingId { get; set; }
        [DoNotNotify]
        public IList<long> ActionsPerformed { get; }
        [DoNotNotify]
        public DateTimeOffset DatePerformed { get; set; }
        [DoNotNotify]
        public string LocationId { get; set; }
        [DoNotNotify]
        public IList<MaintainKeg> Kegs { get; }
        [DoNotNotify]
        public IList<MaintenanceDoneRequestModelTag> Tags { get; }
        [DoNotNotify]
        public long Latitude { get; set; }
        [DoNotNotify]
        public long Longitude { get; set; }
        [DoNotNotify]
        public string Notes { get; set; }
        [DoNotNotify]
        public PartnerModel PartnerModel { get; set; }
    }

    public class MaintainKeg: RealmObject
    {
        [DoNotNotify]
        public string Barcode { get; set; }
        [DoNotNotify]
        public DateTimeOffset ScanDate { get; set; }
        [DoNotNotify]
        public IList<KegTag> Tags { get; }
        [DoNotNotify]
        public int ValidationStatus { get; set; }
    }

    public class KegTag : RealmObject
    {
        [DoNotNotify]
        public string Property { get; set; }
        [DoNotNotify]
        public string Value { get; set; }
    }

    public class MaintenanceDoneRequestModelTag : RealmObject
    {
        [DoNotNotify]
        public string Name { get; set; }
        [DoNotNotify]
        public string Value { get; set; }
    }
}
