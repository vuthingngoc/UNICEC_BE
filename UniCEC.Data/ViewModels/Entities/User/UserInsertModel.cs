using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.User
{
    public class UserInsertModel
    {
        [JsonPropertyName("role_id")]
        public int RoleId { get; set; }
        [JsonPropertyName("university_id")]
        public int? UniversityId { get; set; }
        [JsonPropertyName("major_id")]
        public int? MajorId { get; set; }
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Dob { get; set; }
        public string Description { get; set; }
    }
}
