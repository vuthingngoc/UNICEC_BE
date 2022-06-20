using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.City
{
    public class ViewCity
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("status")]
        public bool Status { get; set; }
    }
}
