using System;
using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.Blog
{
    public class BlogInsertModel
    {
        [JsonPropertyName("blog_type_id")]
        public int BlogTypeId { get; set; }
        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }
        [JsonPropertyName("competition_id")]
        public int? CompetitionId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
    }
}
