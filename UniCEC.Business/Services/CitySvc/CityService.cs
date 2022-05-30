using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.CityRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.City;

namespace UniCEC.Business.Services.CitySvc
{
    public class CityService : ICityService
    {
        private ICityRepo _cityRepo;
        private JwtSecurityTokenHandler _tokenHandler;
        //private IConfiguration _configuration;

        public CityService(ICityRepo cityRepo, IConfiguration configuration)
        {
            _cityRepo = cityRepo;
            //_configuration = configuration;
        }
        // Test upload file 
        //public async Task<string> UploadFile(IFormFile file, string token)
        //{
        //    string bucket = _configuration.GetSection("Firebase").GetSection("Bucket").Value;//"unics-e46a4.appspot.com";

        //    Stream stream = file.OpenReadStream();
        //    var cancellationToken = new CancellationTokenSource().Token;
        //    var firebaseStorage = new FirebaseStorage(bucket);
        //    var fullPath = await firebaseStorage.Child("assets").Child($"{Guid.NewGuid()}").PutAsync(stream, cancellationToken);
        //    var fileName = GetFileName(fullPath);
        //    return await firebaseStorage.Child("assets").Child(fileName).GetDownloadUrlAsync();
        //}

        //private string GetFileName(string url)
        //{
        //    return url.Split("%2F")[1].Split("?")[0];
        //}

        //public async Task DeleteFile(string url)
        //{
        //    string fileName = GetFileName(url);
        //    string bucket = _configuration.GetSection("Firebase").GetSection("Bucket").Value;
        //    await new FirebaseStorage(bucket).Child("assets").Child(fileName).DeleteAsync();
        //}

        //public async Task UpdateFile(string oldFileName, IFormFile file, string token)
        //{
        //    string bucket = _configuration.GetSection("Firebase").GetSection("Bucket").Value;//"unics-e46a4.appspot.com";

        //    Stream stream = file.OpenReadStream();
        //    var cancellationToken = new CancellationTokenSource().Token;
        //    var firebaseStorage = new FirebaseStorage(bucket);
        //    await firebaseStorage.Child("assets").Child($"{oldFileName}").PutAsync(stream, cancellationToken);
        //}

        // Search cities
        public async Task<PagingResult<ViewCity>> SearchCitiesByName(string name, string token, PagingRequest request)
        {
            PagingResult<ViewCity> result = await _cityRepo.SearchCitiesByName(name, request);
            if (result == null) throw new NullReferenceException();
            return result;
        }

        // Get city by id
        public async Task<ViewCity> GetByCityId(int id, string token)
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
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        //Insert-City
        public async Task<ViewCity> Insert(CityInsertModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Description))
                    throw new ArgumentNullException(" Name Null || Description Null ");

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
                else
                {
                    return null;
                }
            }
            catch (Exception) { throw; }
        }


        //Update-City
        public async Task<bool> Update(ViewCity city)
        {
            try
            {
                if (string.IsNullOrEmpty(city.Name) || string.IsNullOrEmpty(city.Description) || city.Id == 0)
                    throw new ArgumentNullException(" Name Null || Description Null || City Id Null");
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
                else
                {
                    throw new ArgumentException("City not find to update");
                }
                return false;
            }
            catch (Exception)
            {
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
