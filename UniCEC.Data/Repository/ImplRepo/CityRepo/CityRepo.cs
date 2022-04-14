using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
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

        //Get-List-Cites
        public async Task<PagingResult<ViewCity>> GetListCities(CityRequestModel request)
        {
            //View
            List<ViewCity> listCitiesView = new List<ViewCity>();

            //
            var query = from City c in context.Cities
                        select c;
            //filter...
            //phân trang
            List<City> listCities = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
            //
            int count = listCities.Count();
            //
            listCities.ForEach((City c) => {
                //gán qua view
                ViewCity viewCity = new ViewCity();
                viewCity.Id = c.Id;
                viewCity.Name = c.Name;
                viewCity.Description = c.Description;
                //
                listCitiesView.Add(viewCity);
                //
            });

            //PagingResult
            PagingResult<ViewCity> result = new PagingResult<ViewCity>(listCitiesView, count, request.CurrentPage,request.PageSize);
            //
            return result;

        }
    }
}
