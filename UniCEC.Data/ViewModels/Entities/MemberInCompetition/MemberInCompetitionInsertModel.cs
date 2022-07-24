using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities
{
    public class MemberInCompetitionInsertModel
    {
        [JsonPropertyName("competition_id"), BindRequired]
        public int CompetitionId { get; set; }

        [JsonPropertyName("member_id"), BindRequired]
        public int MemberId { get; set; }

        [JsonPropertyName("club_id"), BindRequired]
        public int ClubId { get; set; }
    }
}
