using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.CompetitionHistory
{
    public class CompetitionHistoryStatusInsertModel
    {
   
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }

        [JsonPropertyName("changer_id")]
        public int? ChangerId { get; set; }

        [JsonPropertyName("change_date")]
        public DateTime ChangeDate { get; set; }
 
        public string Description { get; set; }

        public CompetitionStatus Status { get; set; }

    }
}
