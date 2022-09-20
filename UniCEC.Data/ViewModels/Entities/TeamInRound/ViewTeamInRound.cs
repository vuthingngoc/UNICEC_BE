using System.Collections.Generic;
using System.Text.Json.Serialization;
using UniCEC.Data.ViewModels.Entities.Participant;

namespace UniCEC.Data.ViewModels.Entities.TeamInRound
{
    public class ViewTeamInRound
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("team_id")]
        public int TeamId { get; set; }
        [JsonPropertyName("team_name")]
        public string TeamName { get; set; }
        [JsonPropertyName("round_id")]
        public int RoundId { get; set; }
        [JsonPropertyName("scores")]
        public int Scores { get; set; }
        [JsonPropertyName("status")]
        public bool Status { get; set; }
        [JsonPropertyName("rank")]
        public int Rank { get; set; }
        [JsonPropertyName("number_of_participated_matches")]
        public int NumberOfParticipatedMatches { get; set; }
        [JsonPropertyName("members_in_team")]
        public List<ViewParticipantInTeam> MembersInTeam { get; set; }
        
    }
}
