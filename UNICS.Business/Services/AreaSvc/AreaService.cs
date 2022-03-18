using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.AreaRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Area;

namespace UNICS.Business.Services.AreaSvc
{
    public class AreaService : IAreaService
    {
        private ICityRepo _areaRepo;

        public AreaService(ICityRepo areaRepo)
        {
            _areaRepo = areaRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewArea>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewArea> GetByCampusId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Insert(AreaInsertModel area)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewArea area)
        {
            throw new NotImplementedException();
        }
    }
}
