using System;
using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.Campus
{
    public class ClubUpdateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonPropertyName("total_member")]
        public int TotalMember { get; set; }
        public DateTime Founding { get; set; }
        public bool Status { get; set; }
    }
}
