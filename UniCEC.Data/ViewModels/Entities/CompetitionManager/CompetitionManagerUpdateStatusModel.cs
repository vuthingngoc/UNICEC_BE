using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniCEC.Data.ViewModels.Entities.CompetitionManager
{
    public class CompetitionManagerUpdateStatusModel
    {
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }

        [JsonPropertyName("member_id")]
        public int MemberId { get; set; }

        public bool Status { get; set; }

        //---------Author to check user is Leader of Club   
        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }
    }
}
