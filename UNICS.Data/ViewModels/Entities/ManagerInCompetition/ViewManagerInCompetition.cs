using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.ManagerInCompetition
{
    public class ViewManagerInCompetition
    {
        [JsonPropertyName("competition_id")]
        public int? CompetitionId { get; set; }
        [JsonPropertyName("user_id")]
        public int? UserId { get; set; }
        public int Id { get; set; }
    }
}
