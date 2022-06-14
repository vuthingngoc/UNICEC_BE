using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.SponsorInCompetition
{
    public class FeedbackSponsorInCompetitionModel
    {
        [JsonPropertyName("sponsor_in_competition_id")]
        public int SponsorInCompetitionId { get; set; }
        public string Feedback { get; set; }
        public SponsorInCompetitionStatus Status { get; set; }

        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }

    }
}
