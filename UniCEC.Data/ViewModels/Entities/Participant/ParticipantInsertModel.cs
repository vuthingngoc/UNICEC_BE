using System;
using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Participant
{
    public class ParticipantInsertModel
    {
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
        [JsonPropertyName("club_id")]
        public int? ClubId { get; set; }
    }
}
