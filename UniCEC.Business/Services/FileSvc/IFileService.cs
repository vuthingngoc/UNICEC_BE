using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace UniCEC.Business.Services.FileSvc
{
    public interface IFileService
    {
        public Task<string> GetUrlFromFilenameAsync(string filename);
        public Task<string> UploadFile(IFormFile file);
        public Task<string> UploadFile(string base64String);
        public Task UploadFile(string oldFilename, IFormFile file); // update
        public Task UploadFile(string oldFilename, string base64String);
        public Task DeleteFile(string filename);
        
    }
}
