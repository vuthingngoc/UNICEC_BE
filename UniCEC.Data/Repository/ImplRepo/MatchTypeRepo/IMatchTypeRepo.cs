using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Entities.MatchType;

namespace UniCEC.Data.Repository.ImplRepo.MatchTypeRepo
{
    public interface IMatchTypeRepo : IRepository<MatchType>
    {
        public Task<bool> CheckExistedType(int typeId);
        public Task<List<ViewMatchType>> GetByConditions(MatchTypeRequestModel request);
        public Task<ViewMatchType> GetById(int id);
        public Task<bool> CheckDuplicatedMatchType(string name);
    }
}
