using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.CompetitionInClub
{
    public class ViewCompetitionInClub
    {
        public int Id { get; set; }
        public int ClubId { get; set; }
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
        [JsonPropertyName("is_owner")]
        public bool IsOwner { get; set; }
    }
}
