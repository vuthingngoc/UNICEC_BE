using UNICS.Data.ViewModels.Entities.University;

namespace UNICS.Business.Services.UniversitySvc
{
    public interface IUniversityService
    {
        public ViewUniversity GetUniversityById(string id);
    }
}
