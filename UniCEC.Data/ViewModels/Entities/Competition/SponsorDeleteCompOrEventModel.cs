using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniCEC.Data.ViewModels.Entities.Competition
{
    public class SponsorDeleteCompOrEventModel
    {
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
    }
}
