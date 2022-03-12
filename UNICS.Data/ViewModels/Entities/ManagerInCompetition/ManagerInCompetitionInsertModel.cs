using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.ManagerInCompetition
{
    public class ManagerInCompetitionInsertModel
    {
        [JsonPropertyName("competition_id")]
        public int? CompetitionId { get; set; }
        [JsonPropertyName("user_id")]
        public int? UserId { get; set; }
    }
}
