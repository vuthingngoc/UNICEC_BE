using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using UniCEC.Data.Enum;
using UniCEC.Data.ViewModels.Entities.ActivitiesEntity;

namespace UniCEC.Data.ViewModels.Entities.CompetitionActivity
{
    public class ViewCompetitionActivity
    {
        public int Id { get; set; }
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; } 
           
        [JsonPropertyName("create_time")]
        public DateTime CreateTime { get; set; }
        public DateTime Ending { get; set; }
        public string Name { get; set; }
        public PriorityStatus Priority { get; set; }

        [JsonPropertyName("competition_activity_status")]
        public CompetitionActivityStatus Status { get; set; }

        [JsonPropertyName("creator_id")]
        public int CreatorId { get; set; }

        [JsonPropertyName("creator_name")]
        public string CreatorName { get; set; }

        //------------- ADD Field Activities Entities -> Object Array  
        [JsonPropertyName("activities_entities")]
        public List<ViewActivitiesEntity> ActivitiesEntities { get; set; }
    }
}
