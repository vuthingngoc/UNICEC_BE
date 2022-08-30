using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.Competition
{
    public class AdminUniUpdateCompetitionStatusModel
    {
        public int Id { get; set; }

        public CompetitionStatus? Status { get; set; }
    }
}
