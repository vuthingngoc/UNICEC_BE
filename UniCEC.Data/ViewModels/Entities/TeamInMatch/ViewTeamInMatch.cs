using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.TeamInMatch
{
    public class ViewTeamInMatch
    {
        public int Id { get; set; }
        [JsonPropertyName("match_id")]
        public int MatchId { get; set; }
        [JsonPropertyName("match_title")]
        public string MatchTitle { get; set; }
        [JsonPropertyName("round_id")]
        public int RoundId { get; set; }
        [JsonPropertyName("round_name")]
        public string RoundName { get; set; }
        [JsonPropertyName("team_id")]
        public int TeamId { get; set; }
        [JsonPropertyName("team_name")]
        public string TeamName { get; set; }
        public int Scores { get; set; }
        public TeamInMatchStatus Status { get; set; }
        public string Description { get; set; }
        [JsonPropertyName("number_of_members")]
        public int NumberOfMembers { get; set; }
    }
}
