using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniCEC.Data.ViewModels.Entities.CompetitionInMajor
{
    public class CompetitionInMajorDeleteModel
    {
        [JsonPropertyName("competition_in_major_id"), BindRequired]
        public int CompetitionInMajorId { get; set; }

        //---------Author to check user is Leader of Club and Collaborate in Copetition       
        [JsonPropertyName("club_id"), BindRequired]
        public int ClubId { get; set; }
    }
}
