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

    public class PreferenceTag
    {
        public string Name { get; set; }
        public bool Required { get; set; }
        public string Type { get; set; }
        public object DefaultValue { get; set; }
    }

    public class PreferenceTags
    {
        public IList<PreferenceTag> Tags { get; set; }
    }

}
