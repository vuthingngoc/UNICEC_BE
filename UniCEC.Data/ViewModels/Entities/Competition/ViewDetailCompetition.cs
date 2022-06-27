using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.ViewModels.Entities.CompetitionEntity;
using UniCEC.Data.ViewModels.Entities.Influencer;

namespace UniCEC.Data.ViewModels.Entities.Competition
{
    public class ViewDetailCompetition
    {
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }       
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

        //------------- ADD Field Sponsor Collaborate -> Object Array
        [JsonPropertyName("sponsors_in_competition")]
        public List<ViewSponsorInComp> SponsorInCompetition { get; set; }

        //------------- ADD Field Club Collaborate -> Object Array
        [JsonPropertyName("clubs_in_competition")]
        public List<ViewClubInComp> ClubInCompetition { get; set; }

        //------------- ADD Field Department   -> Object Array
        [JsonPropertyName("departments_in_competition")]
        public List<ViewMajorInComp> DepartmentInCompetition { get; set; }

        //------------- ADD Field Infuencer   -> Object Array
        [JsonPropertyName("influencers_in_competition")]
        public List<ViewInfluencerInCompetition> InfluencerInCompetition { get; set; }

        //------------- ADD Field Competition Entity -> Object Array  
        [JsonPropertyName("competition_entities")]
        public List<ViewCompetitionEntity> CompetitionEntities { get; set; }

        //------------- ADD number of participant joining
        [JsonPropertyName("number_of_participant_join")]
        public int NumberOfParticipantJoin { get; set; }

    }
}
