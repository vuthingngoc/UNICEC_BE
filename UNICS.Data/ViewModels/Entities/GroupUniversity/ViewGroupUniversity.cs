using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.GroupUniversity
{
    public class ViewGroupUniversity
    {
        public int Id { get; set; }
        [JsonPropertyName("university_id")]
        public int? UniversityId { get; set; }
        [JsonPropertyName("competition_id")]
        public int? CompetitionId { get; set; }
    }
}
