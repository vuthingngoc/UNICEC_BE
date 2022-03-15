using System.Threading.Tasks;
using UNICS.Data.ViewModels.Entities.University;

namespace UNICS.Business.Services.UniversitySvc
{
    public interface IUniversityService
    {
        Task<ViewUniversity> GetUniversityById(string id);
    }
}
