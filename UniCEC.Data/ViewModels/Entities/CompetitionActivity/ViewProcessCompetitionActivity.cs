using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.CompetitionActivity
{
    public class ViewProcessCompetitionActivity 
    {
               
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }

        [JsonPropertyName("competition_name")]
        public string CompetitionName { get; set; }

        [JsonPropertyName("number_activity_finished")]     
        public int numberOfFinished { get; set; }

        [JsonPropertyName("number_activity_open")]
        public int numberOfOpen { get; set; }

        [JsonPropertyName("number_activity_pending")]
        public int numberOfPending { get; set; }

        [JsonPropertyName("number_activity_completed")]
        public int numberOfCompleted{ get; set; }

        [JsonPropertyName("number_activity_cancelling")]
        public int numberOfCancelling { get; set; }

    }
}
