using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.GroupUniversityRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.GroupUniversity;

namespace UNICS.Business.Services.GroupUniversitySvc
{
    public class GroupUniversityService : IGroupUniversityService
    {
        private IGroupUniversityRepo _groupUniversityRepo;

        public GroupUniversityService(IGroupUniversityRepo groupUniversityRepo)
        {
            _groupUniversityRepo = groupUniversityRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewGroupUniversity>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewGroupUniversity> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Insert(GroupUniversityInsertModel groupUniversity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewGroupUniversity groupUniversity)
        {
            throw new NotImplementedException();
        }
    }
}
