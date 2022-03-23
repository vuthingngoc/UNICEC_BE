using System;
using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Competition
{
    public class ViewCompetition
    {
        public int Id { get; set; }
        [JsonPropertyName("competition_type_id")]
        public int CompetitionTypeId { get; set; }
        public string Organizer { get; set; }
        [JsonPropertyName("number_of_participations")]
        public int NumberOfParticipation { get; set; }
        [JsonPropertyName("number_of_groups")]
        public int NumberOfGroup { get; set; }
        [JsonPropertyName("approved_time")]
        public DateTime ApprovedTime { get; set; }
        [JsonPropertyName("start_time")]
        public DateTime StartTime { get; set; }
        [JsonPropertyName("end_time")]
        public DateTime EndTime { get; set; }
        [JsonPropertyName("start_time_register")]
        public DateTime StartTimeRegister { get; set; }
        [JsonPropertyName("end_time_register")]
        public DateTime EndTimeRegister { get; set; }
        [JsonPropertyName("seeds_code")]
        public string SeedsCode { get; set; }
        [JsonPropertyName("seeds_point")]
        public double SeedsPoint { get; set; }
        [JsonPropertyName("seeds_deposited")]
        public double SeedsDeposited { get; set; }
        public bool Public { get; set; }
        public int Status { get; set; }
        public string Address { get; set; }
        public int View { get; set; }
    }
}
