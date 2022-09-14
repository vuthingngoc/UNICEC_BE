using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Entities.CompetitionRoundType;

namespace UniCEC.Business.Services.CompetitionRoundTypeSvc
{
    public interface ICompetitionRoundTypeService
    {
        public Task<ViewCompetitionRoundType> GetById(int id, string token);
        public Task<List<ViewCompetitionRoundType>> GetByConditions(CompetitionRoundTypeRequestModel request, string token);
        public Task<ViewCompetitionRoundType> Insert(CompetitionRoundTypeInsertModel model, string token);
        public Task Update(CompetitionRoundTypeUpdateModel model, string token);
        public Task Delete(int id, string token);
    }
}
