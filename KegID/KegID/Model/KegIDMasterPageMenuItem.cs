using System;

namespace KegID.Model
{
    public class KegIDMasterPageMenuItem
    {
        public KegIDMasterPageMenuItem()
        {
            TargetType = typeof(Type);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public string MenuIcon { get; set; }
        public Type TargetType { get; set; }
    }

}
