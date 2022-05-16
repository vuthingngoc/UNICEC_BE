using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniCEC.Data.ViewModels.Entities.ClubActivity
{
    public class ClubActivityDeleteModel
    {
        [JsonPropertyName("club_activity_id")]
        public int ClubActivityId { get; set; }
        
        //---------Author to check user is Leader of Club and Collaborate in Copetition
        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }
        [JsonPropertyName("term_id")]
        public int TermId { get; set; }
    }
}
