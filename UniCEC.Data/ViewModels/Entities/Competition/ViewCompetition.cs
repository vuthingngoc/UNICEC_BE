using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.Competition
{
    public class ViewCompetition
    {
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("competition_type_id")]
        public int CompetitionTypeId { get; set; }
        [JsonPropertyName("competition_type_name")]
        public string CompetitionTypeName { get; set; }
        [JsonPropertyName("create_time")]
        public DateTime CreateTime { get; set; }
        [JsonPropertyName("is_sponsor")]
        public bool IsSponsor { get; set; }
        public bool Public { get; set; }      
        public int View { get; set; }

        //-------------ADD Field           
        [JsonPropertyName("clubs_in_competition")]
        public List<ViewClubInComp> ClubInCompetition { get; set; }

        public List<ViewDeparmentInComp> DepartmentInCompetition { get; set; }

    }
}
