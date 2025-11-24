using FoodBev.Application.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FoodBev.Infrastructure.Persistence.Services
{
    /// <summary>
    /// Implements file storage service using local file system.
    /// In production, consider using cloud storage (Azure Blob, AWS S3, etc.).
    /// </summary>
    public class FileStorageService : IFileStorageService
    {
        private readonly string _basePath;

        public FileStorageService()
        {
            // Store files in wwwroot/uploads by default
            _basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(_basePath))
                Directory.CreateDirectory(_basePath);
        }

        public async Task<string> UploadFileAsync(string folder, byte[] fileBytes, string fileName)
        {
            if (fileBytes == null || fileBytes.Length == 0)
                throw new ArgumentException("File is empty or null.");

            // Create folder if it doesn't exist
            var folderPath = Path.Combine(_basePath, folder);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Generate unique filename
            var extension = Path.GetExtension(fileName);
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(folderPath, uniqueFileName);

            // Save file
            await File.WriteAllBytesAsync(filePath, fileBytes);

            // Return relative URL
            return $"/uploads/{folder}/{uniqueFileName}";
        }

        public Task<bool> DeleteFileAsync(string fileUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(fileUrl))
                    return Task.FromResult(false);

                // Remove leading slash if present
                var relativePath = fileUrl.TrimStart('/');
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return Task.FromResult(true);
                }

                return Task.FromResult(false);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }
    }
}

