using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniCEC.Data.ViewModels.Entities.CompetitionInDepartment
{
    public class CompetitionInDepartmentInsertModel
    {
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }

        //---------List DepartmentID belong to University Insert in Competition
        [JsonPropertyName("list_department_id")]
        public List<int> ListDepartmentId { get; set; }

        //---------Author to check user is Leader of Club and Collaborate in Copetition       
        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }       
    }
}
