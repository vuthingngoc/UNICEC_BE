using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Major;

namespace UniCEC.Data.Repository.ImplRepo.MajorRepo
{
    public interface IMajorRepo : IRepository<Major>
    {
        public Task<ViewMajor> GetById(int id, bool? status);
        public Task<PagingResult<ViewMajor>> GetByConditions(MajorRequestModel request);
        public Task<PagingResult<ViewMajor>> GetByCompetition(int competitionId, PagingRequest request);
        public Task<int> CheckDuplicatedName(string name);
        //
        public Task<bool> CheckMajor(List<int> listMajorId);
        public Task<bool> CheckMajorBelongToUni(List<int> listMajorId, int universityId); 
    }
}
