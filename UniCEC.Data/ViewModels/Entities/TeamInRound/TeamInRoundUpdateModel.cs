using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.TeamInRound
{
    public class TeamInRoundUpdateModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("team_id")]
        public int TeamId { get; set; }
        [JsonPropertyName("round_id")]
        public int RoundId { get; set; }
        [JsonPropertyName("scores")]
        public int? Scores { get; set; }
        [JsonPropertyName("status")]
        public bool? Status { get; set; }
        [JsonPropertyName("rank")]
        public int? Rank { get; set; }
    }
}
