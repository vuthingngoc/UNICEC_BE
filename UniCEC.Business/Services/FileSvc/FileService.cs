using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text.RegularExpressions;
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
            _bucket = _configuration.GetSection("Firebase:Bucket").Value;
        }

        public bool ValidationFile(IFormFile file)
        {
            const int MAX_SIZE = 5 * 1024 * 1024; // 5MB
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
            if (!isValid) throw new ArgumentException("Just image and the size is less than 5MB");

            Stream stream = file.OpenReadStream();
            var filename = Guid.NewGuid();

            return await Upload(filename, stream);
        }

        public async Task UploadFile(string oldFilename, IFormFile file)
        {
            bool isValid = ValidationFile(file);
            if (!isValid) throw new ArgumentException("Just image and the size is less than 5MB");

            Stream stream = file.OpenReadStream();
            var cancellationToken = new CancellationTokenSource().Token;
            await new FirebaseStorage(_bucket).Child("assets").Child($"{oldFilename}").PutAsync(stream, cancellationToken);
        }

        public async Task UploadFile(string oldFilename, string base64String)
        {
            Stream stream = ConvertBase64ToStream(base64String);
            await Upload(Guid.Parse(oldFilename), stream);
        }

        public async Task<string> UploadFile(string base64String)
        {
            Stream stream = ConvertBase64ToStream(base64String);
            var filename = Guid.NewGuid();
            return await Upload(filename, stream);
        }

        private Stream ConvertBase64ToStream(string base64)
        {
            base64 = base64.Trim();
            if ((base64.Length % 4 != 0) || !Regex.IsMatch(base64, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None)) throw new ArgumentException("Invalid image");
            byte[] bytes = Convert.FromBase64String(base64);
            return new MemoryStream(bytes);
        }

        private async Task<string> Upload(Guid filename, Stream stream)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            await new FirebaseStorage(_bucket).Child("assets").Child($"{filename}").PutAsync(stream, cancellationToken);
            return filename.ToString();
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
