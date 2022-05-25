using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Influencer
{
    public class ViewInfluencer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }
    }
}
