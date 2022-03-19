using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.Team
{
    public class TeamUpdateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonPropertyName("number_of_student_in_team")]
        public int NumberOfStudentInTeam { get; set; }
        public bool Status { get; set; }
    }
}
