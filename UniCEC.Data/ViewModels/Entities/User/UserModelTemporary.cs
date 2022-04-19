
using System.Text.Json.Serialization;


namespace UniCEC.Data.ViewModels.Entities.User
{
    public class UserModelTemporary
    {
        [JsonPropertyName("role_id")]
        public int RoleId { get; set; }
        public string Email { get; set; }



    }
}
