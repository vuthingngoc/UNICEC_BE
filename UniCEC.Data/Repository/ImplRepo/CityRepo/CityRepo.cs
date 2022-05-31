using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.City;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace UniCEC.Data.Repository.ImplRepo.CityRepo
{
    public class CityRepo : Repository<City>, ICityRepo
    {
        public CityRepo(UniCECContext context) : base(context)
        {

        }

        public Task<ViewCity> GetById(int id)
        {
            //var query = from c in context.Cities
            //            where c.Id.Equals(id)
            throw new System.NotImplementedException();
        }

        // Search Cities
        public async Task<PagingResult<ViewCity>> SearchCitiesByName(string name, PagingRequest request)
        {
            var query = from c in context.Cities
                        where c.Name.Contains(name)
                        select c;
            
            int totalCount = query.Count();
            //filter 
            List<ViewCity> cities = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                .Select(c => new ViewCity()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                }).ToListAsync();            

            return new PagingResult<ViewCity>(cities, totalCount, request.CurrentPage, request.PageSize);
        }
    }
}
