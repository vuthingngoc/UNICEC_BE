using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities
{
    public class MemberInCompetitionInsertModel
    {
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }

        [JsonPropertyName("member_id")]
        public int MemberId { get; set; }

        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }
    }
}
