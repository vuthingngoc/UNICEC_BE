using System;
using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.Match
{
    public class ViewMatch
    {
        public int Id { get; set; }
        [JsonPropertyName("round_id")]
        public int RoundId { get; set; }
        [JsonPropertyName("round_name")]
        public string RoundName { get; set; }
        [JsonPropertyName("round_type_id")]
        public int RoundTypeId { get; set; }
        [JsonPropertyName("round_type_name")]
        public string RoundTypeName { get; set; }
        [JsonPropertyName("is_lose_match")]
        public bool? IsLoseMatch { get; set; }
        public string Address { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [JsonPropertyName("create_time")]
        public DateTime CreateTime { get; set; }
        [JsonPropertyName("start_time")]
        public DateTime StartTime { get; set; }
        [JsonPropertyName("end_time")]
        public DateTime EndTime { get; set; }
        [JsonPropertyName("number_of_team")]
        public int NumberOfTeam { get; set; }
        public MatchStatus Status { get; set; }
    }
}
