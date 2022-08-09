using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniCEC.Data.ViewModels.Entities.Competition
{
    public class ViewProcessCompetitionOrEventOfClub
    {      
        
        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }

        [JsonPropertyName("number_competition_registering")]
        public int numberCompetitionOfRegistering { get; set; }

        [JsonPropertyName("number_competition_upcoming")]
        public int numberCompetitionOfUpComing { get; set; }

        [JsonPropertyName("number_competition_ongoing")]
        public int numberCompetitionOfOnGoing { get; set; }

        [JsonPropertyName("number_competition_completed")]
        public int numberCompetitionOfCompleted { get; set; }

        [JsonPropertyName("number_event_registering")]
        public int numberEventOfRegistering { get; set; }

        [JsonPropertyName("number_event_upcoming")]
        public int numberEventOfUpComing { get; set; }

        [JsonPropertyName("number_event_ongoing")]
        public int numberEventOfOnGoing { get; set; }

        [JsonPropertyName("number_event_completed")]
        public int numberEventOfCompleted { get; set; }
    }

}
