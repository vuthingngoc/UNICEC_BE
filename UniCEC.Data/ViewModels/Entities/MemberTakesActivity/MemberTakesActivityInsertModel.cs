using System;
using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.MemberTakesActivity
{
    public class MemberTakesActivityInsertModel
    {
        [JsonPropertyName("member_id")]
        public int MemberId { get; set; }
        [JsonPropertyName("comeptition_activity_id")]
        public int CompetitionActivityId { get; set; }
        
        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }
       
    }
}
