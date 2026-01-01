using CloudinaryDotNet.Actions;

namespace Cinemart.Services.Interfaces
{
    public interface IFileUploaderService
    {
        Task<ImageUploadResult> AddFileAsync(IFormFile file);
        Task<string> DeleteFileAsync(string  publicId);
    }
}
