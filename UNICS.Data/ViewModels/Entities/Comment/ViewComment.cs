using System;
using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.Comment
{
    public class ViewComment
    {
        public int Id { get; set; }
        [JsonPropertyName("student_id")]
        public int StudentId { get; set; }
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        [JsonPropertyName("create_time")]
        public DateTime CreateTime { get; set; }
    }
}
