using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Entities.ActivitiesEntity;

namespace UniCEC.Business.Services.ActivitiesEntitySvc
{
    public interface IActivitiesEntityService
    {
        public Task<List<ViewActivitiesEntity>> AddActivitiesEntity(ActivitiesEntityInsertModel model, string token);

        public Task<bool> DeleteActivitiesEntity(int activitiesEntityId, int clubId, string token);
    }
}
