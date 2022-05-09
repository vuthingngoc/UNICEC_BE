using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Member
{
    public class MemberInsertModel
    {
        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }
        [JsonPropertyName("student_id")]
        public int StudentId { get; set; }
    }
}
