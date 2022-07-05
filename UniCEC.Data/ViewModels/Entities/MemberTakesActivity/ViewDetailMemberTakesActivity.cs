using System;
using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.MemberTakesActivity
{
    public class ViewDetailMemberTakesActivity
    {
        
        public int Id { get; set; }

        [JsonPropertyName("competition_activity_id")]
        public int CompetitionActivityId { get; set; }

        [JsonPropertyName("member_id")]
        public int MemberId { get; set; }

        [JsonPropertyName("member_name")]
        public string MemberName { get; set; }

        [JsonPropertyName("booker_id")]
        public int BookerId { get; set; }

        [JsonPropertyName("booker_name")]
        public string BookerName { get; set; }
    }
}
