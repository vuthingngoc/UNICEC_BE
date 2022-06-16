using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.CompetitionRound;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionRoundRepo
{
    public class CompetitionRoundRepo : Repository<CompetitionRound>, ICompetitionRoundRepo
    {
        public CompetitionRoundRepo(UniCECContext context) : base(context)
        {
        }

        public Task<PagingResult<ViewCompetitionRound>> GetByConditions(CompetitionRoundRequestModel model)
        {
            throw new NotImplementedException();
        }

        public Task<ViewCompetitionRound> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
