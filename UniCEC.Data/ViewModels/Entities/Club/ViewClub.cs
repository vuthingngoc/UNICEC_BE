using System;
using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Club
{
    public class ViewClub
    {
        public int Id { get; set; }
        [JsonPropertyName("university_id")]
        public int UniversityId { get; set; }
        [JsonPropertyName("university_name")]
        public string UniversityName { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("image")]
        public string Image { get; set; }
        [JsonPropertyName("club_fanpage")]
        public string ClubFanpage { get; set; }
        [JsonPropertyName("club_contact")]
        public string ClubContact { get; set; }
        [JsonPropertyName("total_event")]
        public int TotalEvent { get; set; }
        [JsonPropertyName("total_activity")]
        public int TotalActivity { get; set; }
        [JsonPropertyName("total_member")]
        public int TotalMember { get; set; }
        [JsonPropertyName("member_increase_this_month")]
        public int MemberIncreaseThisMonth { get; set; }
        [JsonPropertyName("founding")]
        public DateTime Founding { get; set; }
        [JsonPropertyName("status")]
        public bool Status { get; set; }

    }
}
