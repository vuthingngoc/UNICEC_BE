using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniCEC.Data.ViewModels.Entities.CompetitionInClub
{
    public class CompetitionInClubDeleteModel
    {
        [JsonPropertyName("competition_in_club_id"), BindRequired]
        public int CompetitionInClubId { get; set; }
        //---------Author to check user is Leader of Club
        [JsonPropertyName("club_id"), BindRequired]
        public int ClubId { get; set; }
    }
}
