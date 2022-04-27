using System;
using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Club
{
    public class ClubUpdateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        [JsonPropertyName("total_member")]
        public int TotalMember { get; set; }
        public DateTime Founding { get; set; }
        public bool Status { get; set; }
    }
}
