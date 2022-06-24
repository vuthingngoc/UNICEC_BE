using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.CompetitionManager;

namespace UniCEC.Data.Repository.ImplRepo.ICompetitionManagerRepo
{
    public interface ICompetitionManagerRepo : IRepository<CompetitionManager>
    {
        //Check Condititon in Competition Service
        //public Task<CompetitionManager> GetManagerInCompetitionManager(int CompetitionId, int ClubId, int MemberId);

        //Get all records have all in CompetitionManger with MemberId TO CHECK
        public Task<CompetitionManager> GetMemberInCompetitionManager(int competitionId, int userId, int clubId);

        public bool CheckValidManagerByUser(int competitionId, int memberId);
        //Get All Manager in competition manager
        public Task<PagingResult<ViewCompetitionManager>> GetAllManagerCompOrEve(CompetitionManagerRequestModel request);
    }
}
