using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.ActivitiesEntity
{
    public class ViewActivitiesEntity
    {
        [JsonPropertyName("activities_entity_id")]
        public int Id { get; set; }

        [JsonPropertyName("competition_activity_id")]
        public int CompetitionActivityId { get; set; }

        public string Name { get; set; }

        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }
    }
}
