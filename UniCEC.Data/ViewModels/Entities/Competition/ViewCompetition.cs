using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.Competition
{
    public class ViewCompetition
    {
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("competition_type_id")]
        public int CompetitionTypeId { get; set; }

        [JsonPropertyName("number_of_participations")]
        public int NumberOfParticipation { get; set; }
        [JsonPropertyName("number_of_team")]
        public int NumberOfTeam { get; set; }
        [JsonPropertyName("start_time_register")]
        public DateTime StartTimeRegister { get; set; }
        [JsonPropertyName("end_time_register")]
        public DateTime EndTimeRegister { get; set; }
        [JsonPropertyName("start_time")]
        public DateTime StartTime { get; set; }
        [JsonPropertyName("end_time")]
        public DateTime EndTime { get; set; }

        [JsonPropertyName("seeds_code")]
        public string SeedsCode { get; set; }
        [JsonPropertyName("seeds_point")]
        public double SeedsPoint { get; set; }
        [JsonPropertyName("seeds_deposited")]
        public double SeedsDeposited { get; set; }
        [JsonPropertyName("is_sponsor")]
        public bool IsSponsor { get; set; }
        public bool Public { get; set; }
        public CompetitionStatus Status { get; set; }
        public string Address { get; set; }
        public int View { get; set; }
        //------------- ADD Field Sponsor Collaborate
        [JsonPropertyName("sponsors_in_competition")]
        public List<int> SponsorInCompetition_Id { get; set; }
        //------------- ADD Field Club Collaborate
        [JsonPropertyName("clubs_in_competition")]
        public List<int> ClubInCompetition_Id { get; set; }
        //------------- ADD Field Department Id 
        [JsonPropertyName("departments_in_competition")]
        public List<int> DepartmentInCompetition_Id { get; set; }
    }
}
