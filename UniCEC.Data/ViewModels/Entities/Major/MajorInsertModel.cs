using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Major
{
    public class MajorInsertModel
    {
        [JsonPropertyName("department_id")]
        public int DepartmentId { get; set; }
        [JsonPropertyName("major_code")]
        public string MajorCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
