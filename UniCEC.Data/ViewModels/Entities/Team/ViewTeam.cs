using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.Team
{
    public class ViewTeam
    {
        [JsonPropertyName("team_id")]
        public int TeamId { get; set; }
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonPropertyName("invited_code")]
        public string InvitedCode { get; set; }
        public TeamStatus Status { get; set; }
        //
        [JsonPropertyName("number_of_member_in_team")]
        public int NumberOfMemberInTeam { get; set; }

    }
}
