using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.CompetitionActivity;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionActivityRepo
{
    public interface ICompetitionActivityRepo : IRepository<CompetitionActivity>
    {

        //public Task<bool> CheckExistCode(string code);
        ////
        public Task<PagingResult<ViewCompetitionActivity>> GetListActivitiesByConditions(CompetitionActivityRequestModel conditions);

        //public Task<PagingResult<ViewCompetitionActivity>> GetListActivitiesByConditions2(CompetitionActivityRequestModel conditions);

        public Task<List<ViewProcessCompetitionActivity>> GetTopCompetitionActivity(int clubId, int topCompetition, int topCompetitionActivity);

        public Task<PagingResult<ViewCompetitionActivity>> GetListCompetitionActivitiesIsAssigned(PagingRequest request, int competitionId, PriorityStatus? priorityStatus, List<CompetitionActivityStatus> statuses,  string name, int userId);


        // Nhat
        public Task<int> GetTotalActivityByClub(int clubId);
    }
}
