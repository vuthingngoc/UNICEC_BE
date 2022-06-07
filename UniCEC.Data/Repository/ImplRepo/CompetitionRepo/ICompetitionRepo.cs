using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Competition;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionRepo
{
    public interface ICompetitionRepo : IRepository<Competition>
    {
        public Task<bool> CheckExistCode(string code);

        //Get EVENT or COMPETITION by conditions
        public Task<PagingResult<ViewCompetition>> GetCompOrEve(CompetitionRequestModel request);
        //Get top 3 EVENT or COMPETITION by Status
        public Task<List<ViewCompetition>> GetTop3CompOrEve(int? clubId , bool? Event, CompetitionStatus? status, CompetitionScopeStatus? scope);

        // Nhat
        public Task<CompetitionScopeStatus> GetScopeCompetition(int id);
    }
}
