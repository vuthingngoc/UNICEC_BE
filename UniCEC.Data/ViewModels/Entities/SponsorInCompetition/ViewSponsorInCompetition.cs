using System;
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

        [JsonPropertyName("create_time")]
        public DateTime CreateTime { get; set; }

        //Infomation of User belong to Sponsor
        public string Email { get; set; }

        [JsonPropertyName("full_name")]
        public string Fullname { get; set; }

        //Infomation of Sponsor
        [JsonPropertyName("sponsor_name")]
        public string SponsorName { get; set; }
        [JsonPropertyName("sponsor_logo")]
        public string SponsorLogo { get; set; }

        public SponsorInCompetitionStatus Status { get; set;}

    }
}
