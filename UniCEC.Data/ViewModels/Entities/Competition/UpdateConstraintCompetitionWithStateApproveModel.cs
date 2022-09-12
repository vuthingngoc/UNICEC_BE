using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.Competition
{
    public class UpdateConstraintCompetitionWithStateApproveModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public CompetitionScopeStatus? Scope { get; set; }

        //Comment why you update
        public string Comment { get; set; }

        //---------Author to check user is Leader of Club and Collaborate in Copetition
        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }
    }
}
