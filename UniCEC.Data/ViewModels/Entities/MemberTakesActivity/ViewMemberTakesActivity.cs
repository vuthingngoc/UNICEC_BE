using System;
using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.MemberTakesActivity
{
    public class ViewMemberTakesActivity
    {
        public int Id { get; set; }
        [JsonPropertyName("member_id")]
        public int MemberId { get; set; }
        [JsonPropertyName("competition_activity_id")]
        public int CompetitionActivityId { get; set; }
        [JsonPropertyName("start_time")]
        public DateTime StartTime { get; set; }
        [JsonPropertyName("end_time")]
        public DateTime EndTime { get; set; }
        public DateTime Deadline { get; set; }
        public MemberTakesActivityStatus Status { get; set; }
    }
}
