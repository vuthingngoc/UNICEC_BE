using System.Threading.Tasks;
using UNICS.Data.Models.DB;
using UNICS.Data.Repository.ImplRepo.UniversityRepo;
using UNICS.Data.ViewModels.Common;
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

        public Task<bool> Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<PagingResult<ViewUniversity>> GetAll(PagingRequest request)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ViewUniversity> GetUniversityById(string id)
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
                //
            }
            return uniView;

        }

        public Task<ViewUniversity> Insert(UniversityInsertModel university)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Update(ViewUniversity university)
        {
            throw new System.NotImplementedException();
        }
    }
}
