using System;
using System.Collections.Generic;

namespace KegID.Model
{

    public class KegMaintenanceHistoryModel 
    {
        public IList<KegMaintenanceHistoryResponseModel> KegMaintenanceHistoryResponseModel { get; set; }
        public KegIDResponse Response { get; set; }
    }

    public class KegMaintenanceHistoryResponseModel
    {
        public string KegId { get; set; }
        public string Barcode { get; set; }
        public MaintenanceType MaintenanceType { get; set; }
        public DateTimeOffset DatePerformed { get; set; }
        public string DueDate { get; set; }
        public string Message { get; set; }
        public string AlertCleared { get; set; }
        public string ManifestId { get; set; }
    }

    public class MaintenanceType
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAlert { get; set; }
        public bool IsAction { get; set; }
        public string DefectType { get; set; }
        public string ActivationMethod { get; set; }
        public string DeletedDate { get; set; }
        public bool InUse { get; set; }
        public List<string> ActivationPartnerTypes { get; set; }
    }

}
