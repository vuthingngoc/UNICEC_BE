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
    }
}
