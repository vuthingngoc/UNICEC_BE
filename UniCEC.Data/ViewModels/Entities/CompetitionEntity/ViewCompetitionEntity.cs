using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.CompetitionEntity
{
    public class ViewCompetitionEntity
    {
        [JsonPropertyName("competition_entity_id")]
        public int Id { get; set; }

        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }

        [JsonPropertyName("entity_type_id")]
        public int EntityTypeId { get; set; }

        [JsonPropertyName("entity_type_name")]
        public string EntityTypeName { get; set; }
        public string Name { get; set; }

        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }

        public string Website { get; set; }

        public string Email { get; set; }

        public string Description { get; set; }    
    }
}
