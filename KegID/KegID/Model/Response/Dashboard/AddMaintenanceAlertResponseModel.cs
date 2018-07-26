using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public class AddMaintenanceAlertResponseModel
    {
        public string KegId { get; set; }
        public string Barcode { get; set; }
        public MaintenanceType MaintenanceType { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public string Message { get; set; }
        public bool IsActivated { get; set; }
        public DateTimeOffset ActivationDate { get; set; }
        public long ScheduleId { get; set; }
    }

    public class AddMaintenanceAlertModel 
    {
        public IList<AddMaintenanceAlertResponseModel> AddMaintenanceAlertResponseModel { get; set; }
        public KegIDResponse Response { get; set; }
    }
}
