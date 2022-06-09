using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.CityRepo;
using UniCEC.Data.Repository.ImplRepo.UniversityRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.City;

namespace UniCEC.Business.Services.CitySvc
{
    public class CityService : ICityService
    {
        private ICityRepo _cityRepo;
        private IUniversityRepo _universityRepo;
        private JwtSecurityTokenHandler _tokenHandler;
        //private IConfiguration _configuration;

        public CityService(ICityRepo cityRepo, IUniversityRepo universityRepo)//, IConfiguration configuration)
        {
            _cityRepo = cityRepo;
            _universityRepo = universityRepo;
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

        //private Stream ConvertBase64ToStream(string base64)
        //{
        //    byte[] bytes = Convert.FromBase64String(base64);
        //    return new MemoryStream(bytes);
        //}

        //public async Task<string> UploadFile(string base64String)
        //{
        //    Stream stream = ConvertBase64ToStream(base64String);

        //    string bucket = _configuration.GetSection("Firebase:Bucket").Value;//"unics-e46a4.appspot.com";

        //    var cancellationToken = new CancellationTokenSource().Token;
        //    var firebaseStorage = new FirebaseStorage(bucket);
        //    var filename = Guid.NewGuid();
        //    await firebaseStorage.Child("assets").Child($"{filename}").PutAsync(stream, cancellationToken);
        //    return filename.ToString();
        //}

        //public async Task<string> GetUrlFromFilenameAsync(string filename)
        //{
        //    return await new FirebaseStorage(_configuration.GetSection("Firebase:Bucket").Value).Child("assets").Child($"{filename}").GetDownloadUrlAsync();
        //}



        private int DecodeToken(string token, string nameClaim)
        {
            if(_tokenHandler == null) _tokenHandler = new JwtSecurityTokenHandler();
            var claim = _tokenHandler.ReadJwtToken(token).Claims.FirstOrDefault(selector => selector.Type.ToString().Equals(nameClaim));
            return Int32.Parse(claim.Value);
        }

        // Search cities
        public async Task<PagingResult<ViewCity>> SearchCitiesByName(string name, string token, PagingRequest request)
        {
            int roleId = DecodeToken(token, "RoleId");
            PagingResult<ViewCity> result = await _cityRepo.SearchCitiesByName(name, roleId, request);

            if (result == null) throw new NullReferenceException();
            return result;
        }

        // Get city by id
        public async Task<ViewCity> GetByCityId(int id, string token)
        {
            try
            {
                int roleId = DecodeToken(token, "RoleId");
                ViewCity city = await _cityRepo.GetById(id,roleId);

                if (city == null) throw new NullReferenceException("Not found this city");
                return city;
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
        public async Task<bool> Update(CityUpdateModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Description) || model.Id == 0)
                    throw new ArgumentNullException(" Name Null || Description Null || Id Null");
                //get city
                City city = await _cityRepo.Get(model.Id);
                //
                if (city != null)
                {
                    city.Name = (!model.Name.Equals("")) ? model.Name : city.Name;
                    city.Description = (!model.Description.Equals("")) ? model.Description : city.Description;
                    if(city.Status == false && model.Status == true) await _universityRepo.UpdateStatusByCityId(model.Id, model.Status.Value);               

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
        public async Task Delete(int id)
        {
            City city = await _cityRepo.Get(id);
            if (city == null) throw new NullReferenceException("Not found this city");

            bool status = false; // status for delete
            city.Status = status; 
            
            await _universityRepo.UpdateStatusByCityId(city.Id, status);
            await _cityRepo.Update();
        }
    }
}
