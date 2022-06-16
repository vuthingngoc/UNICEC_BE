using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.CompetitionActivity
{
    public class CompetitionActivityDeleteModel
    {
        [JsonPropertyName("competition_activity_id")]
        public int CompetitionActivityId { get; set; }
        
        //---------Author to check user is Leader of Club and Collaborate in Copetition
        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }
       
    }
}
