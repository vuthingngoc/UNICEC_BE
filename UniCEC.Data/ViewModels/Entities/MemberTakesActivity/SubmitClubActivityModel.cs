using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniCEC.Data.ViewModels.Entities.MemberTakesActivity
{
    public class SubmitClubActivityModel
    {
        [JsonPropertyName("club_activity_id")]
        public int ClubActivityId { get; set; }
    }
}
