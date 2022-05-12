using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Sponsor
{
    public class SponsorInsertModel
    {
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Logo { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? Address { get; set; }
        public bool Status { get; set; }
    }
}
