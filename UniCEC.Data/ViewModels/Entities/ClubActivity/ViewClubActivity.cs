using System;
using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.ClubActivity
{
    public class ViewClubActivity
    {
        public int Id { get; set; }
        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonPropertyName("seeds_point")]
        public double SeedsPoint { get; set; }
        [JsonPropertyName("seeds_code")]
        public string SeedsCode { get; set; }
        public DateTime Beginning { get; set; }
        public DateTime Ending { get; set; }
        [JsonPropertyName("create_time")]
        public DateTime CreateTime { get; set; }
        public ClubActivityStatus Status { get; set; }
        [JsonPropertyName("num_of_member")]
        public int NumOfMember { get; set; }
    }
}
