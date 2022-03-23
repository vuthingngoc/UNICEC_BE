using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.DepartmentInUniversity
{
    public class DepartmentInUniversityInsertModel
    {
        [JsonPropertyName("department_id")]
        public int DepartmentId { get; set; }
        [JsonPropertyName("university_id")]
        public int UniversityId { get; set; }
    }
}
