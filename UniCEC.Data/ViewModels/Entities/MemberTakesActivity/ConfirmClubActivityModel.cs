using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.MemberTakesActivity
{
    public class ConfirmClubActivityModel
    {
        [JsonPropertyName("club_activity_id")]
        public int ClubActivityId { get; set; }
        //
        [JsonPropertyName("member_id")]
        public int MemberId { get; set; }
        
        [JsonPropertyName("term_id")]
        public int TermId { get; set; }

        [JsonPropertyName("member_take_activity_status")]
        public MemberTakesActivityStatus Status { get; set; }

    }
}
