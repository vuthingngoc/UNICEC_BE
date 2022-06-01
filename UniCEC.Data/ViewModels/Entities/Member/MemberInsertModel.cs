using System;
using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Member
{
    public class MemberInsertModel
    {
        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
        [JsonPropertyName("club_role_id")]
        public int ClubRoleId { get; set; }
        [JsonPropertyName("term_id")]
        public int TermId { get; set; }
        [JsonPropertyName("start_time")]
        public DateTime StartTime { get; set; }
        [JsonPropertyName("end_time")]
        public DateTime? EndTime { get; set; }

    }
}
