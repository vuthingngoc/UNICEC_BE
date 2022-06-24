using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniCEC.Data.ViewModels.Entities.CompetitionManager
{
    public class ViewCompetitionManager
    {
        [JsonPropertyName("competition_manager_id")]
        public int Id { get; set; }
        [JsonPropertyName("competition_in_club_id")]
        public int CompetitionInClubId { get; set; }
        [JsonPropertyName("competition_role_id")]
        public int CompetitionRoleId { get; set; }
        [JsonPropertyName("competition_role_name")]
        public string CompetitionRoleName { get; set; }
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
        [JsonPropertyName("fullname")]
        public string FullName { get; set; }
        public bool Status { get; set; }
    }
}
