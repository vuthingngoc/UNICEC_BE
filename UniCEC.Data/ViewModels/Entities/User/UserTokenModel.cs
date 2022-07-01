
using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.User
{
    public class UserTokenModel
    {
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Fullname { get; set; }
        [JsonPropertyName("university_id")]
        public int UniversityId { get; set; }
        [JsonPropertyName("role_id")]
        public int RoleId { get; set; }
        [JsonPropertyName("role_name")]
        public string RoleName { get; set; }
        public string Avatar { get; set; }
        public UserStatus Status { get; set; }
        //public string Email { get; set; }
        //[JsonPropertyName("phone_number")]
        //public string PhoneNumber { get; set; }
    }
}
