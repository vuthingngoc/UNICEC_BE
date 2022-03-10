using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.AreaRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Area;

namespace UNICS.Business.Services.AreaSvc
{
    public class AreaSvc : IAreaSvc
    {
        private IAreaRepo _areaRepo;

        public AreaSvc(IAreaRepo areaRepo)
        {
            _areaRepo = areaRepo;
        }

        public PagingResult<ViewArea> getAll()
        {
            throw new NotImplementedException();
        }

        public ViewArea getByCampusId(int id)
        {
            throw new NotImplementedException();
        }
    }
}
