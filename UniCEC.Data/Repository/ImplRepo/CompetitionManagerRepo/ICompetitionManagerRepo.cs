using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.ICompetitionManagerRepo
{
    public interface ICompetitionManagerRepo : IRepository<CompetitionManager>
    {
        //Check Condititon in Competition Service
        //public Task<CompetitionManager> GetManagerInCompetitionManager(int CompetitionId, int ClubId, int MemberId);

        //Get all records have all in CompetitionManger with MemberId
        public Task<CompetitionManager> GetMemberInCompetitionManager(int competitionId, int memberId, int clubId);
        public bool CheckValidManagerByUser(int competitionId, int userId);
       
    }
}
