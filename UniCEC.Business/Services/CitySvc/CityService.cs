using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.CityRepo;
using UniCEC.Data.Repository.ImplRepo.UniversityRepo;
using UniCEC.Data.RequestModels;
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
            if (_tokenHandler == null) _tokenHandler = new JwtSecurityTokenHandler();
            var claim = _tokenHandler.ReadJwtToken(token).Claims.FirstOrDefault(selector => selector.Type.ToString().Equals(nameClaim));
            return Int32.Parse(claim.Value);
        }

        // Search cities
        public async Task<PagingResult<ViewCity>> SearchCitiesByName(string token, CityRequestModel request)
        {
            int roleId = DecodeToken(token, "RoleId");
            if (!roleId.Equals(4)) request.Status = true;
            
            PagingResult<ViewCity> result = await _cityRepo.SearchCitiesByName(request);

            if (result == null) throw new NullReferenceException();
            return result;
        }

        // Get city by id
        public async Task<ViewCity> GetByCityId(int id, string token)
        {
            int roleId = DecodeToken(token, "RoleId");
            bool? status = null;

            if (!roleId.Equals(4)) status = true;

            ViewCity city = await _cityRepo.GetById(id, status);

            if (city == null) throw new NullReferenceException("Not found this city");
            return city;
        }

        //Insert-City
        public async Task<ViewCity> Insert(string token, CityInsertModel model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Description))
                throw new ArgumentNullException(" Name Null || Description Null ");

            int roleId = DecodeToken(token, "RoleId");
            if (roleId != 4) throw new UnauthorizedAccessException("You do not have permission to access this resource"); // not system admin

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
            int roleId = DecodeToken(token, "RoleId");
            if (roleId != 4) throw new UnauthorizedAccessException("You do not have permission to access this resource"); // not system admin

            //get city
            City city = await _cityRepo.Get(model.Id);
            //
            if (city != null)
            {
                city.Name = (!model.Name.Equals("")) ? model.Name : city.Name;
                city.Description = (!model.Description.Equals("")) ? model.Description : city.Description;
                if (city.Status == false && model.Status == true) await _universityRepo.UpdateStatusByCityId(model.Id, model.Status.Value);

                await _cityRepo.Update();
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
            City city = await _cityRepo.Get(id);
            if (city == null) throw new NullReferenceException("Not found this city");

            city.Status = false; // status for delete

            await _universityRepo.UpdateStatusByCityId(city.Id, city.Status);
            await _cityRepo.Update();
        }
    }
}
