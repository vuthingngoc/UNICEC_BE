using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.CompetitionInMajor
{
    public class CompetitionInMajorInsertModel
    {
        [JsonPropertyName("competition_id"), BindRequired]
        public int CompetitionId { get; set; }

        //---------List DepartmentID belong to University Insert in Competition
        [JsonPropertyName("list_major_id")]
        public List<int> ListMajorId { get; set; }

        //---------Author to check user is Leader of Club and Collaborate in Copetition       
        [JsonPropertyName("club_id"),  BindRequired]
        public int ClubId { get; set; }       
    }
}
