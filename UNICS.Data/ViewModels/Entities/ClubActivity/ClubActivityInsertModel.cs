using System;
using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.ClubActivity
{
    public class ClubActivityInsertModel
    {
        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonPropertyName("seeds_point")]
        public double SeedsPoint { get; set; }
        public DateTime Beginning { get; set; }
        public DateTime Ending { get; set; }
        [JsonPropertyName("num_of_member")]
        public int NumOfMember { get; set; }
        public int Status { get; set; }
    }
}
