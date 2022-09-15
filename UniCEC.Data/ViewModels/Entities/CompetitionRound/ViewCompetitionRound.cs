using System;
using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.CompetitionRound
{
    public class ViewCompetitionRound
    {
        public int Id { get; set; }
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
        [JsonPropertyName("round_type_id")]
        public int RoundTypeId { get; set; }
        [JsonPropertyName("round_type_name")]
        public string RoundTypeName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [JsonPropertyName("start_time")]
        public DateTime StartTime { get; set; }
        [JsonPropertyName("end_time")]
        public DateTime EndTime { get; set; }
        [JsonPropertyName("number_of_team")]
        public int NumberOfTeam { get; set; }
        [JsonPropertyName("seeds_point")]
        public int SeedsPoint { get; set; }
        public CompetitionRoundStatus Status { get; set; }
        public int Order { get; set; }
    }
}
