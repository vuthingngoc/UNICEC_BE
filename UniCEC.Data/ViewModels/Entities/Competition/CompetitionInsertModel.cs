using System;
using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.Competition
{
    public class CompetitionInsertModel
    {
        [JsonPropertyName("competition_type_id")]
        public int CompetitionTypeId { get; set; }
        [JsonPropertyName("number_of_participations")]
        public int NumberOfParticipations { get; set; }
        [JsonPropertyName("number_of_groups")]
        public int NumberOfGroups { get; set; }
        [JsonPropertyName("start_time")]
        public DateTime StartTime { get; set; }
        [JsonPropertyName("end_time")]
        public DateTime EndTime { get; set; }
        [JsonPropertyName("start_time_register")]
        public DateTime StartTimeRegister { get; set; }
        [JsonPropertyName("end_time_register")]
        public DateTime EndTimeRegister { get; set; }
        public bool Public { get; set; }
        public string Address { get; set; }
        [JsonPropertyName("seeds_point")]
        public double SeedsPoint { get; set; }
        [JsonPropertyName("seeds_deposited")]
        public double SeedsDeposited { get; set; }

        //---------Author to check user is Leader of Club
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }
        [JsonPropertyName("term_id")]
        public int TermId { get; set; }

    }
}
