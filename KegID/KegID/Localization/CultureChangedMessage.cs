using System.Globalization;

namespace KegID.Localization
{
    public class CultureChangedMessage
    {
        public CultureInfo NewCultureInfo { get; set; }

        public CultureChangedMessage(string lngName)
            : this(new CultureInfo(lngName))
        { }

        public CultureChangedMessage(CultureInfo newCultureInfo)
        {
            NewCultureInfo = newCultureInfo;
        }
    }
}
