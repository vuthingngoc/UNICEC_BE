using System;
using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Match
{
    public class MatchInsertModel
    {
        [JsonPropertyName("round_id")]
        public int RoundId { get; set; }
        [JsonPropertyName("is_lose_match")]
        public bool? IsLoseMatch { get; set; }
        public string Address { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [JsonPropertyName("start_time")]
        public DateTime StartTime { get; set; }
        [JsonPropertyName("end_time")]
        public DateTime EndTime { get; set; }
        [JsonPropertyName("number_of_team")]
        public int? NumberOfTeam { get; set; }
    }
}
