﻿using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UniCEC.Data.ViewModels.Common;
using System;

namespace UniCEC.Data.Repository.ImplRepo.MajorRepo
{
    public class MajorRepo : Repository<Major>, IMajorRepo
    {
        public MajorRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<PagingResult<Major>> GetByCondition(MajorRequestModel request)
        {
            var query = from m in context.Majors
                        select new { m };
            if (!string.IsNullOrEmpty(request.Name)) query = query.Where(x => x.m.Name.Contains(request.Name));

            if (request.DepartmentId != null) query = query.Where(x => x.m.DepartmentId == request.DepartmentId);

            if (request.Status != null) query = query.Where(x => x.m.Status == request.Status);

            var items = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                    .Select(x => new Major()
                                        {
                                            Id = x.m.Id,
                                            DepartmentId = x.m.DepartmentId,
                                            Description = x.m.Description,
                                            MajorCode = x.m.MajorCode,
                                            Name = x.m.Name,
                                            Status = x.m.Status
                                        }
                                    ).ToListAsync();

            return new PagingResult<Major>(items, context.Majors.Count(), request.CurrentPage, request.PageSize);
        }
    }
}
