using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.SponsorInCompetition
{
    public class SponsorInCompetitionInsertModel
    {
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }       
    }
}
