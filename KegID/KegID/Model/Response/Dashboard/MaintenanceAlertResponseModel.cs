using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public partial class MaintenanceAlertResponseModel
    {
        public string KegId { get; set; }
        public string Barcode { get; set; }
        public MaintenanceType MaintenanceType { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public string Message { get; set; }
        public bool IsActivated { get; set; }
        public object ActivationDate { get; set; }
        public long ScheduleId { get; set; }
    }

    public class MaintenanceAlertModel : KegIDResponse
    {
        public IList<MaintenanceAlertResponseModel> MaintenanceAlertResponseModel { get; set; }
    }
}
