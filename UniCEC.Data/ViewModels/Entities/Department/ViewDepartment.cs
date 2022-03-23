using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Department
{
    public class ViewDepartment
    {
        public int Id { get; set; }
        [JsonPropertyName("competition_id")]
        public int? CompetitionId { get; set; }
        public string Name { get; set; }
    }
}
