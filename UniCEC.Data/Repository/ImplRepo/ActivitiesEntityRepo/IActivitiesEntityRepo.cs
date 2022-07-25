using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.ActivitiesEntityRepo
{
    public interface IActivitiesEntityRepo : IRepository<ActivitiesEntity>
    {
        public Task<List<ActivitiesEntity>> GetListActivitesEntityByCompetition(int competitionActivityId);

        public Task DeleteActivitiesEntity(int competitionActivityId);
    }
}
