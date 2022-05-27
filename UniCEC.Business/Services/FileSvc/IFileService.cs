using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace UniCEC.Business.Services.FileSvc
{
    public interface IFileService
    {
        public Task<string> UploadFile(IFormFile file);
    }
}
