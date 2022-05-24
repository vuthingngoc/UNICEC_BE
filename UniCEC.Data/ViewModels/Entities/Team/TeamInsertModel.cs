using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Team
{
    public class TeamInsertModel
    {
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }             
    }
}
