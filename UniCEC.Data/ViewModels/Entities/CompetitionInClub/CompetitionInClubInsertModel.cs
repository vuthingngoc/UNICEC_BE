using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.CompetitionInClub
{
    public class CompetitionInClubInsertModel
    {
        [JsonPropertyName("club_id_collaborate"), BindRequired]
        public int ClubIdCollaborate { get; set; }
        [JsonPropertyName("competition_id"), BindRequired]
        public int CompetitionId { get; set; }
        //---------Author to check user is Leader of Club
        [JsonPropertyName("club_id"), BindRequired]
        public int ClubId { get; set; }       
    }
}
