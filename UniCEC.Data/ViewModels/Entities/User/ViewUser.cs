using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.User
{
    public class ViewUser
    {
        public int Id { get; set; }
        [JsonPropertyName("role_id")]
        public int RoleId { get; set; }
        [JsonPropertyName("university_id")]
        public int? UniversityId { get; set; }
        [JsonPropertyName("university_name")]
        public string UniversityName { get; set; }
        public string Fullname { get; set; }
        public string Avatar { get; set; }
        public string Gender { get; set; }
        [JsonPropertyName("department_id")]
        public int? DepartmentId { get; set; }
        [JsonPropertyName("department_name")]
        public string DepartmentName { get; set; }
        [JsonPropertyName("student_code")]
        public string StudentCode { get; set; }
        public string Email { get; set; }
        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }
        public UserStatus Status { get; set; }
        public string Dob { get; set; }
        public string Description { get; set; }
        [JsonPropertyName("is_online")]
        public bool IsOnline { get; set; }
    }
}
