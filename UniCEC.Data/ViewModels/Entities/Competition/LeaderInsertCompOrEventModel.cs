using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniCEC.Data.ViewModels.Entities.Competition
{
    public class LeaderInsertCompOrEventModel
    {
        public string Name { get; set; }
        [JsonPropertyName("competition_type_id")]
        public int CompetitionTypeId { get; set; }
        [JsonPropertyName("number_of_participations")]
        public int NumberOfParticipations { get; set; }
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
        public string Content { get; set; }
        public double Fee { get; set; }
        public bool Public { get; set; }
        [JsonPropertyName("address_name")]
        public string AddressName { get; set; }
        public string Address { get; set; }
        [JsonPropertyName("seeds_point")]
        public double SeedsPoint { get; set; }
        [JsonPropertyName("seeds_deposited")]
        public double SeedsDeposited { get; set; }

        //List Department Id Belong To University
        [JsonPropertyName("list_department_id")]
        public List<int>? ListDepartmentId { get; set; }

        //List Major
        //public List<int>? ListMajorId { get; set; } -> sửa lại

        //List Influencer Id Belong To System
        [JsonPropertyName("list_influencer_id")]
        public List<int>? ListInfluencerId { get; set; }

        //List<InfluencerModel> ListInfluencer { get; set; } -> sửa lại

        //---------Author to check user is Leader of Club   
        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }
       
    }
}
