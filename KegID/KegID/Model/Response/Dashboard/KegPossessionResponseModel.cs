using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public class KegPossessionResponseModel
    {
        public string KegId { get; set; }
        public string Barcode { get; set; }
        public string PossessorId { get; set; }
        public string PossessorName { get; set; }
        public string Contents { get; set; }
        public string TypeName { get; set; }
        public string SizeName { get; set; }
        public string Status { get; set; }
        public long HeldDays { get; set; }
        public string ReceivedDate { get; set; }
        public string AgeStatus { get; set; }
    }

    public class KegPossessionModel 
    {
        public KegIDResponse Response { get; set; }
        public IList<KegPossessionResponseModel> KegPossessionResponseModel { get; set; }
    }
}
