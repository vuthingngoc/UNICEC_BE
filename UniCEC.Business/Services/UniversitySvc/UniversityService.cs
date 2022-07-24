using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.FileSvc;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.CityRepo;
using UniCEC.Data.Repository.ImplRepo.UniversityRepo;
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

        public UniversityService(IUniversityRepo universityRepo, ICityRepo cityRepo, IFileService fileService)
        {
            _universityRepo = universityRepo;
            _cityRepo = cityRepo;
            _fileService = fileService; 
        }

        //


        //Get-University-By-Id
        public async Task<ViewUniversity> GetUniversityById(int id)
        {
            University uni = await _universityRepo.Get(id);
            //
            City city = await _cityRepo.Get(uni.CityId);
            //
            ViewUniversity uniView = new ViewUniversity();
            //
            if (uni != null)
            {
                //gán vào các trường view
                uniView.Name = uni.Name;
                uniView.Description = uni.Description;
                uniView.Phone = uni.Phone;
                uniView.Email = uni.Email;
                uniView.Opening = uni.Openning;
                uniView.Closing = uni.Closing;
                uniView.CityId = uni.CityId;
                uniView.CityName = city.Name;
                uniView.Status = uni.Status;
                uniView.Id = id;
                uniView.Founding = uni.Founding;
                uniView.UniCode = uni.UniCode;
                //
            }
            return uniView;

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
                    || string.IsNullOrEmpty(universityModel.Closing)
                    )
                    throw new ArgumentNullException("CityId Null || UniCode Null || Name Null || Description Null || Phone Null " +
                                                     " Email Null  || Founding Null || Openning Null ||  Closing Null ");



                University uni = new University();

                uni.CityId = universityModel.CityId;
                uni.UniCode = universityModel.UniCode;
                uni.Name = universityModel.Name;
                uni.Description = universityModel.Description;
                uni.Phone = universityModel.Phone;
                //
                if (!string.IsNullOrEmpty(universityModel.Base64StringImg))
                {
                    string url = await _fileService.UploadFile(universityModel.Base64StringImg);
                    
                }
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
                    University u = await _universityRepo.Get(result);

                    City c = await _cityRepo.Get(result);

                    viewUniversity.Id = u.Id;
                    viewUniversity.CityId = u.CityId;
                    viewUniversity.CityName = c.Name;
                    viewUniversity.UniCode = u.UniCode;
                    viewUniversity.Name = u.Name;
                    viewUniversity.Description = u.Description;
                    viewUniversity.Phone = u.Phone;
                    viewUniversity.ImgURL = u.ImageUrl; 
                    viewUniversity.Email = u.Email;
                    viewUniversity.Opening = u.Openning;
                    viewUniversity.Closing = u.Closing;
                    viewUniversity.Founding = u.Founding;
                    viewUniversity.Status = u.Status;
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
        public async Task<bool> Update(ViewUniversity university)
        {
            try
            {
                if (university.Id == 0) throw new ArgumentNullException("University Id Null");
                //get Uni
                University uni = await _universityRepo.Get(university.Id);
                bool check = false;
                if (uni != null)
                {
                    //update name-des-phone-opening-closing-founding-cityId-unicode
                    uni.Name = (!string.IsNullOrEmpty(university.Name)) ? university.Name : uni.Name;
                    uni.Description = (!string.IsNullOrEmpty(university.Description)) ? university.Description : uni.Description;
                    uni.Phone = (!string.IsNullOrEmpty(university.Phone)) ? university.Phone : uni.Phone;
                    if (!string.IsNullOrEmpty(university.ImgURL)) {
                        uni.ImageUrl = await _fileService.UploadFile(university.ImgURL);
                    }                 
                    uni.Openning = (!string.IsNullOrEmpty(university.Opening)) ? university.Opening : uni.Openning;
                    uni.Closing = (!string.IsNullOrEmpty(university.Closing)) ? university.Closing : uni.Closing;
                    uni.CityId = (university.CityId > 0) ? university.CityId : uni.CityId;
                    uni.UniCode = (!string.IsNullOrEmpty(university.Name)) ? university.UniCode : uni.UniCode;
                    uni.Status = university.Status;
                    //img url

                    await _universityRepo.Update();
                    return true;
                }
                else
                {
                    throw new ArgumentException("University not found to update");
                }
                return check;
            }
            catch (Exception)
            {
                throw;
            }

        }

        //Delete-University
        public async Task<bool> Delete(int id)
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
                return false;
            }
            catch (Exception) { throw; }
        }

        //Get-Universities-By-Conditions
        public async Task<PagingResult<ViewUniversity>> GetUniversitiesByConditions(UniversityRequestModel request)
        {
            //
            PagingResult<ViewUniversity> result = await _universityRepo.GetUniversitiesByConditions(request);
            //
            return result;
        }

        //Check-Email-University
        public async Task<bool> CheckEmailUniversity(string email)
        {
            bool check = false;

            check = await _universityRepo.CheckEmailUniversity(email);

            return check;
        }

        //Get-List-Universities-By-Email
        public async Task<List<ViewUniversity>> GetListUniversityByEmail(string email)
        {
            List<ViewUniversity> result = await _universityRepo.GetListUniversityByEmail(email);
            if (result == null) throw new NullReferenceException();
            return result;
        }
    }
}
