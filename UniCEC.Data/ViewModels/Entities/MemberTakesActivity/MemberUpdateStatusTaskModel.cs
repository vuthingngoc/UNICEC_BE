using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.MemberTakesActivity
{
    public class MemberUpdateStatusTaskModel
    {
        [JsonPropertyName("competition_activity_id"), BindRequired]
        public int CompetitionActivityId { get; set; }       
        public CompetitionActivityStatus Status { get; set; }
        //---------Author to check user is Leader of Club 
        [JsonPropertyName("club_id"), BindRequired]
        public int ClubId { get; set; }
    }
}
