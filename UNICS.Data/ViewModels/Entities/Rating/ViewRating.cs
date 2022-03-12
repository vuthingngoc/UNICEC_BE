using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.Rating
{
    public class ViewRating
    {
        public int Id { get; set; }
        [JsonPropertyName("student_id")]
        public int StudentId { get; set; }
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
        public int Rate { get; set; }
    }
}
