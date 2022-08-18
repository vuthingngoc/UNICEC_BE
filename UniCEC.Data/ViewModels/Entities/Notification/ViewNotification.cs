using System;
using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Notification
{
    public class ViewNotification
    {
        public int Id { get; set; }
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }        
        public string Title { get; set; }
        public string Body { get; set; }
        [JsonPropertyName("redirect_url")]
        public string RedirectUrl { get; set; }
        [JsonPropertyName("create_time")]
        public DateTime CreateTime { get; set; }
    }
}
