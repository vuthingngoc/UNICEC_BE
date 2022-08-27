using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Entities.CompetitionEntity;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionEntityRepo
{
    public interface ICompetitionEntityRepo : IRepository<CompetitionEntity>
    {
        public Task<List<CompetitionEntity>> GetListCompetitionEntity(int competitionId);

        // Gwi
        public Task<List<ViewCompetitionEntity>> GetCompetitionEntities(int competitionId);

        public Task<bool> CheckSponsorStillInCompetition(int competitionId, int entityTypeId);

        public Task DeleteCompetitionEntity(int competitionEntityId);
    }
}
