using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Role;

namespace UNICS.Business.Services.RoleSvc
{
    public interface IRoleSvc
    {
        Task<PagingResult<ViewRole>> GetAll(PagingRequest request);
        Task<ViewRole> GetById(int id);

        Task<bool> Insert(RoleInsertModel role);
        Task<bool> Update(ViewRole role);
        Task<bool> Delete(int id);
    }
}
