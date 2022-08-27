using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Competition
{
    public class ViewMajorInComp
    {
        public int Id { get; set; }

        [JsonPropertyName("major_id")]
        public int MajorId { get; set; }

        public string Name { get; set; }
    }
}
