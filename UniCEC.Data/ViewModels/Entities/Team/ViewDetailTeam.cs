using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.ViewModels.Entities.Participant;

namespace UniCEC.Data.ViewModels.Entities.Team
{
    public class ViewDetailTeam
    {
        [JsonPropertyName("team_id")]
        public int TeamId { get; set; }
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonPropertyName("invited_code")]
        public string InvitedCode { get; set; }
        public TeamStatus Status { get; set; }

        [JsonPropertyName("list_participant")]
        public List<ViewParticipant> ListParticipant { get; set; } 

    }
}
