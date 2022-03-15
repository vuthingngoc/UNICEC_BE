using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.Role
{
    public class RoleInsertModel
    {
        [JsonPropertyName("role_name")]
        public string RoleName { get; set; }
    }
}
