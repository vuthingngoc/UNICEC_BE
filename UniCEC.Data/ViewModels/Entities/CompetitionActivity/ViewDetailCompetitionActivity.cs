using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using UniCEC.Data.Enum;
using UniCEC.Data.ViewModels.Entities.ActivitiesEntity;

namespace UniCEC.Data.ViewModels.Entities.CompetitionActivity
{
    public class ViewDetailCompetitionActivity
    {
        public int Id { get; set; }
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
        public PriorityStatus Priority { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonPropertyName("seeds_point")]
        public double SeedsPoint { get; set; }
        [JsonPropertyName("seeds_code")]
        public string SeedsCode { get; set; }
        [JsonPropertyName("create_time")]
        public DateTime CreateTime { get; set; }
        public DateTime Ending { get; set; }
        [JsonPropertyName("club_activity_status")]
        public CompetitionActivityStatus Status { get; set; }
        [JsonPropertyName("num_of_member")]
        public int NumOfMember { get; set; }
        //------------- ADD Field Activities Entities -> Object Array  
        [JsonPropertyName("activities_entities")]
        public List<ViewActivitiesEntity> ActivitiesEntities { get; set; }

    }
}
