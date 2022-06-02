using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniCEC.Data.ViewModels.Entities.CompetitionManager
{
    public class CompetitionManagerUpdateModel
    {
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }

        [JsonPropertyName("member_id")]
        public int MemberId { get; set; }

        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }

        [JsonPropertyName("role_competition_id")]
        public int RoleCompetitionId { get; set; }
    }
}
