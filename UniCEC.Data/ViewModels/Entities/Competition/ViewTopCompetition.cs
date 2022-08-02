using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.Competition
{
    public class ViewTopCompetition
    {
        public int Id { get; set; }
        [JsonPropertyName("university_id")]
        public int UniversityId { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("competition_type_id")]
        public int CompetitionTypeId { get; set; }
        [JsonPropertyName("competition_type_name")]
        public string CompetitionTypeName { get; set; }
        [JsonPropertyName("create_time")]
        public DateTime CreateTime { get; set; }

        [JsonPropertyName("start_time")]
        public DateTime StartTime { get; set; }

        [JsonPropertyName("is_sponsor")]
        public bool IsSponsor { get; set; }

        [JsonPropertyName("is_event")]
        public bool IsEvent { get; set; }

        public CompetitionScopeStatus Scope { get; set; }
        public CompetitionStatus Status { get; set; }
        public int View { get; set; }

        //-------------ADD Field           
        [JsonPropertyName("clubs_in_competition")]
        public List<ViewClubInComp> ClubInCompetition { get; set; }
        [JsonPropertyName("majors_in_competition")]
        public List<ViewMajorInComp> MajorInCompetition { get; set; }
    }
}
