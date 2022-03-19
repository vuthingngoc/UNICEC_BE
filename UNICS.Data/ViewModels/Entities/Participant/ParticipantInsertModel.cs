using System;
using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.Participant
{
    public class ParticipantInsertModel
    {
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
        [JsonPropertyName("member_id")]
        public int? MemberId { get; set; }
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
        [JsonPropertyName("register_time")]
        public DateTime RegisterTime { get; set; }
    }
}
