using System;
using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

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
        public bool IsOnline { get; set; }       
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public MemberStatus Status { get; set; }
        public int TermId { get; set; }
        public string TermName { get; set; }
    }
}
