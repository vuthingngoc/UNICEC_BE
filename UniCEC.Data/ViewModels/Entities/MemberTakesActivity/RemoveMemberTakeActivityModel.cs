using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniCEC.Data.ViewModels.Entities.MemberTakesActivity
{
    public class RemoveMemberTakeActivityModel
    {
        [JsonPropertyName("member_takes_activity_id"), BindRequired]
        public int MemberTakesActivityId { get; set; }

        [JsonPropertyName("club_id"), BindRequired]
        public int ClubId { get; set; }
    }
}
