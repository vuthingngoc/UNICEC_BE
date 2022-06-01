using System;
using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.Member
{
    public class MemberUpdateModel
    {
        public int Id { get; set; }
        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }
        [JsonPropertyName("club_role_id")]
        public int? ClubRoleId { get; set; }
        [JsonPropertyName("start_time")]
        public DateTime? StartTime { get; set; }
        [JsonPropertyName("end_time")]
        public DateTime? EndTime { get; set; }
        public MemberStatus? Status { get; set; }
    }
}
