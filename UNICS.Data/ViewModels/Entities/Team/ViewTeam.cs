using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.Team
{
    public class ViewTeam
    {
        public int Id { get; set; }
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonPropertyName("number_of_student_in_team")]
        public int NumberOfStudentInTeam { get; set; }
        [JsonPropertyName("invited_code")]
        public string InvitedCode { get; set; }
        public bool Status { get; set; }
    }
}
