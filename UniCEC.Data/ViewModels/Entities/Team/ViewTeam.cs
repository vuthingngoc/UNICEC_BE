using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Team
{
    public class ViewTeam
    {
        public int Id { get; set; }
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonPropertyName("invited_code")]
        public string InvitedCode { get; set; }
    }
}
