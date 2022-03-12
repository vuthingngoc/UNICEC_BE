using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.MajorInCompetition
{
    public class ViewMajorInCompetition
    {
        public int Id { get; set; }
        [JsonPropertyName("major_id")]
        public int MajorId { get; set; }
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
    }
}
