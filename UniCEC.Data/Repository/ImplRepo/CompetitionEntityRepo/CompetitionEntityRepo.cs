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

        public async Task<bool> CheckSponsorStillInCompetition(int competitionId, int entityTypeId)
        {
            List<CompetitionEntity> competitionEntities = await (from ce in context.CompetitionEntities
                                                                 where ce.CompetitionId == competitionId && ce.EntityTypeId == entityTypeId
                                                                 select ce).ToListAsync();
            return (competitionEntities.Count > 0) ? true : false;
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

        public async Task<List<ViewCompetitionEntity>> GetCompetitionEntities(int competitionId)
        {
            List<ViewCompetitionEntity> competitionEntities = await (from ce in context.CompetitionEntities
                                                                     join e in context.EntityTypes on ce.EntityTypeId equals e.Id
                                                                     where ce.CompetitionId.Equals(competitionId)
                                                                     select new ViewCompetitionEntity()
                                                                     {
                                                                         Id = ce.Id,
                                                                         CompetitionId = competitionId,
                                                                         Description = ce.Description,
                                                                         Email = ce.Email,
                                                                         EntityTypeId = ce.EntityTypeId,
                                                                         EntityTypeName = e.Name,
                                                                         Name = ce.Name,
                                                                         Website = ce.Website,
                                                                         ImageUrl = ce.ImageUrl
                                                                     }).ToListAsync();

            return (competitionEntities.Count > 0) ? competitionEntities : null;
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
