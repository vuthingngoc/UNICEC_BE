using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.DepartmentInUniversity
{
    public class DepartmentInUniversityInsertModel
    {
        [JsonPropertyName("department_id")]
        public int DepartmentId { get; set; }
        [JsonPropertyName("university_id")]
        public int UniversityId { get; set; }
    }
}
