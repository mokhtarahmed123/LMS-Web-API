using LMS.Service.Abstract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace LMS.Service.implementation
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public Task DeleteFileAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return Task.CompletedTask;


            var fullPath = Path.GetFullPath(filePath);

            if (File.Exists(fullPath))
                File.Delete(fullPath);

            return Task.CompletedTask;
        }

        public async Task<string> UploadFile(string Location, IFormFile file)
        {

            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty");



            var extension = Path.GetExtension(file.FileName).ToLower();
            string[] allowedExtensions = { ".pdf", ".zip", ".docx", ".pptx", ".jpg", ".png", ".xlsx" };

            if (!allowedExtensions.Contains(extension))
                throw new InvalidOperationException("Invalid File extension");

            long maxSize = 40 * 1024 * 1024;
            if (file.Length > maxSize)
                throw new InvalidOperationException("File size exceeded");

            var path = Path.Combine(_webHostEnvironment.WebRootPath, Location);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var fileName = $"{Guid.NewGuid():N}{extension}";
            var fullPath = Path.Combine(path, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/{Location}/{fileName}";
        }


        public async Task<string> UploadImage(string Location, IFormFile file)
        {
            var path = _webHostEnvironment.WebRootPath + "/" + Location + "/";
            var Extension = Path.GetExtension(file.FileName);
            var fileName = Guid.NewGuid().ToString().Replace("-", string.Empty) + Extension;
            string[] allowedExtensions = { ".jpg", ".png" };
            if (file.Length > 0)
            {
                if (file.Length > (7 * 1024 * 1024))
                    return "Maximum Size Can Be 7mb";


                if (!allowedExtensions.Contains(Extension))
                    return $"Invalid file type With Extension {Extension}";

                if (Directory.Exists(path) == false)
                {
                    Directory.CreateDirectory(path);
                }

                using (FileStream fileStream = File.Create(path + fileName))
                {
                    await file.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                    return $"/{Location}/{fileName}";
                }

            }
            return "File is null or empty.";
        }

        public async Task<string> UploadVideo(string location, IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty");

            if (!file.ContentType.StartsWith("video/"))
                throw new InvalidOperationException("Invalid video content");

            var extension = Path.GetExtension(file.FileName).ToLower();
            string[] allowedExtensions = { ".mp4", ".avi", ".mov", ".mkv" };

            if (!allowedExtensions.Contains(extension))
                throw new InvalidOperationException("Invalid video extension");

            long maxSize = 400 * 1024 * 1024;
            if (file.Length > maxSize)
                throw new InvalidOperationException("File size exceeded");

            var path = Path.Combine(_webHostEnvironment.WebRootPath, location);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var fileName = $"{Guid.NewGuid():N}{extension}";
            var fullPath = Path.Combine(path, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/{location}/{fileName}";
        }
    }
}