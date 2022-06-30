using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Entities.Major;

namespace UniCEC.Data.Repository.ImplRepo.MajorRepo
{
    public class MajorRepo : Repository<Major>, IMajorRepo
    {
        public MajorRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<PagingResult<ViewMajor>> GetByConditions(MajorRequestModel request)
        {
            var query = from m in context.Majors
                        select m;

            if (!string.IsNullOrEmpty(request.Name)) query = query.Where(major => major.Name.Contains(request.Name));

            if (request.Status.HasValue) query = query.Where(major => major.Status.Equals(request.Status.Value));

            int totalCount = query.Count();
            List<ViewMajor> majors = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                                        .Select(major => new ViewMajor()
                                                        {
                                                            Id = major.Id,
                                                            Name = major.Name,
                                                            Status = major.Status
                                                        }).ToListAsync();

            return (majors.Count > 0) ? new PagingResult<ViewMajor>(majors, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task<PagingResult<ViewMajor>> GetByCompetition(int competitionId, PagingRequest request)
        {
            var query = from cim in context.CompetitionInMajors
                        join m in context.Majors on cim.MajorId equals m.Id
                        where cim.CompetitionId.Equals(competitionId)
                        select m;

            int totalCount = query.Count();

            List<ViewMajor> majors = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                                            .Select(major => new ViewMajor()
                                                            {
                                                                Id = major.Id,
                                                                Name = major.Name,
                                                                Status = major.Status
                                                            }).ToListAsync();

            return (majors.Count > 0) ? new PagingResult<ViewMajor>(majors, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task<ViewMajor> GetById(int id, bool? status)
        {
            var query = from m in context.Majors
                        where m.Id.Equals(id)
                        select m;

            if (status.HasValue) query = query.Where(major => major.Status.Equals(status.Value));

            return await query.Select(major => new ViewMajor()
            {
                Id = major.Id,
                Name = major.Name,
                Status = major.Status,
            }).FirstOrDefaultAsync();
        }

        public async Task<int> CheckDuplicatedName(string name)
        {
            return await (from m in context.Majors
                          where m.Name.ToLower().Equals(name.ToLower())
                          select m.Id).FirstOrDefaultAsync();
        }

        // TA
        public async Task<bool> CheckMajor(List<int> listMajorId)
        {
            bool result = true;
            foreach (int majorId in listMajorId)
            {
                var query = await (from major in context.Majors
                                   where major.Id == majorId
                                   select major).FirstOrDefaultAsync();

                if (query == null)
                {
                    result = false;
                }
            }
            return result;
        }

        public async Task<bool> CheckMajorBelongToUni(List<int> listMajorId, int universityId)
        {
            bool result = true;
            foreach (int majorId in listMajorId)
            {
                var query = await (from d in context.Departments
                                   where d.UniversityId == universityId
                                   from m in context.Majors
                                   where m.Id == d.Id
                                   select m).FirstOrDefaultAsync();

                if (query == null)
                {
                    result = false;
                }
            }
            return result;
        }
    }
}
