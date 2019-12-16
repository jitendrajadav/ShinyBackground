using KegID.ViewModel;

namespace KegID.Model
{
    public class Dashboard : BaseViewModel
    {
        public Dashboard() : base(null)
        {
        }

        public string Stock { get; set; }
        public string Empty { get; set; }
        public string InUse { get; set; }
        public string Total { get; set; }
        public string AverageCycle { get; set; }
        public string Atriskegs { get; set; }
    }
}
