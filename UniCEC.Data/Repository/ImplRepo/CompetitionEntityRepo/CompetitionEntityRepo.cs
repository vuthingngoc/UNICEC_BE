using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Entities.CompetitionEntity;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace UniCEC.Data.Repository.ImplRepo.CompetitionEntityRepo
{
    public class CompetitionEntityRepo : Repository<CompetitionEntity>, ICompetitionEntityRepo
    {
        public CompetitionEntityRepo(UniCECContext context) : base(context)
        {

        }

        public async Task DeleteCompetitionEntity(int competitionEntityId)
        {
            CompetitionEntity entity = await (from ce in context.CompetitionEntities
                                              where ce.Id == competitionEntityId
                                              select ce).FirstOrDefaultAsync();
            if (entity != null)
            {
                context.CompetitionEntities.Remove(entity);
                await Update();
            }
        }

        public async Task<List<CompetitionEntity>> GetListCompetitionEntity(int competitionId)
        {
            List<CompetitionEntity> competitionEntities = await (from ce in context.CompetitionEntities
                                                                 where ce.CompetitionId == competitionId
                                                                 select ce).ToListAsync();

            return (competitionEntities.Count > 0) ? competitionEntities : null;

        }
    }
}
