using Realms;
using System;
using System.Collections.Generic;

namespace KegID.Model
{

    public class MaintenanceModel : RealmObject
    {
        public MaintenanceDoneRequestModel MaintenanceDoneRequestModel { get; set; }
        public KegIDResponse Response { get; set; }
    }

    public class MaintenanceDoneRequestModel : RealmObject
    {
        public string MaintenancePostingId { get; set; }
        public IList<long> ActionsPerformed { get; }
        public DateTimeOffset DatePerformed { get; set; }
        public string LocationId { get; set; }
        public IList<MaintainKeg> Kegs { get; }
        public IList<MaintenanceDoneRequestModelTag> Tags { get; }
        public long Latitude { get; set; }
        public long Longitude { get; set; }
    }

    public class MaintainKeg: RealmObject
    {
        public string Barcode { get; set; }
        public DateTimeOffset ScanDate { get; set; }
        public IList<KegTag> Tags { get; }
        public int ValidationStatus { get; set; }
    }

    public class KegTag : RealmObject
    {
        public string Property { get; set; }
        public string Value { get; set; }
    }

    public class MaintenanceDoneRequestModelTag : RealmObject
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
