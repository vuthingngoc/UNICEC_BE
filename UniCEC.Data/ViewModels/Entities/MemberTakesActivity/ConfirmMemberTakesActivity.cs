using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.MemberTakesActivity
{
    public class ConfirmMemberTakesActivity
    {
        [JsonPropertyName("member_takes_activity_id")]
        public int MemberTakesActivityId { get; set; }
        
       

        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }
    }
}
