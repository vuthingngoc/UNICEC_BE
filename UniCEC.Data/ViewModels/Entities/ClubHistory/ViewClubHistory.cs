using System;
using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.ClubHistory
{
    public class ViewClubHistory
    {
        public int Id { get; set; }
        [JsonPropertyName("club_role_id")]
        public int ClubRoleId { get; set; }
        [JsonPropertyName("club_role_name")]
        public string ClubRoleName { get; set; }
        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }
        [JsonPropertyName("club_name")]
        public string ClubName { get; set; }
        [JsonPropertyName("member_id")]
        public int MemberId { get; set; }
        [JsonPropertyName("term_id")]
        public int TermId { get; set; }
        [JsonPropertyName("term_name")]
        public string TermName { get; set; }
        [JsonPropertyName("start_time")]
        public DateTime StartTime { get; set; }
        [JsonPropertyName("end_time")]
        public DateTime? EndTime { get; set; }
        public MemberStatus Status { get; set; }
    }
}
