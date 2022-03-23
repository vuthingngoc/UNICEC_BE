using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.CompetitionType
{
    public class CompetitionTypeInsertModel
    {
        [JsonPropertyName("type_name")]
        public string TypeName { get; set; }
    }
}
