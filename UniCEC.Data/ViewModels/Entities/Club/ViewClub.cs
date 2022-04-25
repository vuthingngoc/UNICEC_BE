using System;
using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.Club
{
    public class ViewClub
    {
        public int Id { get; set; }
        [JsonPropertyName("university_id")]
        public int UniversityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonPropertyName("total_member")]
        public int TotalMember { get; set; }
        public DateTime Founding { get; set; }
        public bool Status { get; set; }

    }
}
