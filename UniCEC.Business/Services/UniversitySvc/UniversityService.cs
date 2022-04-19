using System;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.UniversityRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.University;

namespace UniCEC.Business.Services.UniversitySvc
{
    public class UniversityService : IUniversityService
    {
        private IUniversityRepo _universityRepo;

        public UniversityService(IUniversityRepo universityRepo)
        {
            _universityRepo = universityRepo;
        }

        //
        

        //Get-University-By-Id
        public async Task<ViewUniversity> GetUniversityById(int id)
        {
            University uni = await _universityRepo.Get(id);
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
                uniView.Openning = uni.Openning;
                uniView.Closing = uni.Closing;
                uniView.CityId = uni.CityId;
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
                University uni = new University();

                uni.CityId = universityModel.CityId;    
                uni.UniCode = universityModel.UniCode;
                uni.Name = universityModel.Name;
                uni.Description = universityModel.Description;
                uni.Phone = universityModel.Phone;
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

                    viewUniversity.Id = u.Id;
                    viewUniversity.CityId = u.CityId;
                    viewUniversity.UniCode = u.UniCode;
                    viewUniversity.Name = u.Name;
                    viewUniversity.Description = u.Description;
                    viewUniversity.Phone = u.Phone;
                    viewUniversity.Email = u.Email;
                    viewUniversity.Openning = u.Openning;
                    viewUniversity.Closing = u.Closing;
                    viewUniversity.Founding = u.Founding;
                    viewUniversity.Status = u.Status;
                    return viewUniversity;
                }
                else {
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
                //get Uni
                University uni = await _universityRepo.Get(university.Id);
                bool check = false;
                if (uni != null)
                {
                    //update name-des-phone-opening-closing-founding
                    uni.Name = (!university.Name.Equals("")) ? university.Name : uni.Name;
                    uni.Description = (!university.Description.Equals("")) ? university.Description : uni.Description;
                    uni.Phone = (!university.Phone.Equals("")) ? university.Phone : uni.Phone;
                    uni.Openning = (!university.Openning.Equals("")) ? university.Openning : uni.Openning;
                    uni.Closing = (!university.Closing.Equals("")) ? university.Closing : uni.Closing;
                    uni.Status = university.Status; 
                    check = await _universityRepo.Update();
                    return check;
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
                bool check = false;
                //
                University university = await _universityRepo.Get(id);
                if (university != null)
                {
                    //
                    university.Status = false;
                    check = await _universityRepo.Update();
                    if (check) {
                        return check;
                    }
                }
                else
                {
                    return check;
                }
             return check;
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
    }
}
