using System.Text.Json.Serialization;

namespace UniCEC.Data.Models.Notification
{
    public class GoogleNotification
    {
        [JsonPropertyName("priority")]
        public string Priority { get; set; }
        [JsonPropertyName("data")]
        public DataPayload Data { get; set; }
        [JsonPropertyName("notification")]
        public DataPayload Notification { get; set; }
    }
}
