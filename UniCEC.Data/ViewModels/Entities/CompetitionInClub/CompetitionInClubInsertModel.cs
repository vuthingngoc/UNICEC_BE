using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.CompetitionInClub
{
    public class CompetitionInClubInsertModel
    {
        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
        //---------Author to check user is Leader of Club
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
        [JsonPropertyName("term_id")]
        public int TermId { get; set; }
    }
}
