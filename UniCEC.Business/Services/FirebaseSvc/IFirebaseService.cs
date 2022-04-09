using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Entities.User;

namespace UniCEC.Business.Services.FirebaseSvc
{
    public interface IFirebaseService
    {
        public Task<ViewUser> AuthenUser(HttpRequest request);
    }
}
