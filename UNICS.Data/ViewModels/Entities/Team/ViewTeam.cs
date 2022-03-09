namespace UNICS.Data.ViewModels.Entities.Team
{
    public class ViewTeam
    {
        public int Id { get; set; }
        public int? CompetitionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? NumberOfStudentInTeam { get; set; }
        public string InvitedCode { get; set; }
    }
}
