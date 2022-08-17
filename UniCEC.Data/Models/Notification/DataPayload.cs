using System.Text.Json.Serialization;

namespace UniCEC.Data.Models.Notification
{
    public class DataPayload
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("body")]
        public string Body { get; set; }
    }
}
