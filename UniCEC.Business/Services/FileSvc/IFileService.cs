using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace UniCEC.Business.Services.FileSvc
{
    public interface IFileService
    {
        public Task<string> GetUrlFromFilenameAsync(string filename);
        public Task<string> UploadFile(IFormFile file);
        public Task UploadFile(string oldFilename, IFormFile file); // update 
        public Task DeleteFile(string filename);
    }
}
