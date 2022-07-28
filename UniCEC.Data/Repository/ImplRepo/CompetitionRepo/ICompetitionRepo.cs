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
        public Task<PagingResult<ViewCompetition>> GetCompOrEve(CompetitionRequestModel request, int universityId);
        
        //Get top EVENT or COMPETITION by Status
        public Task<List<ViewCompetition>> GetTopCompOrEve(int clubId, bool? Event/*, CompetitionStatus? status*/, CompetitionScopeStatus? scope, int Top);
        
        //Get Comp by Admin Uni
        public Task<PagingResult<ViewCompetition>> GetCompOrEveByAdminUni(AdminUniGetCompetitionRequestModel request, int universityId);

        //Get Competition by SeedCode
        public Task<Competition> GetCompetitionBySeedsCode(string seedsCode);

        //Get Competition - UnAuthorize
        public Task<PagingResult<ViewCompetition>> GetCompOrEveUnAuthorize(CompetitionUnAuthorizeRequestModel request,List<CompetitionStatus> listCompetitionStatus);

        //
        public Task<PagingResult<ViewCompetition>> GetCompOrEveStudentJoin(PagingRequest request, int userId);

        //
        public Task<PagingResult<ViewCompetition>> GetCompOrEveStudentIsAssignedTask(PagingRequest request, int clubId, int userId);
        
        // Nhat
        public Task<CompetitionScopeStatus> GetScopeCompetition(int id);
        public Task<bool> CheckExisteUniInCompetition(int universityId, int competitionId);
        public Task<bool> CheckExistedCompetition(int competitionId);
    }
}
