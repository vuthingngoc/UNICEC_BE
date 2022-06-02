using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniCEC.Data.ViewModels.Entities.Competition
{
    public class LeaderDeleteCompOrEventModel
    {
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
        //---------Author to check user is Leader of Club and Collaborate in Copetition       
        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }
        
    }
}
