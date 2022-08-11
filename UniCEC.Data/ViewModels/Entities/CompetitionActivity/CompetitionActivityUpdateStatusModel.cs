using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
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
