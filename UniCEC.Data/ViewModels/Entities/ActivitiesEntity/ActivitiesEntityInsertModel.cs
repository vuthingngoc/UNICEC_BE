using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.ActivitiesEntity
{
    public class ActivitiesEntityInsertModel
    {
        [JsonPropertyName("competition_activity_id")]
        public int CompetitionActivityId { get; set; }
        
        [JsonPropertyName("list_activities_entities")]
        public List<AddActivitiesEntity> ListActivitiesEntities { get; set; }

        //---------Author to check user is Leader of Club and Collaborate in Copetition       
        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }
    }
}
