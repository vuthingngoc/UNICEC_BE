using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.CompetitionInClub
{
    public class CompetitionInClubInsertModel
    {
        [JsonPropertyName("club_id_collaborate")]
        public int ClubIdCollaborate { get; set; }
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
        //---------Author to check user is Leader of Club
        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }
        public int TermId { get; set; }
    }
}
