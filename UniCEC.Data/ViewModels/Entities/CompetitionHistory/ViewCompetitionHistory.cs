using System;
using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.CompetitionHistory
{
    public class ViewCompetitionHistory
    {
        public int Id { get; set; }

        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }

        [JsonPropertyName("changer_id")]
        public int? ChangerId { get; set; }

        [JsonPropertyName("changer_name")]
        public string ChangerName { get; set; }

        [JsonPropertyName("change_date")]
        public DateTime ChangeDate { get; set; }

        public string Description { get; set; }

        public CompetitionStatus Status { get; set; }
    }
}
