using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.CompetitionActivity
{
    public class CompetitionActivityUpdateStatusModel
    {
        public int Id { get; set; }
        public CompetitionActivityStatus? Status { get; set; }
        //---------Author to check user is Leader of Club 
        [JsonPropertyName("club_id"), BindRequired]
        public int ClubId { get; set; }
    }
}
