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
        [JsonPropertyName("create_time")]
        public DateTime CreateTime { get; set; }
        [JsonPropertyName("start_time_register")]
        public DateTime StartTimeRegister { get; set; }
        [JsonPropertyName("end_time_register")]
        public DateTime EndTimeRegister { get; set; }
        [JsonPropertyName("start_time")]
        public DateTime StartTime { get; set; }
        [JsonPropertyName("end_time")]
        public DateTime EndTime { get; set; }
        public string Content { get; set; }
        public double Fee { get; set; }
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
        [JsonPropertyName("address_name")]
        public string AddressName { get; set; }
        public string Address { get; set; }
        public int View { get; set; }

        //-------------ADD Field 
        [JsonPropertyName("club_owner_id")]
        public int ClubOwnerId { get; set; }
        [JsonPropertyName("club_owner_name")]
        public string ClubOwnerName { get; set; }
        [JsonPropertyName("club_owner_image")]
        public string ClubOwnerImage { get; set; }
        [JsonPropertyName("departments_in_competition")]
        public List<ViewDeparmentInComp> DepartmentInCompetition { get; set; }

    }
}
