using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.Competition
{
    public class LeaderUpdateCompOrEventModel
    {

        public int Id { get; set; }
        [JsonPropertyName("competition_type_id")]
        public int? CompetitionTypeId { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("start_time_register")]
        public DateTime? StartTimeRegister { get; set; }
        [JsonPropertyName("end_time_register")]
        public DateTime? EndTimeRegister { get; set; }
        [JsonPropertyName("start_time")]
        public DateTime? StartTime { get; set; }
        [JsonPropertyName("end_time")]
        public DateTime? EndTime { get; set; }
        public string Content { get; set; }
        public double? Fee { get; set; }
        [JsonPropertyName("seeds_point")]
        public double? SeedsPoint { get; set; }    
        [JsonPropertyName("address_name")]
        public string AddressName { get; set; }
        public string Address { get; set; }
        [JsonPropertyName("number_of_participant")]
        public int? NumberOfParticipant { get; set; }
        [JsonPropertyName("max_number")]
        public int? MaxNumber { get; set; }
        [JsonPropertyName("min_number")]
        public int? MinNumber { get; set; }

        [JsonPropertyName("min_team_or_participant")]
        public int? MinTeamOrParticipant { get; set; }
        public CompetitionScopeStatus? Scope { get; set; }

        //---------List DepartmentID belong to University Insert in Competition
        [JsonPropertyName("list_major_id")]
        public List<int> ListMajorId { get; set; }

        //---------Author to check user is Leader of Club and Collaborate in Copetition
        [JsonPropertyName("club_id"), BindRequired]
        public int ClubId { get; set; }

        
       
    }
}
