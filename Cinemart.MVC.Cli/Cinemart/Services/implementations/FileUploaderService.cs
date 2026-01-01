using Cinemart.Configurations;
using Cinemart.Services.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace Cinemart.Services.Implementations
{
    public class FileUploaderService : IFileUploaderService
    {
        private readonly Cloudinary _cloudinary;

        public FileUploaderService(IOptions<CloudinarySettings> cloudinarySettings)
        {
            var account = new Account(
                cloudinarySettings.Value.CloudName,
                cloudinarySettings.Value.ApiKey,
                cloudinarySettings.Value.ApiSecret);

            _cloudinary = new Cloudinary(account);
        }

        public async Task<ImageUploadResult> AddFileAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0) 
            { 
                using var stream = file.OpenReadStream();
                var uploadParam = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "Cinemart",
                    Transformation = new Transformation().Height(500).Width(500)
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParam);
            }

            return uploadResult;
        }

        public async Task<string> DeleteFileAsync(string publicId)
        {
            var deleteParam = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParam);
            return result.Result == "OK" ? result.Result : null;
        }
    }
}
