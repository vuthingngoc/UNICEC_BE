using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.User
{
    public class ViewUser
    {
        public int Id { get; set; }
        [JsonPropertyName("role_id")]
        public int RoleId { get; set; }
        [JsonPropertyName("sponsor_id")]
        public int SponsorId { get; set; }
        [JsonPropertyName("university_id")]
        public int? UniversityId { get; set; }
        [JsonPropertyName("major_id")]
        public int? MajorId { get; set; }
        [JsonPropertyName("user_id")]
        public string UserCode { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public UserStatus Status { get; set; }
        public string Dob { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        [JsonPropertyName("is_online")]
        public bool IsOnline { get; set; }
    }
}
