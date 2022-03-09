using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Area;

namespace UNICS.Business.Services.AreaSvc
{
    public interface IAreaSvc
    {
        public PagingResult<ViewArea> getAll();
        public ViewArea getByCampusId(int id);
    }
}
