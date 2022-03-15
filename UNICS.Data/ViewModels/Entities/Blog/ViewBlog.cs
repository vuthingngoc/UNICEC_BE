using System;
using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.Blog
{
    public class ViewBlog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [JsonPropertyName("create_time")]
        public DateTime? CreateTime { get; set; }
        public int? Status { get; set; }
        [JsonPropertyName("user_id")]
        public int? UserId { get; set; }
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
    }
}
