using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using UniCEC.Data.Enum;
using UniCEC.Data.ViewModels.Entities.CompetitionEntity;

namespace UniCEC.Data.ViewModels.Entities.Competition
{
    public class ViewDetailCompetition
    {
        
        public int Id { get; set; }
        [JsonPropertyName("university_id")]
        public int UniversityId { get; set; }

        [JsonPropertyName("university_name")]
        public string UniversityName { get; set; }

        [JsonPropertyName("university_image")]
        public string UniversityImage { get; set; }

        [JsonPropertyName("competition_type_id")]
        public int CompetitionTypeId { get; set; }
        [JsonPropertyName("competition_type_name")]
        public string CompetitionTypeName { get; set; }
        public double Fee { get; set; }
        [JsonPropertyName("seeds_code")]
        public string SeedsCode { get; set; }
        [JsonPropertyName("seeds_point")]
        public double SeedsPoint { get; set; }
        [JsonPropertyName("seeds_deposited")]
        public double SeedsDeposited { get; set; }
        [JsonPropertyName("number_of_participations")]
        public int NumberOfParticipation { get; set; }
        [JsonPropertyName("number_of_team")]
        public int? NumberOfTeam { get; set; }

        [JsonPropertyName("max_number")]
        public int? MaxNumber { get; set; }

        [JsonPropertyName("min_number")]
        public int? MinNumber { get; set; }

        [JsonPropertyName("min_team_or_participant")]
        public int MinTeamOrParticipant { get; set; }

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
        public string Name { get; set; }

        [JsonPropertyName("address_name")]
        public string AddressName { get; set; }
        public string Address { get; set; }
        public string Content { get; set; }
     
        [JsonPropertyName("is_sponsor")]
        public bool IsSponsor { get; set; }
        public CompetitionScopeStatus Scope { get; set; }
        public CompetitionStatus Status { get; set; }   
        public int View { get; set; }

        //------------- ADD Field Club Collaborate -> Object Array
        [JsonPropertyName("clubs_in_competition")]
        public List<ViewClubInComp> ClubInCompetition { get; set; }

        //------------- ADD Field Department   -> Object Array
        [JsonPropertyName("majors_in_competition")]
        public List<ViewMajorInComp> MajorsInCompetition { get; set; }

        //------------- ADD Field Competition Entity -> Object Array  
        [JsonPropertyName("competition_entities")]
        public List<ViewCompetitionEntity> CompetitionEntities { get; set; }

        //------------- ADD number of participant joining
        [JsonPropertyName("number_of_participant_join")]
        public int NumberOfParticipantJoin { get; set; }

    }
}
