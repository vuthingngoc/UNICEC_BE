using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.User
{
    public class UserUpdateModel
    {
        public int Id { get; set; }       
        [JsonPropertyName("department_id")]
        public int? DepartmentId { get; set; }
        [JsonPropertyName("student_code")]
        public string StudentCode { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public UserStatus? Status { get; set; }
        public string Dob { get; set; }
        public string Description { get; set; }
        [JsonPropertyName("is_online")]
        public bool? IsOnline { get; set; }
    }
}
