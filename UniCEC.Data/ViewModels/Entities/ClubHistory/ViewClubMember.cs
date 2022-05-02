using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.ClubHistory
{
    public class ViewClubMember
    {
        [JsonPropertyName("member_id")]
        public int MemberId { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("club_role_name")]
        public string ClubRoleName { get; set; }

        [JsonPropertyName("term_id")]
        public int TermId { get; set; }
    }
}
