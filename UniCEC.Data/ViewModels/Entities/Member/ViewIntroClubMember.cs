using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Member
{
    public class ViewIntroClubMember
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("club_role_id")]
        public int ClubRoleId { get; set; }
        [JsonPropertyName("club_role_name")]
        public string ClubRoleName { get; set; }
        public string Avatar { get; set; }
        [JsonPropertyName("is_online")]
        public bool IsOnline { get; set; }
    }
}
