using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using UniCEC.Data.Common;
using System.Collections.Generic;
using UniCEC.Data.ViewModels.Entities.Competition;
using UniCEC.Data.ViewModels.Entities.CompetitionInClub;
using UniCEC.Data.Enum;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionInClubRepo
{
    public class CompetitionInClubRepo : Repository<CompetitionInClub>, ICompetitionInClubRepo
    {
        public CompetitionInClubRepo(UniCECContext context) : base(context)
        {

        }

        public async Task DeleteCompetitionInClub(int competitionInClubId)
        {
            CompetitionInClub competitionInClub = await (from cic in context.CompetitionInClubs
                                                         where cic.Id == competitionInClubId
                                                         select cic).FirstOrDefaultAsync();
            context.CompetitionInClubs.Remove(competitionInClub);
            await Update();
        }

        public async Task<ViewCompetitionInClub> GetCompetitionInClub(int clubId, int competitionId)
        {
            CompetitionInClub query = await (from cic in context.CompetitionInClubs
                                             where cic.CompetitionId == competitionId && cic.ClubId == clubId
                                             select cic).FirstOrDefaultAsync();

            if (query != null)
            {
                return new ViewCompetitionInClub()
                {
                    Id = query.Id,
                    CompetitionId = query.CompetitionId,
                    ClubId = query.ClubId,
                    IsOwner = query.IsOwner,
                };
            }
            else
            {
                return null;
            }
        }

        public async Task<List<ViewClubInComp>> GetListClubInCompetition(int competitionId)
        {
            List<CompetitionInClub> clubList = await (from cic in context.CompetitionInClubs
                                                      where competitionId == cic.CompetitionId
                                                      select cic).ToListAsync();

            var clubs = await (from c in context.Clubs
                               join cic in context.CompetitionInClubs on c.Id equals cic.ClubId
                               where cic.CompetitionId.Equals(competitionId)
                               select new ViewClubInComp()
                               {
                                   Id = cic.Id,
                                   ClubId = c.Id,
                                   Name = c.Name,
                                   Image = c.Image,
                                   Fanpage = c.ClubFanpage,
                                   IsOwner = cic.IsOwner
                               }).ToListAsync();

            return (clubs.Any()) ? clubs : null;

            //List<ViewClubInComp> List_vcip = new List<ViewClubInComp>();

            //if (clubList.Count > 0)
            //{
            //    foreach (var competitionInClub in clubList)
            //    {
            //        Club club = await (from c in context.Clubs
            //                           where c.Id == competitionInClub.ClubId
            //                           select c).FirstOrDefaultAsync();

            //        ViewClubInComp vcip = new ViewClubInComp()
            //        {
            //            Id = competitionInClub.Id,
            //            Name = club.Name,
            //            Image = club.Image,
            //            Fanpage = club.ClubFanpage,
            //            IsOwner = competitionInClub.IsOwner,
            //            ClubId = club.Id,
            //        };

            //        List_vcip.Add(vcip);
            //    }
            //    if (List_vcip.Count > 0)
            //    {
            //        return List_vcip;
            //    }
            //}

            //return null;
        }

        // Nhat
        public async Task<int> GetTotalEventOrganizedByClub(int clubId)
        {
            var query = from cic in context.CompetitionInClubs
                        join c in context.Competitions on cic.CompetitionId equals c.Id
                        where cic.ClubId.Equals(clubId) && !c.Status.Equals(CompetitionStatus.Cancel)
                        select c;

            return await query.CountAsync();
        }


    }
}
