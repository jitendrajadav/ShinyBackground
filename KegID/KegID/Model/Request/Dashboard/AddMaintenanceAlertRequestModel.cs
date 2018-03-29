﻿using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public partial class AddMaintenanceAlertRequestModel
    {
        public string KegId { get; set; }
        public List<long> NeededTypes { get; set; }
        public long ReminderDays { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public string Message { get; set; }
        public string AlertCc { get; set; }
    }
}
