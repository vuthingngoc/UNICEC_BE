using System;
using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Member
{
    public class ViewDetailMember
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("club_role_id")]
        public int ClubRoleId { get; set; }
        [JsonPropertyName("club_role_name")]
        public string ClubRoleName { get; set; }
        public string Avatar { get; set; }
        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        [JsonPropertyName("join_date")]
        public DateTime JoinDate { get; set; }
        [JsonPropertyName("is_online")]
        public bool IsOnline { get; set; }
    }
}
