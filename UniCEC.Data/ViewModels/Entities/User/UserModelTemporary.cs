
using System.Text.Json.Serialization;


namespace UniCEC.Data.ViewModels.Entities.User
{
    public class UserModelTemporary
    {
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Fullname { get; set; }
        [JsonPropertyName("role_id")]
        public int RoleId { get; set; }
        [JsonPropertyName("role_name")]
        public string RoleName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
    }
}
