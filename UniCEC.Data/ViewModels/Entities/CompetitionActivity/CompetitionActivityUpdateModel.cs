using System;
using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.CompetitionActivity
{
    public class CompetitionActivityUpdateModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        [JsonPropertyName("seeds_point")]
        public double? SeedsPoint { get; set; }       
        public DateTime? Ending { get; set; }
        public PriorityStatus? Priority { get; set; }
        public CompetitionActivityStatus? Status { get; set; }

        //---------Author to check user is Leader of Club 
        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }        
    }
}
