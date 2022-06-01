using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.CompetitionActivity
{
    public class CompetitionActivityDeleteModel
    {
        [JsonPropertyName("club_activity_id")]
        public int ClubActivityId { get; set; }
        
        //---------Author to check user is Leader of Club and Collaborate in Copetition
        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }
        [JsonPropertyName("term_id")]
        public int TermId { get; set; }
    }
}
