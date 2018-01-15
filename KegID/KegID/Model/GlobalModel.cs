using SQLite.Net.Attributes;

namespace KegID.Model
{
    public class GlobalModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string SessionId { get; set; }
    }
}
