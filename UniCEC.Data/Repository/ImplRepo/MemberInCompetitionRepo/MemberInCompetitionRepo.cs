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
using UniCEC.Data.ViewModels.Entities.MemberInCompetition;

namespace UniCEC.Data.Repository.ImplRepo.MemberInCompetitionRepo
{
    public class MemberInCompetitionRepo : Repository<MemberInCompetition>, IMemberInCompetitionRepo
    {
        public MemberInCompetitionRepo(UniCECContext context) : base(context)
        {
        }


        public bool CheckValidManagerByUser(int competitionId, int userId, int? competitionRoleId)
        {
            var query = from mic in context.MemberInCompetitions
                        join m in context.Members on mic.MemberId equals m.Id
                        join c in context.Clubs on m.ClubId equals c.Id
                        //join cic in context.CompetitionInClubs on c.Id equals cic.ClubId
                        where mic.CompetitionId.Equals(competitionId) && m.UserId.Equals(userId) && mic.Status.Equals(true) 
                        select new { mic, m, c/*, cic*/ };

            if (competitionRoleId.HasValue) query = query.Where(selector => selector.mic.CompetitionRoleId.Equals(competitionRoleId));

            return query.Any();
        }

        public async Task<PagingResult<ViewMemberInCompetition>> GetAllManagerCompOrEve(MemberInCompetitionRequestModel request)
        {
            //
            List<ViewMemberInCompetition> list_viewMembersInCompetition = new List<ViewMemberInCompetition>();

            //Member in Competition
            var query = from mic in context.MemberInCompetitions
                        where mic.CompetitionId == request.CompetitionId
                        select mic;

            int totalCount = await query.CountAsync();

            List<MemberInCompetition> memberInCompetitions = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

            foreach (MemberInCompetition memberInCompetition in memberInCompetitions)
            {
                
                ViewMemberInCompetition vmic = new ViewMemberInCompetition()
                {
                    Id = memberInCompetition.Id,
                    CompetitionRoleId = memberInCompetition.CompetitionRoleId,
                    CompetitionRoleName = memberInCompetition.CompetitionRole.RoleName,
                    MemberId = memberInCompetition.MemberId,
                    FullName = memberInCompetition.Member.User.Fullname,
                    Status = memberInCompetition.Status
                };

                list_viewMembersInCompetition.Add(vmic);
            }

            return (list_viewMembersInCompetition.Count > 0) ? new PagingResult<ViewMemberInCompetition>(list_viewMembersInCompetition, totalCount, request.CurrentPage, request.PageSize) : null;

        }

        public async Task<List<MemberInCompetition>> GetAllManagerCompOrEve(int competitionId)
        {
            var query = from mic in context.MemberInCompetitions
                        where mic.CompetitionId.Equals(competitionId)
                        select mic;

            return (query.Any()) ? await query.ToListAsync() : null;
        }

        public async Task<MemberInCompetition> GetMemberInCompetition(int competitionId, int memberId)
        {
            MemberInCompetition mem = await (from mic in context.MemberInCompetitions
                                             where mic.MemberId == memberId && mic.CompetitionId == competitionId && mic.Status == true
                                             select mic).FirstOrDefaultAsync() ;

            return (mem != null) ? mem : null;
        }
    }
}
