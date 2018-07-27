namespace KegID.Model
{
    public class DeviceCheckinRequestModel
    {
        public string UserId { get; set; }
        public string DeviceId { get; set; }
        public string DeviceModel { get; set; }
        public string OS { get; set; }
        public string AppVersion { get; set; }
        public string PushToken { get; set; }
    }
}
