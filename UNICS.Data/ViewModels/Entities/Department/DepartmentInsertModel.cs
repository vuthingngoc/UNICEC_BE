using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.Department
{
    public class DepartmentInsertModel
    {
        [JsonPropertyName("competition_id")]
        public int? CompetitionId { get; set; }
        public string Name { get; set; }
    }
}
