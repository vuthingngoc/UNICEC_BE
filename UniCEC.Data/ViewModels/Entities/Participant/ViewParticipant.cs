using System;
using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Participant
{
    public class ViewParticipant
    {
        public int Id { get; set; }
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
        
        [JsonPropertyName("student_id")]
        public int StudentId { get; set; }

        [JsonPropertyName("student_avatar")]
        public string Avatar { get; set; }

        [JsonPropertyName("student_name")]
        public string StudentName { get; set; }

        [JsonPropertyName("student_code")]
        public string StudentCode { get; set; }

        [JsonPropertyName("register_time")]
        public DateTime RegisterTime { get; set; }

        public bool Attendance { get; set; }
    }
}
