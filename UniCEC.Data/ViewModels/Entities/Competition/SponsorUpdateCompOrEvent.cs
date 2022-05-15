using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniCEC.Data.ViewModels.Entities.Competition
{
    public class SponsorUpdateCompOrEvent
    {
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("start_time_register")]
        public DateTime? StartTimeRegister { get; set; }
        [JsonPropertyName("end_time_register")]
        public DateTime? EndTimeRegister { get; set; }
        [JsonPropertyName("start_time")]
        public DateTime? StartTime { get; set; }
        [JsonPropertyName("end_time")]
        public DateTime? EndTime { get; set; }
        [JsonPropertyName("seeds_point")]
        public double SeedsPoint { get; set; }
        [JsonPropertyName("seeds_deposited")]
        public double SeedsDeposited { get; set; }
        public string? Address { get; set; }
    }
}
