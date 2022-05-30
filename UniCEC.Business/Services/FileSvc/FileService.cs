using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace UniCEC.Business.Services.FileSvc
{
    public class FileService : IFileService
    {
        private IConfiguration _configuration;
        private string _bucket;

        public FileService(IConfiguration configuration)
        {
            _configuration = configuration;
            _bucket = _configuration.GetSection("Firebase").GetSection("Bucket").Value;
        }

        public bool ValidationFile(IFormFile file)
        {
            const int MAX_SIZE = 10 * 1024 * 1024; // 10MB
            string[] listExtensions = { ".png", ".jpeg", ".jpg", ".jfif", ".gif", ".webp" }; 

            bool isValid = false;

            if (file.Length == 0) throw new NullReferenceException("Null File");

            if (file.Length > 0 && file.Length < MAX_SIZE) isValid = true;

            if (isValid)
            {
                string extensionFile = Path.GetExtension(file.FileName);

                foreach (var extension in listExtensions)
                {
                    if (extensionFile.Equals(extension))
                    {
                        isValid = true;
                        break;
                    }
                }
            }
            
            return isValid;
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            bool isValid = ValidationFile(file);
            if (!isValid) throw new ArgumentException("Just image and the size is less than 10MB");

            Stream stream = file.OpenReadStream();
            var cancellationToken = new CancellationTokenSource().Token;
            var fileName = Guid.NewGuid();
            var firebaseStorage = new FirebaseStorage(_bucket);
            await firebaseStorage.Child("assets").Child($"{fileName}").PutAsync(stream, cancellationToken);
            return await firebaseStorage.Child("assets").Child($"{fileName}").GetDownloadUrlAsync();
        }

        public async Task UploadFile(string oldFilename, IFormFile file)
        {
            bool isValid = ValidationFile(file);
            if (!isValid) throw new ArgumentException("Just image and the size is less than 10MB");

            Stream stream = file.OpenReadStream();
            var cancellationToken = new CancellationTokenSource().Token;
            await new FirebaseStorage(_bucket).Child("assets").Child($"{oldFilename}").PutAsync(stream, cancellationToken);
        }

        public async Task DeleteFile(string filename)
        {
            await new FirebaseStorage(_bucket).Child("assets").Child(filename).DeleteAsync();
        }

        public async Task<string> GetUrlFromFilenameAsync(string filename)
        {
            return await new FirebaseStorage(_bucket).Child("assets").Child($"{filename}").GetDownloadUrlAsync();
        }
    }
}
