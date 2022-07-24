using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.MemberInCompetition
{
    public class MemberInCompetitionUpdateModel
    {

        [JsonPropertyName("member_in_competition_id"), BindRequired]
        public int MemberInCompetitionId { get; set; }

        [JsonPropertyName("role_competition_id"), BindRequired]
        public int? RoleCompetitionId { get; set; }

        public bool? Status { get; set; }    

        //---------Author to check user is Leader of Club   
        [JsonPropertyName("club_id"), BindRequired]
        public int ClubId { get; set; }
    }
}
