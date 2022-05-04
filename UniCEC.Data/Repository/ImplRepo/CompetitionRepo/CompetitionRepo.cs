using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using Microsoft.EntityFrameworkCore;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Competition;
using UniCEC.Data.RequestModels;
using System.Linq;
using System.Collections.Generic;
using UniCEC.Data.Enum;
using System;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionRepo
{
    public class CompetitionRepo : Repository<Competition>, ICompetitionRepo
    {
        public CompetitionRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<bool> CheckExistCode(string code)
        {
            bool check = false;
            Competition competition = await context.Competitions.FirstOrDefaultAsync(x => x.SeedsCode.Equals(code));
            if (competition != null)
            {
                check = true;
                return check;
            }
            return check;
        }

        //Get EVENT or COMPETITION by conditions
        public async Task<PagingResult<ViewCompetition>> GetCompOrEve(CompetitionRequestModel request)
        {
            //
            var query = from cic in context.CompetitionInClubs
                        where cic.ClubId == request.ClubId
                        from comp in context.Competitions
                        where cic.CompetitionId == comp.Id
                        select comp;

            //status
            if (request.Status.HasValue) query = query.Where(comp => comp.Status == request.Status);
            //Public
            if (request.Public.HasValue) query = query.Where(comp => comp.Public == request.Public);
            //Serach Event
            if (request.Event.HasValue)
            {
                if (request.Event.Value == true) query = query.Where(comp => comp.NumberOfGroup == 0);
            }
            //
            int totalCount = query.Count();
            //
            List<ViewCompetition> Competitions = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).Select(x => new ViewCompetition()
            {
                Id = x.Id,
                CompetitionTypeId = x.CompetitionTypeId,
                Address = x.Address,
                NumberOfGroup = x.NumberOfGroup,
                NumberOfParticipation = x.NumberOfParticipation,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                StartTimeRegister = x.StartTimeRegister,
                EndTimeRegister = x.EndTimeRegister,
                SeedsCode = x.SeedsCode,
                SeedsPoint = x.SeedsPoint,
                SeedsDeposited = x.SeedsDeposited,
                Public = x.Public,
                Status = x.Status,
                View = x.View
            }).ToListAsync();

            return (Competitions.Count != 0) ? new PagingResult<ViewCompetition>(Competitions, totalCount, request.CurrentPage, request.PageSize) : null;

        }

        //Get top 3 EVENT or COMPETITION by Status
        //gần ngày hiện tại
        public async Task<List<ViewCompetition>> GetTop3CompOrEve_Status(bool? Event, CompetitionStatus? Status, bool? Public)
        {
            //format date
            DateTime now = DateTime.Now;

            var query = from comp in context.Competitions
                        where comp.StartTime >= now
                        orderby comp.StartTime 
                        select comp;

            //Serach Event
            if (Event.HasValue)
            {
                if (Event.Value == true) query = (IOrderedQueryable<Competition>)query.Where(comp => comp.NumberOfGroup == 0);
            }
            //Public
            if (Public.HasValue) query = (IOrderedQueryable<Competition>)query.Where(comp => comp.Public == Public);
            //Status
            if (Status.HasValue) query = (IOrderedQueryable<Competition>)query.Where(comp => comp.Status == Status);

            List<ViewCompetition> competitions = await query.Take(3).Select(x => new ViewCompetition()
            {
                Id = x.Id,
                CompetitionTypeId = x.CompetitionTypeId,
                Address = x.Address,
                NumberOfGroup = x.NumberOfGroup,
                NumberOfParticipation = x.NumberOfParticipation,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                StartTimeRegister = x.StartTimeRegister,
                EndTimeRegister = x.EndTimeRegister,
                SeedsCode = x.SeedsCode,
                SeedsPoint = x.SeedsPoint,
                SeedsDeposited = x.SeedsDeposited,
                Public = x.Public,
                Status = x.Status,
                View = x.View
            }).ToListAsync();

            return (competitions.Count > 0) ? competitions : null;
        }
    }
}
