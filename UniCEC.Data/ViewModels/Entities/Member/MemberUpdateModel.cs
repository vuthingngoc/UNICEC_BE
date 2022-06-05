using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.Member
{
    public class MemberUpdateModel
    {
        public int Id { get; set; }
        [JsonPropertyName("club_role_id")]
        public int ClubRoleId { get; set; }
    }
}
