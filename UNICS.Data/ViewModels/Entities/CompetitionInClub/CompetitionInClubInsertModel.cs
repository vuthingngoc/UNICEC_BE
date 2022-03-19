using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.CompetitionInClub
{
    public class CompetitionInClubInsertModel
    {
        public int ClubId { get; set; }
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
    }
}
