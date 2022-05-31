using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniCEC.Data.ViewModels.Entities.CompetitionInDepartment
{
    public class ViewCompetitionInDepartment
    {
        [JsonPropertyName("competition_in_department_id")]
        public int Id { get; set; }
        [JsonPropertyName("department_id")]
        public int DepartmentId { get; set; }
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
    }
}
