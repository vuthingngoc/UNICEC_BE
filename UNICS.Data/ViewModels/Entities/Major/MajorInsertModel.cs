using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.Major
{
    public class MajorInsertModel
    {
        [JsonPropertyName("major_type_id")]
        public int MajorTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
    }
}
