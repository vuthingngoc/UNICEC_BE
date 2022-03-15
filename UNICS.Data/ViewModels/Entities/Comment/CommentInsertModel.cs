using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.Comment
{
    public class CommentInsertModel
    {
        [JsonPropertyName("student_id")]
        public int StudentId { get; set; }
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
