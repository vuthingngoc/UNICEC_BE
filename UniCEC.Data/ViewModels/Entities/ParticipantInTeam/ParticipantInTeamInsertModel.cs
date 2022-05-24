using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.ParticipantInTeam
{
    public class ParticipantInTeamInsertModel
    {
        [JsonPropertyName("invited_code")]
        public string InvitedCode { get; set; }
    }
}
