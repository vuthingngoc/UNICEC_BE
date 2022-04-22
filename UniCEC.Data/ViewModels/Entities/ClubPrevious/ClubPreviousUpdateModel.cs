using System;
using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.ClubPrevious
{
    public class ClubPreviousUpdateModel
    {
        public int Id { get; set; }
        [JsonPropertyName("club_role_id")]
        public int ClubRoleId { get; set; }
        [JsonPropertyName("start_time")]
        public DateTime StartTime { get; set; }
        [JsonPropertyName("end_time")]
        public DateTime? EndTime { get; set; }
        public string Year { get; set; }
        public ClubPreviousStatus Status { get; set; }
    }
}
