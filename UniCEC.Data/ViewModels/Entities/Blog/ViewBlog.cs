using System;
using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Blog
{
    public class ViewBlog
    {
        public int Id { get; set; }
        [JsonPropertyName("blog_type_id")]
        public int BlogTypeId { get; set; }
        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }
        [JsonPropertyName("competition_id")]
        public int? CompetitionId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [JsonPropertyName("create_time")]
        public DateTime CreateTime { get; set; }
        public int Status { get; set; }
    }
}
