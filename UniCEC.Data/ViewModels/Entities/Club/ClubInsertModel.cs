using System;
using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Club
{
    public class ClubInsertModel
    {
        [JsonPropertyName("university_id")]
        public int UniversityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        [JsonPropertyName("total_member")]
        public int TotalMember { get; set; }
        public DateTime Founding { get; set; }
    }
}
