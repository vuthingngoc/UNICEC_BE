using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.SponsorInCompetition
{
    public class SponsorInCompetitionInsertModel
    {
        public int CompetitionId { get; set; }
        [JsonPropertyName("sponsor_id_collaborate")]
        public int SponsorIdCollaborate { get; set; }
    }
}
