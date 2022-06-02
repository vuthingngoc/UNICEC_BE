using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniCEC.Data.ViewModels.Entities.Influencer
{
    public class InfluencerInComeptitionInsertModel
    {
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }

        [JsonPropertyName("list_influencer_id")]
        public List<int> ListInfluencerId { get; set; }

        //---------Author to check user is Leader of Club and Collaborate in Copetition       
        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }        
    }
}
