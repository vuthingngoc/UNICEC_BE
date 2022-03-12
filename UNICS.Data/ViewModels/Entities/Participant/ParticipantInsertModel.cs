using System;
using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.Participant
{
    public class ParticipantInsertModel
    {
        [JsonPropertyName("team_id")]
        public int? TeamId { get; set; }
        [JsonPropertyName("student_id")]
        public int? StudentId { get; set; }
        [JsonPropertyName("register_time")]
        public DateTime? RegisterTime { get; set; }
    }
}
