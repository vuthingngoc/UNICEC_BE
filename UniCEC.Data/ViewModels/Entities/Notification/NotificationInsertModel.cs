using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Notification
{
    public class NotificationInsertModel
    {
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
        [JsonPropertyName("device_id")]
        public string DeviceId { get; set; }
        [JsonPropertyName("is_android_device")]
        public bool IsAndroidDevice { get; set; }
    }
}
