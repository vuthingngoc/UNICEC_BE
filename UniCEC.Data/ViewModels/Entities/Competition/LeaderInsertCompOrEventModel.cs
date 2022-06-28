using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using UniCEC.Data.Enum;
using UniCEC.Data.ViewModels.Entities.CompetitionEntity;

namespace UniCEC.Data.ViewModels.Entities.Competition
{
    public class LeaderInsertCompOrEventModel
    {
        public string Name { get; set; }
        [JsonPropertyName("competition_type_id")]
        public int CompetitionTypeId { get; set; }
        [JsonPropertyName("number_of_participations")]
        public int NumberOfParticipations { get; set; }

        [JsonPropertyName("max_number_member_in_team")]
        public int? MaxNumberMemberInTeam { get; set; }

        [JsonPropertyName("min_number_member_in_team")]
        public int? MinNumberMemberInTeam { get; set; }
        
        [JsonPropertyName("end_time_register")]
        public DateTime EndTimeRegister { get; set; }
        [JsonPropertyName("start_time")]
        public DateTime StartTime { get; set; }
        [JsonPropertyName("end_time")]
        public DateTime EndTime { get; set; }
        public string Content { get; set; }
        public double Fee { get; set; }
        public CompetitionScopeStatus Scope { get; set; }
        [JsonPropertyName("is_event")]
        public bool IsEvent { get; set; }
        [JsonPropertyName("address_name")]
        public string AddressName { get; set; }
        public string Address { get; set; }
        [JsonPropertyName("seeds_point")]
        public double SeedsPoint { get; set; }

        //List Department Id Belong To University
        [JsonPropertyName("list_department_id")]
        public List<int>? ListMajorId { get; set; }
        
        //Add competition entity
        public AddCompetitionEntity? CompetitionEntity { get; set; }

        //List Influencer Id Belong To System
        //[JsonPropertyName("list_influencer")]
        //public List<InfluencerInsertModel>? ListInfluencer { get; set; }

        //---------Author to check user is Leader of Club   
        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }
       
    }
}
