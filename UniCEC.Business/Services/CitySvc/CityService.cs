using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Business.Services.NotificationSvc;
using UniCEC.Business.Services.UserSvc;
using UniCEC.Business.Utilities;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.CityRepo;
using UniCEC.Data.Repository.ImplRepo.UniversityRepo;
using UniCEC.Data.Repository.ImplRepo.UserRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.City;

namespace UniCEC.Business.Services.CitySvc
{
    public class CityService : ICityService
    {
        private ICityRepo _cityRepo;
        private IUniversityRepo _universityRepo;
        private DecodeToken _decodeToken;
        private IUserRepo _userRepo;

        public CityService(ICityRepo cityRepo, IUniversityRepo universityRepo, IUserRepo userRepo)
        {
            _cityRepo = cityRepo;
            _universityRepo = universityRepo;
            _decodeToken = new DecodeToken();
            _userRepo = userRepo;
        }

        // Search cities
        public async Task<PagingResult<ViewCity>> SearchCitiesByName(string token, CityRequestModel request)
        {
            int roleId = _decodeToken.Decode(token, "RoleId");
            if (!roleId.Equals(4)) request.Status = true;

            PagingResult<ViewCity> result = await _cityRepo.SearchCitiesByName(request);

            if (result == null) throw new NullReferenceException();
            return result;
        }

        // Get city by id
        public async Task<ViewCity> GetByCityId(int id, string token)
        {
            int roleId = _decodeToken.Decode(token, "RoleId");
            bool? status = null;

            if (!roleId.Equals(4)) status = true;

            ViewCity city = await _cityRepo.GetById(id, status);

            if (city == null) throw new NullReferenceException("Not found this city");
            return city;
        }

        public void CheckValidAuthorized(string token)
        {
            int roleId = _decodeToken.Decode(token, "RoleId");
            if (roleId != 4) throw new UnauthorizedAccessException("You do not have permission to access this resource"); // not system admin
        }

        //Insert-City
        public async Task<ViewCity> Insert(string token, CityInsertModel model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Description))
                throw new ArgumentNullException(" Name Null || Description Null ");

            CheckValidAuthorized(token);

            City city = new City()
            {
                Name = model.Name,
                Description = model.Description,
                Status = true // default status when inserting
            };

            int result = await _cityRepo.Insert(city);

            //return data when insert success
            return (result > 0)
                ? new ViewCity()
                {
                    Id = result,
                    Name = model.Name,
                    Description = model.Description,
                    Status = true
                }
                : null;
        }


        //Update-City
        public async Task<bool> Update(string token, CityUpdateModel model)
        {
            CheckValidAuthorized(token);

            //get city
            City city = await _cityRepo.Get(model.Id);
            //
            if (city != null)
            {
                city.Name = (!string.IsNullOrEmpty(model.Name)) ? model.Name : city.Name;
                city.Description = (!string.IsNullOrEmpty(model.Description)) ? model.Description : city.Description;
                city.Status = (model.Status.HasValue) ? model.Status.Value : city.Status;              

                await _cityRepo.Update();

                // update relevant university and user account
                if (model.Status.HasValue)
                {
                    await _universityRepo.UpdateStatusByCityId(model.Id, model.Status.Value);
                    UserStatus userStatus = (model.Status.Value == true) ? UserStatus.Active : UserStatus.InActive;
                    List<int> idsUniInCity = await _universityRepo.GetListIdsUniByCity(model.Id);
                    foreach(int idUni in idsUniInCity)
                    {
                        await _userRepo.UpdateStatusByUniversityId(idUni, userStatus);
                    }
                }                

                return true;
            }
            else
            {
                throw new NullReferenceException("City not find to update");
            }
        }

        //
        public async Task Delete(string token, int id)
        {
            CheckValidAuthorized(token);

            City city = await _cityRepo.Get(id);
            if (city == null) throw new NullReferenceException("Not found this city");

            city.Status = false; // status for delete

            await _universityRepo.UpdateStatusByCityId(city.Id, city.Status);
            await _cityRepo.Update();
        }
    }
}
