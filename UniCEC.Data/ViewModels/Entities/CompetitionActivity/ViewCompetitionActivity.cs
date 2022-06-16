using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.ViewModels.Entities.ActivitiesEntity;

namespace UniCEC.Data.ViewModels.Entities.CompetitionActivity
{
    public class ViewCompetitionActivity
    {
        public int Id { get; set; }
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
        public PriorityStatus Priority { get; set; }
        public string Name { get; set; }
          
        [JsonPropertyName("create_time")]
        public DateTime CreateTime { get; set; }
        public DateTime Ending { get; set; }
        [JsonPropertyName("club_activity_status")]
        public CompetitionActivityStatus Status { get; set; }
       
        //------------- ADD Field Activities Entities -> Object Array  
        [JsonPropertyName("activities_entities")]
        public List<ViewActivitiesEntity> ActivitiesEntities { get; set; }
    }
}
