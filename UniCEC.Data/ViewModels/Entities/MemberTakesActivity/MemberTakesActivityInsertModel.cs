using System;
using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.MemberTakesActivity
{
    public class MemberTakesActivityInsertModel
    {
        [JsonPropertyName("member_id")]
        public int MemberId { get; set; }
        [JsonPropertyName("club_activity_id")]
        public int ClubActivityId { get; set; }
        [JsonPropertyName("term_id")]
        public int TermId { get; set; }
        [JsonPropertyName("start_time")]
        public DateTime StartTime { get; set; }
        //[JsonPropertyName("end_time")]
        //public DateTime EndTime { get; set; }
       // public DateTime Deadline { get; set; }
        //public MemberTakesActivityStatus Status { get; set; }
    }
}
