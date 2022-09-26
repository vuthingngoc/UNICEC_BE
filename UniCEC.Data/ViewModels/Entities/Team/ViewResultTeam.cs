using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.Team
{
    public class ViewResultTeam
    {
        [JsonPropertyName("team_id")]
        public int Id { get; set; }
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        [JsonPropertyName("number_of_member_in_team")]
        public int NumberOfMemberInTeam { get; set; }
        [JsonPropertyName("total_point")]
        public int TotalPoint { get; set; }
        public int Rank { get; set; }
    }
}
