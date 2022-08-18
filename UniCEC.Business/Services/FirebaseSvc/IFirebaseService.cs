using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Firebase.Auth;

namespace UniCEC.Business.Services.FirebaseSvc
{
    public interface IFirebaseService
    {
        public Task<ViewUserInfo> Authentication(string token, string deviceId);
    }
}
