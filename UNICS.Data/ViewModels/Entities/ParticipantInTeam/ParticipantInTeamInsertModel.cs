using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.ParticipantInTeam
{
    public class ParticipantInTeamInsertModel
    {
        public int TeamId { get; set; }
        [JsonPropertyName("participant_id")]
        public int ParticipantId { get; set; }
    }
}
