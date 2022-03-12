using System;
using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.Blog
{
    public class BlogInsertModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
        public int? UserId { get; set; }
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
    }
}
