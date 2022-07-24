using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.Competition
{
    public class CompetitionStatusUpdateModel
    {
        public int Id { get; set; }

        public CompetitionStatus? Status { get; set; }

        //---------Author to check user is Leader of Club and Collaborate in Copetition       
        [JsonPropertyName("club_id"), BindRequired]
        public int ClubId { get; set; }
    }
}
