using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Member
{
    public class ViewMember
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("club_role_id")]
        public int ClubRoleId { get; set; }
        [JsonPropertyName("club_role_name")]
        public string ClubRoleName { get; set; }
        public string Avatar { get; set; }
        public bool Status { get; set; }
    }
}
