using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniCEC.Data.ViewModels.Entities.ParticipantInTeam
{
    public class ParticipantInTeamUpdateModel
    {
        [JsonPropertyName("participant_in_team_id")]
        public int ParticipantInTeamId { get; set; }
    }
}
