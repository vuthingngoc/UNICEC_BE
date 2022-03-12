using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.Campus
{
    public class CampusUpdateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
        public string Address { get; set; }
        [JsonPropertyName("area_id")]
        public int? AreaId { get; set; }
    }
}
