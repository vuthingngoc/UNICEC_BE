using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniCEC.Data.ViewModels.Entities.Influencer
{
    public class ViewInfluencerInCompetition : ViewInfluencer
    {
        [JsonPropertyName("influencer_in_competition_id")]
        public int InfluencerInCompetitionId { get; set; }
        [JsonPropertyName("influencer_id")]
        public int CompetitionId { get; set; }
        
    }
}
