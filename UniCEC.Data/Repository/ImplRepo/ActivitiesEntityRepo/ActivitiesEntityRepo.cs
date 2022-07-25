using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.ActivitiesEntityRepo
{
    public class ActivitiesEntityRepo : Repository<ActivitiesEntity>, IActivitiesEntityRepo
    {
        public ActivitiesEntityRepo(UniCECContext context) : base(context)
        {
        }

        public async Task DeleteActivitiesEntity(int competitionActivityId)
        {
            List<ActivitiesEntity> activitiesEntities = await (from ae in context.ActivitiesEntities
                                                       where ae.CompetitionActivityId == competitionActivityId
                                                       select ae).ToListAsync();
            foreach (ActivitiesEntity ae in activitiesEntities)
            {
                context.ActivitiesEntities.Remove(ae);
            }
            
            await Update();
        }

        public async Task<List<ActivitiesEntity>> GetListActivitesEntityByCompetition(int competitionActivityId)
        {
            List<ActivitiesEntity> activitiesEntities = await (from ca in context.CompetitionActivities
                                                               where ca.Id == competitionActivityId
                                                               from ae in context.ActivitiesEntities
                                                               where ae.CompetitionActivityId == ca.Id
                                                               select ae).ToListAsync();

            return (activitiesEntities.Count > 0) ? activitiesEntities : null;
        }
    }
}
