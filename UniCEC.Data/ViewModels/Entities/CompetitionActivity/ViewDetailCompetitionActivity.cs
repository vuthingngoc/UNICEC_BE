using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using UniCEC.Data.Enum;
using UniCEC.Data.ViewModels.Entities.ActivitiesEntity;
using UniCEC.Data.ViewModels.Entities.MemberTakesActivity;

namespace UniCEC.Data.ViewModels.Entities.CompetitionActivity
{
    public class ViewDetailCompetitionActivity
    {
        public int Id { get; set; }
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
      
        [JsonPropertyName("seeds_point")]
        public double SeedsPoint { get; set; }
        //[JsonPropertyName("seeds_code")]
        //public string SeedsCode { get; set; }
        [JsonPropertyName("create_time")]
        public DateTime CreateTime { get; set; }
        public DateTime Ending { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public PriorityStatus Priority { get; set; }

        [JsonPropertyName("competition_activity_status")]
        public CompetitionActivityStatus Status { get; set; }

        [JsonPropertyName("num_of_member")]
        public int NumOfMember { get; set; }

        //------------- ADD Field Member info who create task 
        [JsonPropertyName("creator_id")]
        public int CreatorId { get; set; }
        [JsonPropertyName("creator_name")]
        public string CreatorName { get; set; }
        [JsonPropertyName("creator_email")]
        public string CreatorEmail { get; set; }

        //------------- ADD Field Activities Entities -> Object Array  
        [JsonPropertyName("activities_entities")]
        public List<ViewActivitiesEntity> ActivitiesEntities { get; set; }

        //------------- ADD Field Member Take Activity -> Object Array
        [JsonPropertyName("member_takes_activities")]
        public List<ViewMemberTakesActivity> MemberTakesActivities { get; set; }

    }
}
