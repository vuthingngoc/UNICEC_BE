using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.Role
{
    public class ViewRole
    {
        public int Id { get; set; }
        [JsonPropertyName("role_name")]
        public string RoleName { get; set; }
    }
}
