using Microsoft.AspNetCore.Mvc.ModelBinding;
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

        [JsonPropertyName("min_team_or_participant")]
        public int MinTeamOrParticipant { get; set; }

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
        public CompetitionScopeStatus Scope { get; set; }
        [JsonPropertyName("is_event")]
        public bool IsEvent { get; set; }
        [JsonPropertyName("address_name")]
        public string AddressName { get; set; }
        public string Address { get; set; }
        [JsonPropertyName("seeds_point")]
        public double SeedsPoint { get; set; }


        //List Department Id Belong To University
        [JsonPropertyName("list_major_id")]
        public List<int> ListMajorId { get; set; }

        //List Image
        [JsonPropertyName("list_image")]
        public List<AddImageModel> ListImage { get; set; }

        //List Influencer
        [JsonPropertyName("list_influencer")]
        public List<AddInfluencerModel> ListInfluencer { get; set; }

        //List Sponsor
        [JsonPropertyName("list_sponsor")]
        public List<AddSponsorModel> ListSponsor { get; set; }


        //---------Author to check user is Leader of Club   
        [JsonPropertyName("club_id"), BindRequired]
        public int ClubId { get; set; }
       
    }
}
