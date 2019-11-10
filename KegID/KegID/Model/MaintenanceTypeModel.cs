using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public class MaintenanceTypeModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAlert { get; set; }
        public bool IsAction { get; set; }
        public string DefectType { get; set; }
        public string ActivationMethod { get; set; }
        public DateTimeOffset DeletedDate { get; set; }
        public bool InUse { get; set; }
        public bool IsToggled { get; set; }
        public List<string> ActivationPartnerTypes { get; }
    }
}
