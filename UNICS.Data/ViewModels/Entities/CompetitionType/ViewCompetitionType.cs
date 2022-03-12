using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.CompetitionType
{
    public class ViewCompetitionType
    {
        public int Id { get; set; }
        [JsonPropertyName("type_name")]
        public string TypeName { get; set; }
    }
}
