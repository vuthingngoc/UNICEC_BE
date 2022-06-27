using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Department
{
    public class ViewDepartment
    {
        public int Id { get; set; }
        [JsonPropertyName("university_id")]
        public int UniversityId { get; set; }
        [JsonPropertyName("major_id")]
        public int MajorId { get; set; }
        [JsonPropertyName("major_code")]
        public string DepartmentCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
    }
}
