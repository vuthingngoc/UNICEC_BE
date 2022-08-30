using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.FileSvc;
using UniCEC.Business.Utilities;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.CityRepo;
using UniCEC.Data.Repository.ImplRepo.UniversityRepo;
using UniCEC.Data.Repository.ImplRepo.UserRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.University;

namespace UniCEC.Business.Services.UniversitySvc
{
    public class UniversityService : IUniversityService
    {
        private IUniversityRepo _universityRepo;
        private ICityRepo _cityRepo;
        private IFileService _fileService;
        private DecodeToken _decodeToken;
        private IUserRepo _userRepo;

        public UniversityService(IUniversityRepo universityRepo, ICityRepo cityRepo, IFileService fileService, IUserRepo userRepo)
        {
            _universityRepo = universityRepo;
            _cityRepo = cityRepo;
            _fileService = fileService;
            _decodeToken = new DecodeToken();
            _userRepo = userRepo;
        }

        //
        private async Task<string> GetImageUrl(string imageUrl, int universityId)
        {
            string fullPathImage = await _fileService.GetUrlFromFilenameAsync(imageUrl) ?? "";
            if (!string.IsNullOrEmpty(fullPathImage) && !fullPathImage.Equals(imageUrl)) // update db
            {
                University university = await _universityRepo.Get(universityId);                
                university.ImageUrl = fullPathImage;
                await _universityRepo.Update();
            }

            return fullPathImage;
        }


        //Get-University-By-Id
        public async Task<ViewUniversity> GetUniversityById(int id)
        {
            //University uni = await _universityRepo.Get(id);
            ////
            //City city = await _cityRepo.Get(uni.CityId);
            ////
            //ViewUniversity uniView = new ViewUniversity();
            ////
            //if (uni != null)
            //{
            //    //gán vào các trường view
            //    uniView.Id = id;
            //    uniView.ImgURL = await GetImageUrl(uni.ImageUrl, uni.Id);
            //    uniView.Name = uni.Name;
            //    uniView.Description = uni.Description;
            //    uniView.Phone = uni.Phone;
            //    uniView.Email = uni.Email;
            //    uniView.Opening = uni.Openning;
            //    uniView.Closing = uni.Closing;
            //    uniView.CityId = uni.CityId;
            //    uniView.CityName = city.Name;
            //    uniView.Status = uni.Status;                
            //    uniView.Founding = uni.Founding;
            //    uniView.UniCode = uni.UniCode;
            //    //
            //}

            ViewUniversity university = await _universityRepo.GetById(id);
            if (university == null) throw new NullReferenceException("Not found this university");
            return university;

        }

        //Insert-University
        public async Task<ViewUniversity> Insert(UniversityInsertModel universityModel)
        {

            try
            {

                if (universityModel.CityId == 0
                    || string.IsNullOrEmpty(universityModel.UniCode)
                    || string.IsNullOrEmpty(universityModel.Name)
                    || string.IsNullOrEmpty(universityModel.Description)
                    || string.IsNullOrEmpty(universityModel.Phone)
                    || string.IsNullOrEmpty(universityModel.Email)
                    || universityModel.Founding == DateTime.Parse("1/1/0001 12:00:00 AM")
                    || string.IsNullOrEmpty(universityModel.Openning)
                    || universityModel.Openning.Length > 5
                    || string.IsNullOrEmpty(universityModel.Closing)
                    || universityModel.Closing.Length > 5
                    )
                    throw new ArgumentNullException("CityId Null || UniCode Null || Name Null || Description Null || Phone Null " +
                                                     " Email Null  || Founding Null || Openning Null or length > 5 ||  Closing Null or length > 5 ");

                // check duplicated university
                int existedUniId = await _universityRepo.CheckDuplicatedUniversity(universityModel.Name, universityModel.CityId, universityModel.UniCode);
                if (existedUniId > 0) throw new ArgumentException("Duplicated university");

                University uni = new University();

                uni.CityId = universityModel.CityId;
                uni.UniCode = universityModel.UniCode;
                uni.Name = universityModel.Name;
                uni.Description = universityModel.Description;
                uni.Phone = universityModel.Phone;
                //
                if (!string.IsNullOrEmpty(universityModel.Image))
                    uni.ImageUrl = (universityModel.Image.Contains("https"))
                        ? universityModel.Image
                        : await _fileService.UploadFile(universityModel.Image);

                uni.Email = universityModel.Email;
                uni.Openning = universityModel.Openning;
                uni.Closing = universityModel.Closing;
                uni.Founding = universityModel.Founding;
                //auto set true
                uni.Status = true;
                //view
                ViewUniversity viewUniversity = new ViewUniversity();
                int result = await _universityRepo.Insert(uni);
                //return data when insert success
                if (result > 0)
                {
                    //
                    City c = await _cityRepo.Get(uni.CityId);

                    viewUniversity.Id = result;// u.Id;
                    viewUniversity.CityId = uni.CityId;
                    viewUniversity.CityName = c.Name;
                    viewUniversity.UniCode = uni.UniCode;
                    viewUniversity.Name = uni.Name;
                    viewUniversity.Description = uni.Description;
                    viewUniversity.Phone = uni.Phone;
                    viewUniversity.ImgURL = uni.ImageUrl;
                    viewUniversity.Email = uni.Email;
                    viewUniversity.Opening = uni.Openning;
                    viewUniversity.Closing = uni.Closing;
                    viewUniversity.Founding = uni.Founding;
                    viewUniversity.Status = uni.Status;
                    return viewUniversity;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception) { throw; }

        }

        //Update-University
        public async Task<bool> Update(UniversityUpdateModel university, string token)
        {
            try
            {
                int roleId = _decodeToken.Decode(token, "RoleId");
                if (roleId.Equals(3)) // student
                    throw new UnauthorizedAccessException("You do not have permission to access this resource");

                if (roleId.Equals(1)) // uni admin
                {
                    int uniId = _decodeToken.Decode(token, "UniversityId");
                    if (!uniId.Equals(university.Id)) throw new UnauthorizedAccessException("You do not have permission to access this resource");
                }

                if (university.Opening?.Length > 5 || university.Closing?.Length > 5) 
                    throw new ArgumentException("Opening time || Closing time length must smaller than five");        

                if (university.Id == 0) throw new ArgumentNullException("University Id Null");
                //get Uni
                University uni = await _universityRepo.Get(university.Id);

                if (uni == null) throw new ArgumentException("University not found to update");

                // check duplicated university
                if (!string.IsNullOrEmpty(university.Name) && university.CityId > 0 && !string.IsNullOrEmpty(university.UniCode)){
                    int existedUniId = await _universityRepo.CheckDuplicatedUniversity(university.Name, university.CityId, university.UniCode);
                    if (existedUniId > 0 && existedUniId != uni.Id) throw new ArgumentException("Duplicated university");
                }

                // upload new image to firebase
                if(!string.IsNullOrEmpty(university.Image)) 
                    uni.ImageUrl = (university.Image.Contains("https")) 
                        ? university.Image 
                        : await _fileService.UploadFile(university.Image);

                //update name-des-phone-opening-closing-founding-cityId-unicode
                uni.Name = (!string.IsNullOrEmpty(university.Name)) ? university.Name : uni.Name;
                uni.Description = (!string.IsNullOrEmpty(university.Description)) ? university.Description : uni.Description;
                uni.Phone = (!string.IsNullOrEmpty(university.Phone)) ? university.Phone : uni.Phone;                
                uni.Openning = (!string.IsNullOrEmpty(university.Opening)) ? university.Opening : uni.Openning;
                uni.Closing = (!string.IsNullOrEmpty(university.Closing)) ? university.Closing : uni.Closing;
                uni.CityId = (university.CityId > 0) ? university.CityId : uni.CityId;
                uni.UniCode = (!string.IsNullOrEmpty(university.Name)) ? university.UniCode : uni.UniCode;
                // update status
                UserStatus? userStatus = null;
                if (university.Status.HasValue && roleId.Equals(4)) // if System admin
                {
                    uni.Status = university.Status.Value;
                    userStatus = (uni.Status == true) ? UserStatus.Active : UserStatus.InActive;
                }

                await _universityRepo.Update();

                // update status relevant user account
                if (userStatus != null) await _userRepo.UpdateStatusByUniversityId(uni.Id, userStatus.Value);

                return true;

            }
            catch (Exception)
            {
                throw;
            }

        }

        //Delete-University
        public async Task<bool> Delete(int id) // no use anymore
        {
            try
            {
                University university = await _universityRepo.Get(id);
                if (university != null)
                {
                    await _universityRepo.DeleteUniversity(id);
                    return true;
                }
                else
                {
                    throw new ArgumentException("University not found to update");
                }
            }
            catch (Exception) { throw; }
        }

        //Get-Universities-By-Conditions
        public async Task<PagingResult<ViewUniversity>> GetUniversitiesByConditions(UniversityRequestModel request)
        {
            //
            PagingResult<ViewUniversity> result = await _universityRepo.GetUniversitiesByConditions(request);

            if (result == null) throw new NullReferenceException();

            foreach (ViewUniversity vu in result.Items)
            {
                vu.ImgURL = await GetImageUrl(vu.ImgURL, vu.Id);
            }
            //
            return result;
        }

        //Check-Email-University
        public async Task<bool> CheckEmailUniversity(string email)
        {
            return await _universityRepo.CheckEmailUniversity(email);
        }

        //Get-List-Universities-By-Email
        public async Task<List<ViewUniversity>> GetListUniversityByEmail(string email)
        {
            List<ViewUniversity> result = await _universityRepo.GetListUniversityByEmail(email);
            if (result == null) throw new NullReferenceException();
            foreach (ViewUniversity vu in result)
            {
                vu.ImgURL = await GetImageUrl(vu.ImgURL, vu.Id);               
            }

            return result;
        }

        public async Task<List<ViewUniversity>> GetUniversities()
        {
            List<ViewUniversity> result = await _universityRepo.GetUniversities();
            if (result == null) throw new NullReferenceException();
            foreach (ViewUniversity vu in result)
            {
                vu.ImgURL = await GetImageUrl(vu.ImgURL, vu.Id);
            }

            return result;
        }
    }
}
