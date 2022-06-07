using System;
using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Club
{
    public class ClubInsertModel
    {
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
        [JsonPropertyName("university_id")]
        public int UniversityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime Founding { get; set; }
        public bool Status { get; set; }
        [JsonPropertyName("club_fanpage")]
        public string ClubFanpage { get; set; }
        [JsonPropertyName("club_contact")]
        public string ClubContact { get; set; }


    }
}
