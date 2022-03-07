using UNICS.Data.Models.DB;
using UNICS.Data.Repository.ImplRepo.UniversityRepo;
using UNICS.Data.ViewModels.Entities.University;

namespace UNICS.Business.Services.UniversitySvc
{
    public class UniversityService : IUniversityService
    {
        private IUniversityRepo _universityRepo;

        public UniversityService(IUniversityRepo universityRepo)
        {
            _universityRepo = universityRepo;
        }

        public ViewUniversity GetUniversityById(string id)
        {
            University uni = _universityRepo.Get(id);
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
                uniView.Openning = uni.Openning.ToString();
                uniView.Closing = uni.Closing.ToString();
                //
            }
            return uniView;

        }
    }
}
