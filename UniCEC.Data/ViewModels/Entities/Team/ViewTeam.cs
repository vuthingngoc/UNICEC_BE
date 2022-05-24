using System.Text.Json.Serialization;

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
    }
}
