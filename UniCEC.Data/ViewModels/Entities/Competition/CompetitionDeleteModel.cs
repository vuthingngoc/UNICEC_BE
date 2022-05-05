using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniCEC.Data.ViewModels.Entities.Competition
{
    public class CompetitionDeleteModel
    {

        public int Id { get; set; }
        //---------Author to check user is Leader of Club and Collaborate in Copetition
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }
        [JsonPropertyName("term_id")]
        public int TermId { get; set; }
    }
}
