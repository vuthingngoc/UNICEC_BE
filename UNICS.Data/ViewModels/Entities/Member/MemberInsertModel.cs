using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.Member
{
    public class MemberInsertModel
    {
        [JsonPropertyName("student_id")]
        public int StudentId { get; set; }
        public bool Status { get; set; }
    }
}
