using System;

namespace KegID.Model
{
    public class DashboardResponseModel 
    {
        public KegIDResponse Response { get; set; }
        public long ActiveKegs { get; set; }
        public long InPossession { get; set; }
        public long Stock { get; set; }
        public long Empty { get; set; }
        public long InUse { get; set; }
        public long AverageCycle { get; set; }
        public long OldKegs { get; set; }
        public long InactiveKegs { get; set; }
        public double TurnsPerYear { get; set; }
        public string GeneratedAt { get; set; }
    }
}
