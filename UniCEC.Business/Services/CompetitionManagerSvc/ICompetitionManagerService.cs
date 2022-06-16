using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.CompetitionManager;

namespace UniCEC.Business.Services.CompetitionManagerSvc
{
    public interface ICompetitionManagerService
    {
        //Competition - Manager
        public Task<PagingResult<ViewCompetitionManager>> GetAllManagerCompOrEve(CompetitionManagerRequestModel model, string token);
        public Task<ViewCompetitionManager> AddMemberInCompetitionManager(CompetitionManagerInsertModel model, string token);
        public Task<bool> UpdateMemberRoleInCompetitionManager(CompetitionManagerUpdateRoleModel model, string token);
        public Task<bool> UpdateMemberStatusInCompetitionManager(CompetitionManagerUpdateStatusModel model, string token);
    }
}
