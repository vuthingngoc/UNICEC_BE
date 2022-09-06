using System;
using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Match
{
    public class ViewMatch
    {
        public int Id { get; set; }
        [JsonPropertyName("round_id")]
        public int RoundId { get; set; }
        [JsonPropertyName("round_name")]
        public string RoundName { get; set; }
        [JsonPropertyName("match_type_id")]
        public int MatchTypeId { get; set; }
        [JsonPropertyName("match_type_name")]
        public string MatchTypeName { get; set; }
        [JsonPropertyName("start_time")]
        public DateTime StartTime { get; set; }
        [JsonPropertyName("end_time")]
        public DateTime EndTime { get; set; }
        public int Scores { get; set; }
        public int Status { get; set; }
    }
}
