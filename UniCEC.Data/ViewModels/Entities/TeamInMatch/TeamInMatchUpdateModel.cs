using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.TeamInMatch
{
    public class TeamInMatchUpdateModel
    {
        public int Id { get; set; }
        [JsonPropertyName("match_id")]
        public int MatchId { get; set; }
        [JsonPropertyName("team_id")]
        public int TeamId { get; set; }
        public int? Scores { get; set; }
        public TeamInMatchStatus? Status { get; set; }
        public string Description { get; set; }
    }
}
