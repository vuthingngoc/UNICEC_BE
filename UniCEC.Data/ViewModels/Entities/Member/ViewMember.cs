using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Member
{
    public class ViewMember
    {
        public int Id { get; set; }
        [JsonPropertyName("student_id")]
        public int StudentId { get; set; }
        public bool Status { get; set; }
    }
}
