﻿using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using UniCEC.Data.Common;
using System.Collections.Generic;
using UniCEC.Data.ViewModels.Entities.Competition;
using UniCEC.Data.ViewModels.Entities.CompetitionInClub;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionInClubRepo
{
    public class CompetitionInClubRepo : Repository<CompetitionInClub>, ICompetitionInClubRepo
    {
        public CompetitionInClubRepo(UniCECContext context) : base(context)
        {

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

        public async Task<List<ViewClubInComp>> GetListClub_In_Competition(int CompetitionId)
        {
            List<CompetitionInClub> clubList = await (from cic in context.CompetitionInClubs
                                                      where CompetitionId == cic.CompetitionId
                                                      select cic).ToListAsync();

            List<ViewClubInComp> List_vcip = new List<ViewClubInComp>();

            if (clubList.Count > 0)
            {
                foreach (var competitionInClub in clubList)
                {
                    Club club = await (from c in context.Clubs
                                       where c.Id == competitionInClub.ClubId
                                       select c).FirstOrDefaultAsync();

                    ViewClubInComp vcip = new ViewClubInComp()
                    {
                        Id = club.Id,
                        Name = club.Name,
                        Image = club.Image,
                        Fanpage = club.ClubFanpage,
                        IsOwner = competitionInClub.IsOwner,
                    };

                    List_vcip.Add(vcip);
                }
                if (List_vcip.Count > 0)
                {
                    return List_vcip;
                }
            }

            return null;
        }

        // Nhat
        public async Task<int> GetTotalEventOrganizedByClub(int clubId)
        {
            var query = from cic in context.CompetitionInClubs
                        join c in context.Competitions on cic.CompetitionId equals c.Id
                        where cic.ClubId.Equals(clubId) && c.EndTime.Date >= new LocalTime().GetLocalTime().DateTime
                        select new { c };

            return await query.CountAsync();
        }


    }
}
