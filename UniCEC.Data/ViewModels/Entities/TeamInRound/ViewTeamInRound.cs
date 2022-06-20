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
        [JsonPropertyName("result")]
        public string Result { get; set; }
        [JsonPropertyName("status")]
        public bool Status { get; set; }
        [JsonPropertyName("rank")]
        public int Rank { get; set; }
        public List<ViewParticipantInTeam> MembersInTeam { get; set; }
    }
}
