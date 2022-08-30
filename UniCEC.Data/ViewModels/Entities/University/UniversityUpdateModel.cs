using System;
using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.University
{
    public class UniversityUpdateModel
    {
        public int Id { get; set; }
        [JsonPropertyName("city_id")]
        public int CityId { get; set; } // for check valid data
        [JsonPropertyName("uni_code")]
        public string UniCode { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public DateTime? Founding { get; set; }
        public string Opening { get; set; }
        public string Closing { get; set; }
        public bool? Status { get; set; }
    }
}
