using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.CompetitionRound;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionRoundRepo
{
    public interface ICompetitionRoundRepo : IRepository<CompetitionRound>
    {
        public Task<ViewCompetitionRound> GetById(int id, bool? status);
        public Task<PagingResult<ViewCompetitionRound>> GetByConditions(CompetitionRoundRequestModel request);
    }
}
