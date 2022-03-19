using System;
using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.MemberTakesActivity
{
    public class ViewMemberTakesActivity
    {
        public int Id { get; set; }
        [JsonPropertyName("member_id")]
        public int MemberId { get; set; }
        [JsonPropertyName("club_activity_id")]
        public int ClubActivityId { get; set; }
        [JsonPropertyName("start_time")]
        public DateTime StartTime { get; set; }
        [JsonPropertyName("end_time")]
        public DateTime EndTime { get; set; }
        public DateTime Deadline { get; set; }
        public int Status { get; set; }
    }
}
