using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniCEC.Data.ViewModels.Entities.Club
{
    public class ViewActivityOfClubModel
    {
        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }
        [JsonPropertyName("total_activity_of_club_create")]
        public int TotalActivity { get; set; }
        [JsonPropertyName("total_activity_of_club_complete")]
        public int TotalActivityComplete { get; set; }
        [JsonPropertyName("total_activity_of_club_late")]
        public int TotalActivityLate { get; set; }
    }
}
