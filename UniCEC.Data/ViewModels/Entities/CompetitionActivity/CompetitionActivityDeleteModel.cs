using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.CompetitionActivity
{
    public class CompetitionActivityDeleteModel
    {
        [JsonPropertyName("competition_activity_id"), BindRequired]
        public int CompetitionActivityId { get; set; }
        
        //---------Author to check user is Leader of Club and Collaborate in Copetition
        [JsonPropertyName("club_id"), BindRequired]
        public int ClubId { get; set; }
       
    }
}
