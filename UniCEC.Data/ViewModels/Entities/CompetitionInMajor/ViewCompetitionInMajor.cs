using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.CompetitionInMajor
{
    public class ViewCompetitionInMajor
    {
        [JsonPropertyName("competition_in_major_id")]
        public int Id { get; set; }
        [JsonPropertyName("major_id")]
        public int MajorId { get; set; }
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
    }
}
