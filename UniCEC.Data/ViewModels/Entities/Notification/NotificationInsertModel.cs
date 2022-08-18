using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Notification
{
    public class NotificationInsertModel
    {
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("body")]
        public string Body { get; set; }
        [JsonPropertyName("redirect_url")]
        public string RedirectUrl { get; set; }
    }
}
