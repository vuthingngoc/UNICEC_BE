using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniCEC.Data.ViewModels.Entities.User
{
    public class UserUpdateWithJWTModel
    {
        public int Id { get; set; }
        [JsonPropertyName("university_id")]
        public int? UniversityId { get; set; }

        [JsonPropertyName("department_id")]
        public int? DepartmentId { get; set; }
        [JsonPropertyName("student_code")]
        public string StudentCode { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string Dob { get; set; }
        public string Description { get; set; }
    }
}
