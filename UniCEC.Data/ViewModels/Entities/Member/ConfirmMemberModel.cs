using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.Member
{
    public class ConfirmMemberModel
    {
        [JsonPropertyName("member_id")]
        public int MemberId { get; set; }
        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }
        public MemberStatus Status { get; set; }
    }
}
