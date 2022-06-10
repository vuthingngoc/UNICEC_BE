using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.SponsorInCompetition
{
    public class ViewSponsorInCompetition
    {
        public int Id { get; set; }
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
        [JsonPropertyName("sponsor_id")]
        public int SponsorId { get; set; }

        [JsonPropertyName("user_id")]
        public int UserId { get; set; }

        public string Email { get; set; }

        [JsonPropertyName("full_name")]
        public string Fullname { get; set; }    

        public SponsorInCompetitionStatus Status { get; set;}

    }
}
