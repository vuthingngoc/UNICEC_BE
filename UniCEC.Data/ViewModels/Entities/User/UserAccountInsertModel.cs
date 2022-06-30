using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.User
{
    public class UserAccountInsertModel
    {
        [JsonPropertyName("university_id")]
        public int UniversityId { get; set; }
        public string Fullname { get; set; }
        public string Gender { get; set; }
        public string Dob { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
