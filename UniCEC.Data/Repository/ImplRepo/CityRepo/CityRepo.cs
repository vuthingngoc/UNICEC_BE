using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.City;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using UniCEC.Data.RequestModels;

namespace UniCEC.Data.Repository.ImplRepo.CityRepo
{
    public class CityRepo : Repository<City>, ICityRepo
    {
        public CityRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<ViewCity> GetById(int id, bool? status)
        {
            var query = from c in context.Cities
                        where c.Id.Equals(id)
                        select c;

            if (status.HasValue) query = query.Where(city => city.Status.Equals(status.Value)); // if not system admin

            return await query.Select(c => new ViewCity()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Status = c.Status
            }).FirstOrDefaultAsync();
        }

        // Search Cities
        public async Task<PagingResult<ViewCity>> SearchCitiesByName(CityRequestModel request)
        {
            var query = from c in context.Cities                        
                        select c;

            if(!string.IsNullOrEmpty(request.Name)) query = query.Where(city => city.Name.Contains(request.Name));

            if(request.Status.HasValue) query = query.Where(city => city.Status.Equals(request.Status.Value));

            int totalCount = query.Count();
            //filter 
            List<ViewCity> cities = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                .Select(c => new ViewCity()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Status = c.Status
                }).ToListAsync();

            return (totalCount > 0)
                ? new PagingResult<ViewCity>(cities, totalCount, request.CurrentPage, request.PageSize) : null;
        }
    }
}
