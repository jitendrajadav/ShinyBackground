using System.Collections.Generic;

namespace KegID.Model
{
    public class PreferenceValueResponseModel
    {
        public List<SelectedWidget> SelectedWidgets { get; set; }
        public long GridWidth { get; set; }
    }

    public class SelectedWidget
    {
        public string Id { get; set; }
        public double X { get; set; }
        public Pos Pos { get; set; }
    }

    public class Pos
    {
        public long X { get; set; }
        public long Y { get; set; }
    }
}
