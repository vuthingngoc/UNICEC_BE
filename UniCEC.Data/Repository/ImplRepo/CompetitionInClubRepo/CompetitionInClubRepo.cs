﻿using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using UniCEC.Data.Common;
using System.Collections.Generic;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionInClubRepo
{
    public class CompetitionInClubRepo : Repository<CompetitionInClub>, ICompetitionInClubRepo
    {
        public CompetitionInClubRepo(UniCECContext context) : base(context)
        {

        }

        //
        public async Task<bool> CheckDuplicateCreateCompetitionOrEvent(int clubId, int competitionId)
        {
            var query = from cic in context.CompetitionInClubs
                        where cic.ClubId == clubId && cic.CompetitionId == competitionId
                        select cic;
            int check = query.Count();
            if (check > 0)
            {
                //có nghĩa là đã tạo nó r
                return false;
            }
            else
            {
                //có nghĩa là chưa 
                return true;
            }
        }

        public async Task<List<int>> GetListClubId_In_Competition(int CompetitionId)
        {
            List<int> clubIdList = await (from cic in context.CompetitionInClubs
                                            where CompetitionId == cic.CompetitionId
                                            select cic.ClubId).ToListAsync();

            if (clubIdList.Count > 0)
            {
                return clubIdList;
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
