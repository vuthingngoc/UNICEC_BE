using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.TeamInRound
{
    public class ParamsNextRoundModel
    {
        [JsonPropertyName("round_id")]
        public int RoundId { get; set; }
        public int Top { get; set; }
    }
}
