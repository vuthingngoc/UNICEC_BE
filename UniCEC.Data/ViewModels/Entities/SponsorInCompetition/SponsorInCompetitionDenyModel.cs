using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniCEC.Data.ViewModels.Entities.SponsorInCompetition
{
    public class SponsorInCompetitionDenyModel
    {
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
    }
}
