using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.Participant
{
    public class ViewDetailParticipant
    {
        [JsonPropertyName("participant_id")]
        public int ParticipantId { get; set; }
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }

        [JsonPropertyName("student_id")]
        public int StudentId { get; set; }

        [JsonPropertyName("student_name")]
        public string Name { get; set; }

        [JsonPropertyName("student_avatar")]
        public string Avatar { get; set; }

        [JsonPropertyName("student_code")]
        public string StudentCode { get; set; }

        [JsonPropertyName("university_name")]
        public string UniversityName { get; set; }

        [JsonPropertyName("register_time")]
        public DateTime RegisterTime { get; set; }

        [JsonPropertyName("participant_in_team_id")]
        public int ParticipantInTeamId { get; set; }

        [JsonPropertyName("team_role_id")]
        public int TeamRoleId { get; set; }

        [JsonPropertyName("team_role_name")]
        public string TeamRoleName { get; set; }

        [JsonPropertyName("status")]
        public ParticipantInTeamStatus Status { get; set; }


    }
}
