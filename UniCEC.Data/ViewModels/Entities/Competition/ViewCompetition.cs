using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using UniCEC.Data.Enum;
using UniCEC.Data.ViewModels.Entities.CompetitionEntity;

namespace UniCEC.Data.ViewModels.Entities.Competition
{
    public class ViewCompetition
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

        //------------- ADD Field Competition Entity -> Object Array  
        [JsonPropertyName("competition_entities")]
        public List<ViewCompetitionEntity> CompetitionEntities { get; set; }

        //----------------------------CLUB MANAGER VIEW
        //-------------Total Activity
        [JsonPropertyName("total_competition_activity")]
        public int? totalCompetitionActivity { get; set; }

        //-------------Total Activity Complete
        [JsonPropertyName("total_competition_activity_completed")]
        public int? totalCompetitionActivityCompleted { get; set; }

       
    }
}
