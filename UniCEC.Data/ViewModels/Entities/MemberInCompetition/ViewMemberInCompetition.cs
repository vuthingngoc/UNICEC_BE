using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.MemberInCompetition
{
    public class ViewMemberInCompetition
    {       
        public int Id { get; set; }       
        [JsonPropertyName("competition_role_id")]
        public int CompetitionRoleId { get; set; }
        [JsonPropertyName("competition_role_name")]
        public string CompetitionRoleName { get; set; }
        [JsonPropertyName("member_id")]
        public int MemberId { get; set; }
        [JsonPropertyName("fullname")]
        public string FullName { get; set; }
        public bool Status { get; set; }
    }
}
