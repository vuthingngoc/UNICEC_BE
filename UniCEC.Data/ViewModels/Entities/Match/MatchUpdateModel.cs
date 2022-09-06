using System;
using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Match
{
    public class MatchUpdateModel
    {
        public int Id { get; set; }
        [JsonPropertyName("round_id")]
        public int RoundId { get; set; }
        [JsonPropertyName("match_type_id")]
        public int MatchTypeId { get; set; }
        public string Address { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [JsonPropertyName("start_time")]
        public DateTime StartTime { get; set; }
        [JsonPropertyName("end_time")]
        public DateTime EndTime { get; set; }
        [JsonPropertyName("number_of_team")]
        public int NumberOfTeam { get; set; }
        public int Status { get; set; }
    }
}
