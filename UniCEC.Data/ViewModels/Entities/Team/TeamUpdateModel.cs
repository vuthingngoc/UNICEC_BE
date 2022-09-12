using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.Team
{
    public class TeamUpdateModel
    {
        [JsonPropertyName("team_id")]
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        public TeamStatus? Status { get; set; }
    }
}
