using System.Collections.Generic;
using System.Text.Json.Serialization;
using UniCEC.Data.Enum;
using UniCEC.Data.ViewModels.Entities.Participant;

namespace UniCEC.Data.ViewModels.Entities.Team
{
    public class ViewTeamInCompetition
    {
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
        [JsonPropertyName("competition_name")]
        public string CompetitionName { get; set; }
        [JsonPropertyName("team_id")]
        public int TeamId { get; set; }
        [JsonPropertyName("team_name")]
        public string TeamName { get; set; }
        [JsonPropertyName("members_in_team")]
        public List<ViewParticipantInTeam> MembersInTeam { get; set; }
        [JsonPropertyName("team_in_rounds")]
        public List<ViewResultTeamInRounds> TeamInRounds { get; set; }
    }

    public class ViewResultTeamInRounds
    {
        [JsonPropertyName("round_id")]
        public int RoundId { get; set; }
        [JsonPropertyName("round_name")]
        public string RoundName { get; set; }
        [JsonPropertyName("round_type_id")]
        public int RoundTypeId { get; set; }
        [JsonPropertyName("round_type_name")]
        public string RoundTypeName { get; set; }
        [JsonPropertyName("scores")]
        public int Scores { get; set; }
        [JsonPropertyName("status")]
        public bool Status { get; set; }
        [JsonPropertyName("rank")]
        public int Rank { get; set; }        
        [JsonPropertyName("team_in_matches")]
        public List<ViewResultTeamInMatches> TeamInMatches { get; set; }

    }

    public class ViewResultTeamInMatches
    {
        [JsonPropertyName("match_id")]
        public int MatchId { get; set; }
        [JsonPropertyName("match_title")]
        public string MatchTitle { get; set; }
        [JsonPropertyName("scores")]
        public int Scores { get; set; }
        [JsonPropertyName("status")]
        public TeamInMatchStatus Status { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
