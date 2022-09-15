using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Entities.CompetitionRoundType;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionRoundTypeRepo
{
    public interface ICompetitionRoundTypeRepo : IRepository<CompetitionRoundType>
    {
        public Task<bool> CheckExistedType(int typeId);
        public Task<List<ViewCompetitionRoundType>> GetByConditions(CompetitionRoundTypeRequestModel request);
        public Task<ViewCompetitionRoundType> GetById(int id);
        public Task<bool> CheckDuplicatedCompetitionRoundType(string name);
    }
}
