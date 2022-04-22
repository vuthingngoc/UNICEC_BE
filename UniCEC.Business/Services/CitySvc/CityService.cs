using System;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.CityRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.City;

namespace UniCEC.Business.Services.CitySvc
{
    public class CityService : ICityService
    {
        private ICityRepo _cityRepo;

        public CityService(ICityRepo cityRepo)
        {
            _cityRepo = cityRepo;
        }


        //Get-List-Cites
        public async Task<PagingResult<ViewCity>> GetListCities(CityRequestModel request)
        {
            PagingResult<ViewCity> result = await _cityRepo.GetListCities(request);
            return result;
        }

        //Get-List-Cites-By-Id
        public async Task<ViewCity> GetByCityId(int id)
        {
            try
            {
                City city = await _cityRepo.Get(id);
                ViewCity viewCity = new ViewCity();
                if (city != null)
                {
                    viewCity.Id = city.Id;
                    viewCity.Name = city.Name;
                    viewCity.Description = city.Description;
                    return viewCity;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
            
        }
        //Insert-City
        public async Task<ViewCity> Insert(CityInsertModel model)
        {
            try {
                City city = new City();

                city.Name = model.Name;
                city.Description = model.Description;

                int result = await _cityRepo.Insert(city);
                //view
                ViewCity viewCity = new ViewCity();
                //return data when insert success
                if (result > 0)
                {
                    //
                    City c = await _cityRepo.Get(result);
                    viewCity.Id = c.Id;
                    viewCity.Name = c.Name;
                    viewCity.Description = c.Description;
                    return viewCity;
                }
                else {
                    return null;
                }
            }
            catch (Exception) { throw; }
        }


        //Update-City
        public async Task<bool> Update(ViewCity city)
        {
            try{
                //get city
                City c = await _cityRepo.Get(city.Id);
                //
                if (c != null)
                {
                    c.Name = (!city.Name.Equals("")) ? city.Name : c.Name;
                    c.Description = (!city.Description.Equals("")) ? city.Description : c.Description;

                    await _cityRepo.Update();
                    return true;
                }
                return false;
            }
            catch (Exception) { 
                throw;
            }
        }

        //
        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        

      

       
    }
}
