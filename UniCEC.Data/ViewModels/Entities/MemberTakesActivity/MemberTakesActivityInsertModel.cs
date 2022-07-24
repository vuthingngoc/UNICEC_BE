using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.MemberTakesActivity
{
    public class MemberTakesActivityInsertModel
    {
        [JsonPropertyName("member_id"), BindRequired]
        public int MemberId { get; set; }
        [JsonPropertyName("comeptition_activity_id"), BindRequired]
        public int CompetitionActivityId { get; set; }
        
        [JsonPropertyName("club_id"), BindRequired]
        public int ClubId { get; set; }
       
    }
}
