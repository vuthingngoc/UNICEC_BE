using Microsoft.EntityFrameworkCore;
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
    public class CompetitionManagerRepo : Repository<CompetitionManager>, ICompetitionManagerRepo
    {
        public CompetitionManagerRepo(UniCECContext context) : base(context)
        {
        }

        public bool CheckValidManagerByUser(int competitionId, int userId)
        {
            var query = from cm in context.CompetitionManagers
                        join m in context.Members on cm.MemberId equals m.Id
                        join c in context.Clubs on m.ClubId equals c.Id
                        join cic in context.CompetitionInClubs on c.Id equals cic.ClubId
                        where cic.CompetitionId.Equals(competitionId) && m.UserId.Equals(userId)
                        select new {cm, m, c, cic};
            
            return query.Any();
        }

        public async Task<PagingResult<ViewCompetitionManager>> GetAllManagerCompOrEve(CompetitionManagerRequestModel request)
        {
            //
            List<ViewCompetitionManager> list_viewCompetitionManagers = new List<ViewCompetitionManager>();

            //CompetitionManager
            var query = from cic in context.CompetitionInClubs
                        where cic.CompetitionId == request.CompetitionId
                        from cm in context.CompetitionManagers
                        where cm.CompetitionInClubId == cic.Id 
                        select cm;

            int totalCount = await query.CountAsync();

            List<CompetitionManager> list_compeManager = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

            foreach (CompetitionManager competitionManager in list_compeManager)
            {
                //Lấy RoleName
                CompetitionRole competitionRole = await (from cr in context.CompetitionRoles
                                                        where cr.Id == competitionManager.CompetitionRoleId
                                                        select cr).FirstOrDefaultAsync();

                //Lấy FullName
                User user = await (from m in context.Members
                                   where m.Id == competitionManager.MemberId
                                   from us in context.Users
                                   where us.Id == m.UserId
                                   select us).FirstOrDefaultAsync(); 

                ViewCompetitionManager vcm = new ViewCompetitionManager()
                {
                    Id = competitionManager.Id,
                    CompetitionInClubId = competitionManager.CompetitionInClubId,
                    CompetitionRoleId = competitionManager.CompetitionRoleId,
                    CompetitionRoleName = competitionRole.RoleName,
                    MemberId = competitionManager.MemberId,
                    FullName = user.Fullname,
                    Status = competitionManager.Status,
                };

                list_viewCompetitionManagers.Add(vcm);
            }

            return (list_viewCompetitionManagers.Count > 0) ? new PagingResult<ViewCompetitionManager>(list_viewCompetitionManagers, totalCount, request.CurrentPage, request.PageSize) : null;

        }

        public async Task<CompetitionManager> GetMemberInCompetitionManager(int CompetitionId, int MemberId, int ClubId)
        {
            var query = from cic in context.CompetitionInClubs
                        where cic.CompetitionId == CompetitionId && cic.ClubId == ClubId
                        from cm in context.CompetitionManagers
                        where cm.CompetitionInClubId == cic.Id && cm.MemberId == MemberId && cm.Status == true
                        select cm;

            CompetitionManager competitionManager = await query.FirstOrDefaultAsync();
            if (competitionManager != null)
            {
                return competitionManager;
            }
            return null;
        }
    }
}
