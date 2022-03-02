
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNICS.Bussiness.Common;
using UNICS.Bussiness.ViewModels.University;
using UNICS.Data.Models.DB;
using UNICS.Data.Respository.ImpIRepo.UniversityRepo;

namespace UNICS.Bussiness.Services.UniversitySvc
{
    public class UniversityService : IUniversityService
    {
        IUniversityRepo UniversityRepo;

        //khởi tạo constructor để injection vào 
        public UniversityService(IUniversityRepo UniversityRepo)
        {
            this.UniversityRepo = UniversityRepo;
        }

        public ViewUniversity GetUniversityById(string id)
        {
            //
            University uni = UniversityRepo.Get(id);
            //
            ViewUniversity uniView = new ViewUniversity();
            //
            if(uni != null)
            {
                //gán vào các trường view
                uniView.Name = uni.Name;
                uniView.Description = uni.Description;
                uniView.Phone = uni.Phone;
                uniView.Email = uni.Email;
                uniView.Openning = uni.Openning.ToString();
                uniView.Closing = uni.Closing.ToString();
               //
            }
            return uniView;

        }
    }
}
