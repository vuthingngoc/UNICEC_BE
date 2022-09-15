using System;
using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.CompetitionRound
{
    public class CompetitionRoundInsertModel
    {
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
        [JsonPropertyName("round_type_id")]
        public int RoundTypeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [JsonPropertyName("start_time")]
        public DateTime StartTime { get; set; }
        [JsonPropertyName("end_time")]
        public DateTime EndTime { get; set; }
        [JsonPropertyName("seeds_point")]
        public int SeedsPoint { get; set; }
    }
}
