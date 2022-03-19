using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.ManagerInCompetition
{
    public class MemberInsertModel
    {
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
        public bool Status { get; set; }
    }
}
