using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.TeamInRound
{
    public class TeamInRoundInsertModel
    {
        [JsonPropertyName("team_id")]
        public int TeamId { get; set; }
        [JsonPropertyName("round_id")]
        public int RoundId { get; set; }
        [JsonPropertyName("scores")]
        public int Scores { get; set; }
    }
}
