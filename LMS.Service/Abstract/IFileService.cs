using Microsoft.AspNetCore.Http;

namespace LMS.Service.Abstract
{
    public interface IFileService
    {
        Task<string> UploadImage(string Location, IFormFile file);
        Task DeleteFileAsync(string filePath);

        Task<string> UploadVideo(string Location, IFormFile file);

        Task<string> UploadFile(string Location, IFormFile file);


    }
}
