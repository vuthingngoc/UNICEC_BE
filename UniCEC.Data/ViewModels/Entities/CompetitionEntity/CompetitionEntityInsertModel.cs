using System;
using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.CompetitionEntity
{
    public class CompetitionEntityInsertModel
    {
        [JsonPropertyName("entity_type_id")]
        public int EntityTypeId { get; set; }
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string Description { get; set; }
        [JsonPropertyName("last_modified")]
        public DateTime LastModified { get; set; }
    }
}
