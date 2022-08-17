using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Notification
{
    public class ViewNotification
    {
        [JsonPropertyName("is_success")]
        public bool IsSuccess { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
