using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.ManagerInCompetition
{
    public class ViewMember
    {
        public int Id { get; set; }
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
        public bool Status { get; set; }
    }
}
