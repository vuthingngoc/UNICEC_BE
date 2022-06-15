using System;
using System.Text.Json.Serialization;
using UniCEC.Data.Enum;
using UniCEC.Data.ViewModels.Entities.ActivitiesEntity;

namespace UniCEC.Data.ViewModels.Entities.CompetitionActivity
{
    public class CompetitionActivityInsertModel
    {

        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonPropertyName("seeds_point")]
        public double SeedsPoint { get; set; }     
        public DateTime Ending { get; set; }

        public PriorityStatus Priority { get; set; }    

        //Add activities entity
        public AddActivitiesEntity? ActivitiesEntity { get; set; }

        //---------Author to check user is Leader of Club

        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }


    }
}
