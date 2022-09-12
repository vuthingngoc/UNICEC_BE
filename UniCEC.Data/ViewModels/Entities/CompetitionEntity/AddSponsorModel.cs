using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.CompetitionEntity
{
    public class AddSponsorModel
    {
        public string Name { get; set; }
        [JsonPropertyName("base64_string_img")]
        public string Base64StringImg { get; set; }

        public string Website { get; set; }

        public string Email { get; set; }

        public string Description { get; set; }
    }
}
