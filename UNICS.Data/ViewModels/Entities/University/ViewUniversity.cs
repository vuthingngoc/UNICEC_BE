using System;
using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.University
{
    public class ViewUniversity
    {
        public int Id { get; set; }
        [JsonPropertyName("uni_code")]
        public string UniCode { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public DateTime? Founding { get; set; }
        public DateTime? Openning { get; set; }
        public DateTime? Closing { get; set; }
        public int? Status { get; set; }
    }
}
