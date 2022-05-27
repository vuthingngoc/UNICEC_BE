using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace UniCEC.Business.Services.FileSvc
{
    public class FileService : IFileService
    {

        public bool ValidationFile(IFormFile file)
        {
            const int MAX_SIZE = 10 * 1024 * 1024;
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

            string bucket = "unics-e46a4.appspot.com";

            Stream stream = file.OpenReadStream();
            var cancellationToken = new CancellationTokenSource().Token;
            return await new FirebaseStorage(bucket).Child("assets").Child($"{file.FileName}").PutAsync(stream, cancellationToken);
        }
    }
}
